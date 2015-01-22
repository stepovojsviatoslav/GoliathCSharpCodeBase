local Class = require 'utils.hump.class'

local StatefulSequence = Class({init = function(self, behaviours)
      self._behaviours = behaviours
      self.returnCode = B_FAILURE
      self.lastBehaviour = 1
end})

function StatefulSequence:Behave(creature)
  while self.lastBehaviour <= #self._behaviours do
    code = self._behaviours[self.lastBehaviour]:Behave(creature)
    if code == B_FAILURE then
      self.lastBehaviour = 1
      self.returnCode = code
      return self.returnCode
    elseif code == B_SUCCESS then
      -- continue
    elseif code == B_RUNNING then
      self.returnCode = code
      return self.returnCode
    end
    self.lastBehaviour = self.lastBehaviour + 1
  end
  
  self.lastBehaviour = 1
  self.returnCode = B_SUCCESS
  return self.returnCode
end

return StatefulSequence