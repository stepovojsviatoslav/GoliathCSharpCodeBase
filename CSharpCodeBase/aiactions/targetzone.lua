local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'

local AI_TargetZone = Class({__includes=AINode, init = function (self, entity)
  AINode.init(self, entity)
  self.targetZoneRadius=self.entity.config:Get("targetZoneRadius")
  self.targetZoneTimeout=self.entity.config:Get("targetZoneTimeout")
  self:ResetTimeoutAndTarget()
  self.currentTarget = nil
end})

function AI_TargetZone:ResetTimeoutAndTarget()
  self.timeout = self.targetZoneTimeout
  self.currentTarget=nil
end

function AI_TargetZone:Visit()
  if self.status == NODE_READY then
    self:ResetTimeoutAndTarget()
    local possibleTargets = self.entity.vision:GetVisibleEntitiesByRSTag("alertness")
    self.currentTarget = possibleTargets[1]
    if self.currentTarget ~= nil then
      self.status = NODE_RUNNING
      self.entity.mover:Stop()
    else
      self.status = NODE_FAILURE
      self:Sleep(0.5)
    end
  end
  
  if self.status == NODE_RUNNING then
    if self.entity:GetSimpleDistance(self.currentTarget) > self.targetZoneRadius then
      self.status = NODE_FAILURE
    else
      self.entity.mover:LookAt(self.currentTarget:GetPosition())
      self.timeout = self.timeout - GameController.deltaTime
      if self.timeout < 0 then
        self.entity.relationship:AddInstance("enemy", self.currentTarget)
        self.entity._screamTrigger = true
        self.entity._screamTarget = self.currentTarget
        self.status = NODE_SUCCESS
      end
    end
  end
end

return AI_TargetZone