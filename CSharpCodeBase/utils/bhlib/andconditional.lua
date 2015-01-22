local Class = require 'utils.hump.class'

local AndConditional = Class({init = function(self, conditionals)
    self.conditionals = conditionals
    self.returnCode = B_FAILURE
end})

function AndConditional:Behave(creature)
  result = true
  for k, v in pairs(self.conditionals) do
    if v:Behave(creature) == B_FAILURE then
      result = false
      break
    end
  end
  self.returnCode = result and B_SUCCESS or B_FAILURE
  return self.returnCode
end

function AndConditional:Bool(creature)
  result = true
  for k, v in pairs(self.conditionals) do
    if v:Behave(creature) == B_FAILURE then
      result = false
      break
    end
  end
  return result
end

return AndConditional