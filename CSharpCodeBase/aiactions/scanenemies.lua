local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'

local AI_ScanEnemies = Class({__includes=AINode, init=function(self, entity)
  AINode.init(self, entity)
end})

function AI_ScanEnemies:Visit()
  if self.status == NODE_READY then
    self.entity:ScanEnemies()
    self.status = NODE_FAILURE -- keep it failure to running tree
  end
  self:Sleep(0.3)
  return self.status
end

return AI_ScanEnemies