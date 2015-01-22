local Class = require 'utils.hump.class'

local EventSystem = Class({init = function (self)
      self.handlers = {}
end})

function EventSystem:AddListener(event, listener)
  if self.handlers[event] == nil then
    self.handlers[event] = {}
  end
  local idx = Tables.Find(self.handlers[event], listener)
  if idx < 0 then
    self.handlers[event][#self.handlers[event] + 1] = listener
  end
end

function EventSystem:RemoveListener(event, listener)
  if self.handlers[event] == nil then
    self.handlers[event] = {}
  end
  local idx = Tables.Find(self.handlers[event], listener)
  table.remove(self.handlers[event], listener)
end

function EventSystem:Event(e, params)
  if self.handlers[e] ~= nil then
    local handlers = self.handlers[e]
    for k, listener in pairs(handlers) do
      if listener.OnEvent ~= nil then
        listener:OnEvent(e, params)
      else
        listener(e, params)
      end
    end
  end
end

return EventSystem