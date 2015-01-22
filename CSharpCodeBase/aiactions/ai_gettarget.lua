local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'

-- We need to find parent, if parent not exists
local AI_GetTarget = Class({__includes=AINode, init=function(self, entity)
  AINode.init(self, entity)
end})

function AI_GetTarget:Visit()
  if self.status == NODE_READY then
    local entities = self.entity.vision:GetVisibleEntitiesByRSTag("scary",
      self.entity.config:Get("scaryRadius"))
    if #entities > 0 then
      self.status = NODE_SUCCESS
      self.parent.target = entities[1]
    else
      self.status = NODE_FAILURE
    end
  end
  self:Sleep(0.5)
end

return AI_GetTarget