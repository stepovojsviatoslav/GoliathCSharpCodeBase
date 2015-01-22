local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'

-- Select enemy using ai vision
local AI_SelectEnemy = Class({__includes=AINode, init=function(self, entity)
  AINode.init(self, entity)
end})

function AI_SelectEnemy:Visit()
  if self.status == NODE_READY then
    self.parent.selected_target = self.entity:GetPriorityTarget()
    self.status = self.parent.selected_target ~= nil and NODE_SUCCESS or NODE_FAILURE
  end
  return self.status
end

return AI_SelectEnemy