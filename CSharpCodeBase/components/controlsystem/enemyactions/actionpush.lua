local Action = require 'components.controlsystem.action'
local Class = require 'utils.hump.class'

local ActionPush = Class({__includes=Action, init = function (self, entity, targetPosition)
      Action.init(self, 4, false, 'push')
      self.entity = entity
      self.targetPosition = targetPosition
end})

function ActionPush:OnStartRunning()
  --Action.OnStart(self)
  self.entity.mover:Stop()
  self.entity.mecanim:SetFloat("action_type", 1)
  self.entity.mecanim:ResetTrigger("action")
  self.entity.mecanim:ForceSetState("Hit")
  self.dv = self.entity:GetPosition() - self.targetPosition
  self.dv.y = 0
  self.dv:Normalize()
  --print(self.targetPosition.x .. ',' .. self.targetPosition.z)
  self.isHitStarted = false
end

function ActionPush:Update()
  if not self.isHitStarted then
    self.isHitStarted = self.entity.mecanim:CheckStateName("Hit")
  end
  
  return self.isHitStarted and not self.entity.mecanim:CheckStateName("Hit")
end

function ActionPush:FixedUpdate()
  self.entity.mover:LookAt(self.targetPosition)
  local speedCurve = self.entity.mecanim:GetFloat('speed_curve')
  --self.entity.mover:SetInput(self.dv * speedCurve)
  RigidbodyUtils.MoveNotRotate(self.entity.rigidbody, self.dv * speedCurve * 10)
end

function ActionPush:OnStopRunning()
  self.entity.mover:Stop()
end

return ActionPush