local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'

-- Select enemy using ai vision
local AI_Timeout = Class({__includes=AINode, init=function(self, entity, timeout)
  AINode.init(self, entity)
  self.timeout = timeout
  self.currentTimeout = timeout
end})

function AI_Timeout:Visit()
  if self.status == NODE_READY then
    self.status = NODE_RUNNING
    self.currentTimeout = self.timeout
  end
  if self.status == NODE_RUNNING  then
    --[[
    if self.parent.selected_target ~= nil then
      self.entity.mover:LookAt(self.parent.selected_target:GetPosition())
    end
    ]]
    self.currentTimeout = self.currentTimeout - GameController.deltaTime
    if self.currentTimeout <= 0 then
      self.status = NODE_SUCCESS
    end
  end
  return self.status
end

return AI_Timeout