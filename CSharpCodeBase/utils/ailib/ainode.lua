local Class = require 'utils.hump.class'
local AITree = require 'utils.ailib.aitree'

local AINode = Class({init = function (self, entity, childNodes)
      self.childNodes = childNodes
      self.entity = entity
      self.status = NODE_READY
      self.lastStatus = NODE_READY
      self.parent = nil
      if self.childNodes ~= nil then
        for k, v in pairs(self.childNodes) do
          v.parent = self
        end
      end
      self._lockstart = 0
      self._locktime = 0
end})

function AINode:Visit()
  self.status = NODE_FAILURE
end

function AINode:Save()
  self.lastStatus = self.status
  if self.childNodes ~= nil then
    for k, v in pairs(self.childNodes) do
      v:Save()
    end
  end
end

function AINode:Process()
  if self.status ~= NODE_RUNNING then
    self:Reset()
  elseif self.childNodes ~= nil then
    for k, v in pairs(self.childNodes) do
      v:Process()
    end
  end
end

function AINode:Reset()
  if self.status ~= NODE_READY then
    self.status = NODE_READY
    if self.childNodes ~= nil then
      for k, v in pairs(self.childNodes) do
        v:Reset()
      end
    end
  end
end

function AINode:Sleep(seconds)
  self._lockstart = os.clock()
  self._locktime = seconds
end

function AINode:IsSleeping()
  return os.clock() - self._lockstart < self._locktime
end

function AINode:OnEvent(data)
  if self.childNodes ~= nil then
    for k, v in pairs(self.childNodes) do
      if v.status == NODE_RUNNING then
        v:OnEvent(data)
      end
    end
  end  
end

return AINode