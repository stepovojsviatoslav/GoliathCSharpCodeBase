local Class = require 'utils.hump.class'

local Stack = Class({init = function(self, maxsize)
  self.maxsize = maxsize or 100
  self.top = 0
  self.elems = {}
end})

function Stack:Push(elem)
  if self.top == self.maxsize then
    return false
  else
    table.insert(self.elems, elem)
    self.top = self.top + 1
    return true
  end
end

function Stack:Pop()
  self.top = self.top - 1
  return table.remove(self.elems)
end

function Stack:Top()
  return self.elems[self.top]
end

function Stack:Size()
  return self.top
end

function Stack:Dump()  
  result = "Stack dump: "
  for k, v in pairs(self.elems) do
    result = result .. v
    result = result .. " "
  end
  Console:Message(result)
end

function Stack:FollowForTop(elem)
  if self.top == 0 then
    Console:Message("Undefined behaviour! Stack FollowForTop")
    return false
  else
    table.insert(self.elems, self.top, elem)
    self.top = self.top + 1
    return true
  end  
end

function Stack:ReplaceNext(elem)
  if self.top > 1 then
    table.insert(self.elems, self.top, elem)
    return table.remove(self.elems, self.top - 1)
  else
    Console:Message ("Undefined behavior! Stack ReplaceNext")
    return nil
  end
end

function Stack:RemoveNext()
  if self.top > 1 then
    self.top = self.top - 1
    return table.remove(self.elems, self.top)
  else
    Console:Message ("Undefined behaviour! Stack RemoveNext")
    return nil
  end
end

return Stack