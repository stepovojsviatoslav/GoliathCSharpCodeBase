local Class = require 'utils.hump.class'

local Item = Class({init=function(self, name, count, tag)
    self.name = name
    self.count = count
end})

function Item:Clone()
  return Item(self.name, self.count)
end

return Item