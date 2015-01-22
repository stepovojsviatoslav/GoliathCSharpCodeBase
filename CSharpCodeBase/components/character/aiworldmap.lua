local Class = require 'utils.hump.class'
local Component = require 'components.component'
local Vector3 = require 'utils.hump.vector3'

local AIWorldMap = Class({__includes=Component, init = function (self, entity)
      Component.init(self, entity)
      self._map = {}
      self.entity.worldmap = self
end})

function AIWorldMap:AddLocation(locName, vec3)
  self._map[locName] = vec3
end

function AIWorldMap:GetLocation(locName)
  return self._map[locName] or self.entity:GetPosition()
end

function AIWorldMap:GetDistanceTo(locName)
  return self.entity:GetSimpleDistanceToVec(self:GetLocation(locName))
end

function AIWorldMap:Save(storage)
  local i = 0
  for k, v in pairs(self._map) do
    storage:SetFloat('aiworldmap_' .. i .. 'x', v.x)
    storage:SetFloat('aiworldmap_' .. i .. 'y', v.y)
    storage:SetFloat('aiworldmap_' .. i .. 'z', v.z)
    storage:SetString('aiworldmap_' .. i .. 'key', k)
    i = i + 1
  end
  storage:SetInt('aiworldmap_count', #self._map)
end

function AIWorldMap:Load(storage)
  local count = storage:GetInt('aiworldmap_count', 0)
  if count == 0 then
    self:AddLocation('home', self.entity:GetPosition(true))
  else
    for i = 0, count do
      local x = storage:GetFloat('aiworldmap_' .. i .. 'x')
      local y = storage:GetFloat('aiworldmap_' .. i .. 'y')
      local z = storage:GetFloat('aiworldmap_' .. i .. 'z')
      local key = storage:GetString('aiworldmap_' .. i .. 'key') 
      self:AddLocation(key, Vector3(x, y, z))
    end
  end
end

return AIWorldMap