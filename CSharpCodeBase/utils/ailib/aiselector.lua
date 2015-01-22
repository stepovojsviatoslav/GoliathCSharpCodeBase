local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'

local AISelectorNode = Class({__includes=AINode, init = function (self, entity, childNodes, reactive)
      AINode.init(self, entity, childNodes)
      self.idx = 1
      self.reactive = reactive and true or false
end})

function AISelectorNode:Reset()
  AINode.Reset(self)
  self.idx = 1
end

function AISelectorNode:Visit()
  if self.status ~= NODE_RUNNING or self.reactive then
    self.idx = 1
  end
 
  while self.idx <= #self.childNodes do
    local childNode = self.childNodes[self.idx]
    local childStatus = NODE_FAILURE
    
    if not childNode:IsSleeping() then
      childNode:Visit()
      childStatus = childNode.status
    end
    
    if childStatus == NODE_RUNNING or childStatus == NODE_SUCCESS then
      self.status = childNode.status
      return
    end
    self.idx = self.idx + 1
  end
  
  self.status = NODE_FAILED
end

return AISelectorNode