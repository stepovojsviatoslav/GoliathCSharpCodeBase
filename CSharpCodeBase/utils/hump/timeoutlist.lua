local Class = require 'utils.hump.class'

local TimeoutList = Class({init = function (self, globaltimeout)
      self._items = {}
      self._times = {}
      self._globaltimeout = globaltimeout
end})

function TimeoutList:Exists(item)
  local idx = Tables.Find(self._items, item)
  return idx > - 1
end

function TimeoutList:Add(item, timeout)
  timeout = timeout or self._globaltimeout
  local idx = Tables.Find(self._items, item)
  if idx > -1 then
    self._times[idx] = os.clock() + timeout
  else
    self._items[#self._items + 1] = item
    self._times[#self._times + 1] = os.clock() + timeout
  end
end

function TimeoutList:Remove(item)
  local idx = Tables.Find(self._items, item)
  if idx > -1 then
    table.remove(self._items, idx)
    table.remove(self._times, idx)
  end
end

function TimeoutList:SetGlobalTimeout(timeout)
  self._globaltimeout = timeout
end

function TimeoutList:IsEmpty()
  return #self._items == 0
end

function TimeoutList:Update()
  if self._globaltimeout == -1 then return end
  for k, v in pairs(self._times) do
    if os.clock() > v then
      table.remove(self._items, k)
      table.remove(self._times, k)
      break
    end
  end
end

function TimeoutList:GetData()
  return self._items
end

return TimeoutList