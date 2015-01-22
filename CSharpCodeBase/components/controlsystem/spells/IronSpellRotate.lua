local Action = require 'components.controlsystem.action'
local Class = require 'utils.hump.class'
local Vector3 = require 'utils.hump.vector3'
local HitTester = require 'utils.hittest'

local STATE_MOVING = 0
local STATE_REQUEST_ATTACK = 1
local STATE_ATTACK = 2

local SpellAction = Class({__includes=Action, init = function (self, name)
      Action.init(self, 3, false, name)
      self.inState = false
      self.punch_timeout = GameController.database:Get('spells', name .. '/punch_timeout')
      self.radius = GameController.database:Get('spells', name .. '/punch_radius')
      self.timeout = self.punch_timeout
      self.damage = {}
      local damageString = GameController.database:Get('spells', name .. '/punch_damage')
      for dType, dValue in string.gmatch(damageString, '(%w+)%:(%d+)%,?') do
        self.damage[dType] = dValue
      end      
end})

function SpellAction:OnStartRunning()  
  -- call spell animation
  self.entity.mecanim:ForceSetState("SpellRotate")
end

function SpellAction:OnStopRunning()
  -- stop spell animation?
end


function SpellAction:ProcessMoving()
  if GameController.inputService:LeftStickYIsPressed() or  GameController.inputService:LeftStickXIsPressed() then
    self.currentInput = GameController.inputService:LeftStickValues()
    self.currentInput:RotateAroundY(GameController.camera.angle) 
    self.entity.mover:SetInput(self.currentInput)
  end
end

function SpellAction:Punch()
  local entities = RaycastUtils.GetEntitiesInRadius(self.entity:GetPosition(), self.radius)
  for k, v in pairs(entities) do
    if v ~= self.entity then
      self.entity.damageProcessor:SendDamage(v, {damage=self.damage, effects={}})
    end
  end
end

function SpellAction:Update()
  self:ProcessMoving()
  self.timeout = self.timeout - GameController.deltaTime
  if self.timeout < 0 then
    self:Punch()
    self.timeout = self.punch_timeout
  end
  if not self.inState then
    self.inState = self.entity.mecanim:CheckStateName("SpellRotate")
  end
  return self.inState and not self.entity.mecanim:CheckStateName("SpellRotate")
end

function SpellAction:OnEvent(data)
  -- Cast spell by animation here
end

function SpellAction:IsContinuous()
  return false
end

function SpellAction:BeginDraw()
end

function SpellAction:StopDraw()
end

function SpellAction.CanApply()
  return true
end

return SpellAction