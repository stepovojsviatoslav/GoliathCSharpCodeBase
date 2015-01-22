local Class = require 'utils.hump.class'
local InventoryGrid = require 'components.inventory.grid'
local InventoryItem = require 'components.inventory.item'

local Inventory = Class({init=function(self)
      self.grids = {}
      self.grids.inventory = InventoryGrid(28, "inventory")
      --self.grids.gadgets = InventoryGrid(4, "gadgets")
      self.grids.equipment = InventoryGrid(5, "equipment")
end})

function Inventory:AddItem(item, count)
  if count ~= nil then
    item = InventoryItem(item, count)
  end
  local restItem = self.grids.inventory:AddItem(item)
  if restItem.count > 0 then
    restItem = self.grids.gadgets:AddItem(restItem)
  end
  return restItem
end

function Inventory:GetItemCount(item)
  local sum = 0
  for k, v in pairs(self.grids) do
    sum = sum + v:GetItemCount(item)
  end
  return sum
end

function Inventory:SetSlotFromUI(container, idx, item, count)
  self.grids[container]:SetItem(idx, InventoryItem(item, count))
end

function Inventory:GetSlotCount(container, idx)
  return self.grids[container]:GetItem(idx).count
end

function Inventory:RemoveFromSlot(container, idx, count)
  return self.grids[container]:RemoveFromSlot(idx, count)
end

function Inventory:RemoveItems(item, count)
  local rest = count
  for k, v in pairs(self.grids) do
    rest = v:RemoveItem(item, rest)
    if rest == 0 then
      break
    end
  end
end

return Inventory
