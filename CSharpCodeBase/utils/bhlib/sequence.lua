local Class = require 'utils.hump.class'

local Sequence = Class({init = function(self, behaviours)
      self._behaviours = behaviours
      self.returnCode = B_FAILURE
end})

function Sequence:Behave(creature)
  
  anyRunning = false

  for i = 1, #self._behaviours do
    code = self._behaviours[i]:Behave(creature)
    if code == B_FAILURE then
      self.returnCode = code
      return self.returnCode
    elseif code == B_SUCCESS then
      -- continue
    elseif code == B_RUNNING then
      anyRunning = true
      -- continue
    end
  end
  self.returnCode = anyRunning and B_RUNNING or B_SUCCESS
  return self.returnCode
end

return Sequence