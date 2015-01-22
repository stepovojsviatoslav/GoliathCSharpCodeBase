local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'

local AI_ReturnHome = Class({__includes=AINode, init = function (self, entity)
  AINode.init(self, entity)
  self.timeout = nil
  self.radius = self.entity.config:Get("returnHomeRadius")
end})

function AI_ReturnHome:Visit()
  if self.entity.isChild and self.entity.relationship:GetInstance('parent') ~= nil then
    self.status = NODE_FAILURE
    return
  end
  
  if self.status == NODE_READY then
    self.status = NODE_FAILURE
    if self.entity.worldmap:GetDistanceTo("home") > self.radius and self.timeout ~= nil then
      self.timeout = self.timeout - GameController.deltaTime
    else
      self.timeout = self.entity.config:Get("returnHomeTimeout")
    end
  end
  
  if self.timeout < 0 and self.status ~= NODE_RUNNING then
    self.status = NODE_RUNNING
    self.entity.mover:ResetSpeed()
    self.entity.mover:SetGoal(self.entity.worldmap:GetLocation('home'))
  elseif self.status == NODE_RUNNING then
    if not self.entity.mover:IsHaveGoal() then
      self.status = NODE_SUCCESS
    end
  end
  return self.status
end

return AI_ReturnHome