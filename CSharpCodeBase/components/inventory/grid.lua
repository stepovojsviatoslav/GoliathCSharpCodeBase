local Class = require 'utils.hump.class'
local InventoryItem = require 'components.inventory.item'

local InventoryGrid = Class({init=function(self, count, name)
      self._count = count or 4
      self._grid = {}
      self._name = name
      for i = 1, self._count do
        self._grid[i] = nil
      end
end})

function InventoryGrid:SetItem(index, item)
  self._grid[index] = item
end

function InventoryGrid:GetItem(index)
  return self._grid[index]
end

function InventoryGrid:GetItemCount(item)
  local sum = 0
  for k, v in pairs(self._grid) do
    if v.name == item then
      sum = sum + v.count
    end
  end
  return sum
end

-- Return count of removed elements
function InventoryGrid:RemoveFromSlot(idx, count)
  local slotCount = self._grid[idx].count
  local returnCount = 0
  slotCount = slotCount - count
  if slotCount < 0 then
    returnCount = -slotCount
    slotCount = 0
  end
  self._grid[idx].count = slotCount
  GameController.eventSystem:Event("INVENTORY_SLOT_CHANGED_CORE", {self._name, idx, self._grid[idx].name, self._grid[idx].count})
  return returnCount
end

function InventoryGrid:RemoveItem(item, count)
  local rest = count
  for i = 1, self._count do
    if self._grid[i] ~= nil and self._grid[i].name == item then
      if rest > self._grid[i].count then
        rest = rest - self._grid[i].count
        self._grid[i].count = 0
        GameController.eventSystem:Event("INVENTORY_SLOT_CHANGED_CORE", {self._name, i, self._grid[i].name, self._grid[i].count})
      else
        self._grid[i].count = self._grid[i].count - rest
        rest = 0
        GameController.eventSystem:Event("INVENTORY_SLOT_CHANGED_CORE", {self._name, i, self._grid[i].name, self._grid[i].count})
      end
    end
    if rest == 0 then
      break
    end
  end
  return rest
end

function InventoryGrid:GetItems()
  return self._grid
end

function InventoryGrid:GetFilteredItems(tag)
  local tempTable = {}
  for k, v in pairs(self._grid) do 
    if(v.name and GameController.database:Get('items', v.name ..'/tag') == tag and v.count > 0) then
      table.insert(tempTable, v)
    end
  end
  return tempTable
end

-- Return rest item (if count is 0, then item added)
function InventoryGrid:AddItem(newItem)
  -- try to find item, and add as counter
  local addItem = newItem:Clone()
  local stackLimit = GameController.database:Get('items', addItem.name .. '/stackLimit')
  for i = 1, self._count do
    local item = self._grid[i]
    if item ~= nil and item.name == addItem.name and item.count > 0 then
      local restItemsToStack = stackLimit - item.count
      if restItemsToStack >= addItem.count then
        item.count = item.count + addItem.count
        GameController.eventSystem:Event("INVENTORY_SLOT_CHANGED_CORE", {self._name, i, item.name, item.count})
        addItem.count = 0
        break
      else
        addItem.count = addItem.count - restItemsToStack
        item.count = item.count + restItemsToStack
        GameController.eventSystem:Event("INVENTORY_SLOT_CHANGED_CORE", {self._name, i, item.name, item.count})
      end
    end
  end
  
  -- try to find empty place, and insert item
  for i = 1, self._count do
    if self._grid[i] == nil or self._grid[i].count == 0 then
      if self._grid[i] == nil then
        self._grid[i] = InventoryItem(nil, 0)
      end
      self._grid[i].name = addItem.name
      if addItem.count > stackLimit then
        self._grid[i].count = stackLimit
      else
        self._grid[i].count = addItem.count
      end
      addItem.count = addItem.count - self._grid[i].count
      GameController.eventSystem:Event("INVENTORY_SLOT_CHANGED_CORE", {self._name, i, self._grid[i].name, self._grid[i].count})
      if addItem.count == 0 then
        break
      end
    end
  end
  
  -- Have no place
  return addItem
end

return InventoryGrid