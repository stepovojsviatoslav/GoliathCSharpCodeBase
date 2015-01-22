local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'

-- Move to enemy
local AI_FollowParent = Class({__includes=AINode, init = function (self, entity)
  AINode.init(self, entity)
  self.radiusMax = self.entity.config:Get("parentSafeRadiusMax")
  self.radiusMin = self.entity.config:Get("parentSafeRadiusMin")
end})

function AI_FollowParent:Visit()
  local parent = self.entity.relationship:GetInstance("parent")
  if self.status == NODE_READY then
    if self.entity:GetSimpleDistance(parent) < self.radiusMax then
      -- nothing to do, we are in safe-zone
      self.status = NODE_SUCCESS
    else
      -- we need move to parent
      self.status = NODE_RUNNING
    end
  elseif self.status == NODE_RUNNING then
    if parent == nil then 
      self.status = NODE_FAILURE
    elseif self.entity:GetSimpleDistance(parent) > self.radiusMin then
      self.entity.mover:ResetSpeed()
      self.entity.mover:SetInputToVec(parent:GetPosition())
    else
      self.status = NODE_SUCCESS
    end
  end
end

return AI_FollowParent