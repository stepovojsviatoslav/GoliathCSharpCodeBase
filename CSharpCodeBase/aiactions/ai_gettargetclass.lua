local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'

-- We need to find parent, if parent not exists
local AI_GetTarget = Class({__includes=AINode, init=function(self, entity)
  AINode.init(self, entity)
end})

function AI_GetTarget:Visit()
  if self.status == NODE_READY then
    self.parent.target = nil
    local entities = self.entity.vision:GetVisibleEntities(self.entity.config:Get('scaryRadius'))
    for k, v in pairs(entities) do
      if v.characterClass ~= nil and v.characterClass > self.entity.characterClass then
        self.parent.target = v
        break
      end
    end
    if self.parent.target ~= nil then
      self.status = NODE_SUCCESS
    else
      self.status = NODE_FAILURE
    end
  end
  self:Sleep(0.5)
end

return AI_GetTarget