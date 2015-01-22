local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'

local AI_ConditionalNode = Class({__includes=AINode, init = function (self, entity, conditional)
      AINode.init(self, entity)
      self.conditional = conditional
end})

function AI_ConditionalNode:Visit()
  if self.status == NODE_READY then
    self.status = self.conditional(self.entity, self) and NODE_SUCCESS or NODE_FAILURE
  end
end

return AI_ConditionalNode