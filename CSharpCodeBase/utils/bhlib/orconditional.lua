local Class = require 'utils.hump.class'

local OrConditional = Class({init = function(self, conditionals)
    self.conditionals = conditionals
    self.returnCode = B_FAILURE
end})

function OrConditional:Behave(creature)
  result = false
  for k, v in pairs(self.conditionals) do
    if v:Behave(creature) == B_SUCCESS then
      result = true
      break
    end
  end
  self.returnCode = result and B_SUCCESS or B_FAILURE
  return self.returnCode
end

function OrConditional:Bool(creature)
  result = false
  for k, v in pairs(self.conditionals) do
    if v:Behave(creature) == B_SUCCESS then
      result = true
      break
    end
  end
  return result
end

return OrConditional