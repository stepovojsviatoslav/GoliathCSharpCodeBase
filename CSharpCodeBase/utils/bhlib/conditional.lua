local Class = require 'utils.hump.class'

-- Conditionals
local Conditional = Class({init = function(self, func)
    self.func = func
    self.returnCode = B_FAILURE
end})

function Conditional:Behave(creature)
  self.returnCode = self.func(creature) and B_SUCCESS or B_FAILURE
  return self.returnCode
end

function Conditional:Bool(creature)
  return self.func(creature)
end

return Conditional