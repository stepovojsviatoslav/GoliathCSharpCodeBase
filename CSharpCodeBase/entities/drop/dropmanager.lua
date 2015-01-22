local Class = require 'utils.hump.class'

local DropManager = Class({init=function(self)
end})

function DropManager:CreateDrop(items, position, sourceEntity)
  for k,v in pairs(items) do
    if v > 0 then
      self:DropItem(k, v, position, sourceEntity)
    end
  end
end

function DropManager:DropItem(item, count, position, sourceEntity, forceVector)
  local entity = GameController.entityFactory:CreateInWorld(item, position)
  entity:SetupDrop(item, count, sourceEntity, forceVector)
  return entity
end

return DropManager