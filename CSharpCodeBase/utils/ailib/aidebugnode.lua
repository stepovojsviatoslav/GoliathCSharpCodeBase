local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'

local AI_DebugNode = Class({__includes=AINode, init = function (self, entity, result, string)
      AINode.init(self, entity)
      self.result = result
      self.string = string
end})


function AI_DebugNode:Visit()
  print(string)
  self.status = self.result
end

return AI_DebugNode