local Class = require 'utils.hump.class'
local Entity = require 'unity.entity'
local RecipeList = require 'components.inventory.recipelist'

local MedicineChestEntity= Class({__includes=Entity, init=function(self, entity, name)            
  Entity.init(self)
  self.name = name
  self.owner = entity
end})

function MedicineChestEntity:Apply()
  if self.owner.characterClass == 0 then
    self.owner:Message('Increase',  GameController.database:Get('items', self.name ..'/hpcount'))
    GameController.inventory:RemoveItems(self.name, 1)
  end
end

return MedicineChestEntity