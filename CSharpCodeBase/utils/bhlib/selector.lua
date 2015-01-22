local Class = require 'utils.hump.class'

local Selector = Class({init = function(self, behaviours)
      self._behaviours = behaviours
      self.returnCode = B_FAILURE
end})

function Selector:Behave(creature)
  for i = 1, #self._behaviours do
    code = self._behaviours[i]:Behave(creature)
    if code == B_FAILURE then
      -- just continue
    elseif code == B_SUCCESS or code == B_RUNNING then
      self.returnCode = code
      return self.returnCode
    end
  end
  self.returnCode = B_FAILURE
  return self.returnCode
end

return Selector