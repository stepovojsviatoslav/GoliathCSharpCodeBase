local Class = require 'utils.hump.class'

local Entity = Class({init = function (self)
      self.components = {}
      self._awakened = false
end})

function Entity:AddComponent(componentType)
  self.components[componentType] = componentType(self)
  if self._awakened then self.components[componentType]:Awake() end
  return self.components[componentType]
end

function Entity:GetComponent(componentType)
  return self.components[componentType]
end

function Entity:Awake()
  for k, v in pairs(self.components) do v:Awake() end
  self._awakened = true
end

function Entity:Update() 
  for k, v in pairs(self.components) do v:Update() end
end

function Entity:FixedUpdate()
  for k, v in pairs(self.components) do v:FixedUpdate() end
end

function Entity:LateUpdate() 
  for k, v in pairs(self.components) do v:LateUpdate() end
end

function Entity:GetType()
  return "Entity"
end

function Entity:Message(funcname, data)
  for k, v in pairs(self.components) do
    func = v[funcname]
    if func ~= nil then
      func(v, data)
    end
  end
end

function Entity:Enable()
  self.enabled = true
end

function Entity:Disable()
  self.enabled = false
end

function Entity:Callback(method, ...)  
  if self[method] ~= nil then      
    local result = self[method](self, ...)
    return result
  else
    return {}  
  end
end

return Entity