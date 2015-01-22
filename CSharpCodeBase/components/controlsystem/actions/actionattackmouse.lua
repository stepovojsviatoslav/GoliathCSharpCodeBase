local Action = require 'components.controlsystem.action'
local Class = require 'utils.hump.class'
local Vector3 = require 'utils.hump.vector3'
local HitTester = require 'utils.hittest'

local STATE_MOVING = 0
local STATE_REQUEST_ATTACK = 1
local STATE_ATTACK = 2

local ActionFastAttack = Class({__includes=Action, init = function (self, entity, target)
      Action.init(self, 2, false, 'fastattack')
      self.entity = entity
      self.target = target
      self.state = STATE_REQUEST_ATTACK
end})

function ActionFastAttack:OnPushed()
  self.priority = 3
end

function ActionFastAttack:OnStartRunning()
  print("Start attacking!")
  --self.priority = 3
  self.state = STATE_REQUEST_ATTACK
  self.entity.combat:AttackVector(self.target)
  self.entity.mover:SetSpeed(self.entity.config:Get('moverSpeed') / 4)
end

function ActionFastAttack:OnStopRunning()
  self.entity.mover:ResetSpeed()
  Action.OnStopRunning(self)
  --self.entity.mecanim:ForceSetState('Empty', 1)
end


function ActionFastAttack:Update()
  self:ProcessMoving()
  return self.state == STATE_ATTACK 
end

function ActionFastAttack:ProcessMoving()
  if GameController.inputService:LeftStickYIsPressed() or  GameController.inputService:LeftStickXIsPressed() then
    self.currentInput = GameController.inputService:LeftStickValues()
    self.currentInput:RotateAroundY(GameController.camera.angle) 
    self.entity.mover:SetInput(self.currentInput)
  end
end

function ActionFastAttack:OnEvent(data)
  if data == "punch" then  
    self.state = STATE_ATTACK
    -- Get damage objects
    local miss = true
    if self.entity.weaponContainer.currentWeapon.remote > 0 then
      -- shoot
      local sourceForward = Transform.GetForwardVector(self.entity.transform)
      self.entity.weaponContainer:Attack(self.entity:GetPosition() + self.target)
    else
      local nearEntities = RaycastUtils.GetEntitiesInRadius(self.entity:GetPosition(), 5)
      local sourceForward = Transform.GetForwardVector(self.entity.transform)
      for k, v in pairs(nearEntities) do
        if v ~= self.entity and v.interactable then
          if HitTester.CheckHitEntity(self.entity, sourceForward, v, 45, 1.5) then
            self.entity.damageProcessor:SendDamage(v, self.entity.weaponContainer.currentWeapon:GetDamage())
            if GameController.inputService:IsGamepad() and self.entity.gamepadRightStickController:GetTarget() == nil and v.gameObject.tag ~= 'Player' and v.gameObject.tag == 'Enemy' then
              self.entity.gamepadRightStickController:SetTarget(v)
            end
            miss = false
          end
        end
      end
    end
    if miss then
      self.entity.combat:ResetCombo()
    end
  end
end

function ActionFastAttack:IsContinuous()
  return false
end

return ActionFastAttack