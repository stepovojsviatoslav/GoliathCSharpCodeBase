local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'

local AISequenceNode = Class({__includes=AINode, init = function (self, entity, childNodes)
      AINode.init(self, entity, childNodes)
      self.idx = 1
end})

function AISequenceNode:Reset()
  AINode.Reset(self)
  self.idx = 1
end

function AISequenceNode:Visit()
  if self.status ~= NODE_RUNNING then
    self.idx = 1
  end
  
  while self.idx <= #self.childNodes do
    local childNode = self.childNodes[self.idx]
    local childStatus = NODE_FAILURE
    if not childNode:IsSleeping() then
      childNode:Visit()
      childStatus = childNode.status
    end
    if childStatus == NODE_RUNNING or childStatus == NODE_FAILURE then
      if childNode:IsSleeping() then
        self.status = NODE_FAILURE
      else
        self.status = childNode.status
      end
      return
    end
    self.idx = self.idx + 1
  end
  
  self.status = NODE_SUCCESS
end

return AISequenceNode