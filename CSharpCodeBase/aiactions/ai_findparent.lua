local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'

-- We need to find parent, if parent not exists
local AI_FindParent = Class({__includes=AINode, init=function(self, entity)
  AINode.init(self, entity)
end})

function AI_FindParent:Visit()
  if self.status == NODE_READY then
    local parent = self.entity.relationship:GetInstance("parent")
    if parent == nil then
      parents = self.entity.vision:GetVisibleEntitiesByRSTag("parent")
      if #parents > 0 then
        self.entity.relationship:AddInstance("parent", parents[1], -1)
        self.status = NODE_SUCCESS
      else
        self.status = NODE_FAILURE
      end
    else
      self.status = NODE_SUCCESS
    end
  end
  self:Sleep(0.5)
end

return AI_FindParent