local Class = require 'utils.hump.class'

local AITree = Class({init=function (self, entity, root)
  self.entity = entity
  self.root = root
end})

function AITree:Process()
  self.root:Visit()
  self.root:Save()
  self.root:Process()
end

function AITree:Reset()
  self.root:Reset()
end

function AITree:OnEvent(data)
  self.root:OnEvent(data)
end

return AITree