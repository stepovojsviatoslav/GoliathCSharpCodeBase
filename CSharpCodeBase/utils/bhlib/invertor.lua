local Class = require 'utils.hump.class'

local Inverter = Class({init = function(self, cond)
      self.cond = cond
      self.returnCode = B_FAILURE
end})

function Inverter:Behave(creature)
  result = self.cond:Behave(creature) == B_SUCCESS and B_FAILURE or B_SUCCESS
  self.returnCode = result
  return self.returnCode
end

return Inverter