local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'

-- Move to enemy
local AI_RunawayFromTarget = Class({__includes=AINode, init = function (self, entity)
  AINode.init(self, entity)
  self.distance = self.entity.config:Get("scarySafeRadius")
end})

function AI_RunawayFromTarget:Visit()
  local target = self.parent.target
  if self.status == NODE_READY then
    self.status = NODE_RUNNING
  elseif self.status == NODE_RUNNING then
    if self.entity:GetSimpleDistance(target) > self.distance then
      self.status = NODE_SUCCESS
      self.entity.mover:Stop()
    else
      self.entity.mover:SetSpeed(self.entity.config:Get("scaryRunSpeed"))
      self.entity.mover:SetInputFromVec(target:GetPosition(), false, self.entity.config:Get("scaryRunType"))
    end
  end
end

return AI_RunawayFromTarget