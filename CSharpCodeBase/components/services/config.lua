local Class = require 'utils.hump.class'

local Config = Class({init=function(self, table, section)
      self.table = table
      self.section = section
end})

function Config:Get(path)
  return GameController.database:Get(self.table, self.section .. '/' .. path)
end

return Config