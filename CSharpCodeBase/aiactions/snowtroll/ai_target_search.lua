local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'

local AI_TargetSearch = Class({__includes=AINode, init = function (self, entity, callback)
  AINode.init(self, entity)
  self.callback = callback
  self:ResetTarget()
end})

function AI_TargetSearch:ResetTarget()
  self.currentTarget=nil
end

function AI_TargetSearch:Visit()
  if self.status == NODE_READY then
    self:ResetTarget()
    local possibleTargets = self.entity.vision:GetVisibleEntitiesByRSTag("alertness") or self.entity.vision:GetVisibleEntitiesByRSTag("enemy")
    self.currentTarget = possibleTargets[1]
    if self.currentTarget ~= nil then
      self.status = NODE_RUNNING
    else
      self.status = NODE_FAILURE
    end
  end
  
  if self.status == NODE_RUNNING then
    self.entity.goalProsecution = self.currentTarget
    self.entity.lead = self.entity
    self.entity._screamTrigger = true
    self.entity._screamTarget = self.currentTarget
    self.status = NODE_SUCCESS
    self.callback(self.entity, self)
  end
end

return AI_TargetSearch