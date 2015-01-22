local Action = require 'components.controlsystem.action'
local Class = require 'utils.hump.class'
local Vector3 = require 'utils.hump.vector3'
local HitTester = require 'utils.hittest'

local SpellAction = Class({__includes=Action, init = function (self, name)
      Action.init(self, 3, false, name)
      self.inState = false
      
      self.radius = GameController.database:Get('spells', name .. '/punch_radius')
      self.effect = GameController.database:Get('spells', name .. '/punch_effect')
      self.damage = {}
      local damageString = GameController.database:Get('spells', name .. '/punch_damage')
      for dType, dValue in string.gmatch(damageString, '(%w+)%:(%d+)%,?') do
        self.damage[dType] = dValue
      end            
end})

function SpellAction:OnStartRunning()  
  -- call spell animation
  self.entity.mover:Stop()
  local pos = Input.RaycastTerrain(self.position)
  self.entity.mecanim:ForceSetState("SpellDownkick")
  local dv = pos - self.entity:GetPosition()
  --self.speed = 1.26 / dv:Length()
  self.speed = dv:Length() / 1
  self.direction = dv:Normalize()
  self.moving = true
end

function SpellAction:OnStopRunning()
  -- stop spell animation?
  self.entity.mover:Stop()
end


function SpellAction:ProcessMoving()
  RigidbodyUtils.Move(self.entity.rigidbody, self.direction * self.speed)
end

function SpellAction:Punch()
  local entities = RaycastUtils.GetEntitiesInRadius(self.entity:GetPosition(), self.radius)
  for k, v in pairs(entities) do
    if v ~= self.entity then
      local effects = {}
      effects[self.effect] = true
      self.entity.damageProcessor:SendDamage(v, {damage=self.damage, effects=effects})
    end
  end  
end

function SpellAction:FixedUpdate()
  if self.moving then
    self:ProcessMoving()
  end
end

function SpellAction:Update()
  if not self.inState then
    self.inState = self.entity.mecanim:CheckStateName("SpellDownkick")
  end
  return self.inState and not self.entity.mecanim:CheckStateName("SpellDownkick")
end

function SpellAction:OnEvent(data)
  -- Cast spell by animation here
  if data == 'punch' then
    self.moving = false
    self.entity.mover:Stop()
    self:Punch()
  end
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