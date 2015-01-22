local Class = require 'utils.hump.class'

local List = Class({init = function(self)
  self._array = {}  
end})

function List:Add(item)
  self._array[#self._array + 1] = item
end

function List:Remove(item)
  local idx = Tables.Find(self._array, item)
  if idx > - 1 then
    table.remove(self._array, idx)
  end
end

function List:Clear()
  self._array = {}
end

function List:Contains(item)
  return Tables.Find(self._array, item) > -1
end

function List:__len()
  return #self._array
end

function List:GetTable()
  return self._array
end

function List:IsEmpty()
  return #self._array == 0
end

return List