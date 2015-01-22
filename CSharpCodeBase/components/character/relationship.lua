local Class = require 'utils.hump.class'
local Component = require 'components.component'
local TimeoutList = require 'utils.hump.timeoutlist'

local RelationshipComponent = Class({__includes=Component, init = function (self, entity)
      Component.init(self, entity)
      self.entity.relationship = self
      self._tagTypes = {}
      self._tagInstances = {}
      self:RegisterTagTypes("enemy", self.entity.config:Get("enemy") or {})
      self:RegisterTagTypes("friend", self.entity.config:Get("friend") or {})
      self:RegisterTagTypes("parent", self.entity.config:Get("parent") or {})
      self:RegisterTagTypes("alertness", self.entity.config:Get("alertness") or {})
      self:RegisterTagTypes("scary", self.entity.config:Get("scary") or {})
      self:RegisterTagTypes("food", self.entity.config:Get("food") or {})
end})

function RelationshipComponent:RegisterTagTypes(tag, types)
  self._tagTypes[tag] = types
end

function RelationshipComponent:GetTagTypes(tag)
  return self._tagTypes[tag]
end

function RelationshipComponent:AddInstance(tag, entity, timeout)
  if self._tagInstances[tag] == nil then
    self._tagInstances[tag] = TimeoutList(timeout or self.entity.config:Get("rsReactiveTimeout"))
  elseif timeout ~= nil then
    self._tagInstances[tag]:SetGlobalTimeout(timeout)
  end
  self._tagInstances[tag]:Add(entity)
end

function RelationshipComponent:GetInstances(tag)
  if self._tagInstances[tag] == nil then
    self._tagInstances[tag] = TimeoutList(self.entity.config:Get("rsReactiveTimeout"))
  end  
  return self._tagInstances[tag]._items
end

function RelationshipComponent:HasInstance(tag, instance)
  if self._tagInstances[tag] == nil then
    return false
  end
  return self._tagInstances[tag]:Exists(instance)
end

function RelationshipComponent:GetInstance(tag)
  if self._tagInstances[tag] == nil then
    self._tagInstances[tag] = TimeoutList(self.entity.config:Get("rsReactiveTimeout"))
  end  
  return self._tagInstances[tag]._items[1]
end

function RelationshipComponent:Update()
  for k, v in pairs(self._tagInstances) do
    v:Update()
  end
end

return RelationshipComponent