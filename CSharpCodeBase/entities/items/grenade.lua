local Class = require 'utils.hump.class'
local Entity = require 'unity.entity'
local RecipeList = require 'components.inventory.recipelist'

local GrenadeInventoryEntity= Class({__includes=Entity, init=function(self, entity, name)            
  Entity.init(self)
  self.name = name
  self.owner = entity
end})

function GrenadeInventoryEntity:Apply()
    self.owner.grenadeModeVisualizer:Disable()
    self.owner.grenadeModeVisualizer:Enable(self.name)
end

return GrenadeInventoryEntity