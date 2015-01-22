local Class = require 'utils.hump.class'

local Timer = Class({init=function(self, timeout)
      self.timeout = timeout
      self._original_timeout = timeout
end})

function Timer:Tick()
  self.timeout = self.timeout - GameController.deltaTime
  return self.timeout < 0
end

function Timer:Reset()
  self.timeout = self._original_timeout
end

return Timer