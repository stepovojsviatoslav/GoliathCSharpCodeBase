local Class = require 'utils.hump.class'

-- Status codes
B_FAILED = 0
B_FAILURE = B_FAILED
B_SUCCESS = 1
B_RUNNING = 2

-- Actions
local Action = Class({init = function(self, func)
    self.func = func
    self.returnCode = B_FAILURE
end})

function Action:Behave(creature)
  self.returnCode = self.func(creature)
  return self.returnCode
end

return Action