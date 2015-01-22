local Class = require 'utils.hump.class'

local ObjectPool = Class({init=function(self, class)
      self._class = class
      self._poolLength = 1
      self._curPoint = 1
      self._pool = {}
      self:Init()
end})

function ObjectPool:Init()
  for i = 1, self._poolLength do
    if self._pool[i] == nil then
      self._pool[i] = self._class()
      self._pool[i].__pool = self
    end
  end
end

function ObjectPool:Fetch(...)
  if self._curPoint > self._poolLength then
    self._poolLength = self._poolLength + 1
    self:Init()
  end
  local object = self._pool[self._curPoint]
  self._curPoint = self._curPoint + 1
  if object.OnFetch then object:OnFetch(...) end
  return object
end

function ObjectPool:Release(object)
  self._curPoint = self._curPoint - 1
  self._pool[self._curPoint] = object
  if object.OnRelease then object:OnRelease() end
end

return ObjectPool