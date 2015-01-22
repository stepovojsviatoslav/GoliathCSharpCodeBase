local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'

-- Select enemy using ai vision
local AI_InviteToHunt = Class({__includes=AINode, init=function(self, entity)
  AINode.init(self, entity)
end})

function AI_InviteToHunt:Visit()
  if self.status == NODE_READY then
    self.status = NODE_RUNNING
    self.entity:InviteToHunt()
  end
  if self.status == NODE_RUNNING then
    self.status = NODE_SUCCESS
  end
  return self.status
end

return AI_InviteToHunt