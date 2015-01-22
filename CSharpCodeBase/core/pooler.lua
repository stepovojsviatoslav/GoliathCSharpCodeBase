local Class = require 'utils.hump.class'
local Pool = require 'core.objectpool'

local Pooler = Class({init = function (self)
      self._pools = {}
end})

function Pooler:CreatePool(class)
  if self._pools[class] == nil then
    self._pools[class] = Pool(class)
  end
end

function Pooler:Fetch(class, ...)
  return self._pools[class]:Fetch(...)
end

function Pooler:Release(object)
  object.__pool:Release(object)
end

return Pooler