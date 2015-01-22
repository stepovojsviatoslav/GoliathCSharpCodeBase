local Class = require 'utils.hump.class'

local TimeService = Class({init=function(self)
  self.timers = {}
end})

function TimeService:Update()
  for key, value in pairs(self.timers) do
    value:StableUpdate()
  end
end

function TimeService:Add(component)
  table.insert(self.timers, component)
end

function TimeService:Remove(component)
  for key, value in pairs(self.timers) do
    if value == component then
      table.remove(self.timers, key)
      return
    end
  end
end

return TimeService