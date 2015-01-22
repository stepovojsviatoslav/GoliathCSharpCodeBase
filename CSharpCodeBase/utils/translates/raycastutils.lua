local Vector3 = require 'utils.hump.vector3'

RaycastUtils = {}

local _GetHeight = luanet.RaycastUtils.GetHeight
function RaycastUtils.GetHeight(vec3)
  return _GetHeight(vec3.x, vec3.y, vec3.z)
end

local _GetEntitiesInRadius = luanet.RaycastUtils.GetEntitiesInRadius
function RaycastUtils.GetEntitiesInRadius(vec3, radius)
  --local entities = {}
  local entities = GameController.objectMap:GetNearEntities(vec3, radius)
  --_GetEntitiesInRadius(vec3.x, vec3.y, vec3.z, radius, entities)
  return entities
end

local _GetEntitiesInRadiusByTypes = luanet.RaycastUtils.GetEntitiesInRadiusByTypes
function RaycastUtils.GetEntitiesInRadiusByTypes(vec3, radius, types)
  --local entities = {}
  local entities = GameController.objectMap:GetNearEntities(vec3, radius)
  local result = {}
  for k, v in pairs(entities) do
    if Tables.Find(types, v.name) > -1 then
      result[#result + 1] = v
    end
  end
  --_GetEntitiesInRadiusByTypes(vec3.x, vec3.y, vec3.z, radius, types, entities)
  --return entities
  return result
end

local _GetDamaged = luanet.RaycastUtils.GetDamaged
function RaycastUtils.GetDamaged(vec3, radius, target)
  local entities = {}
  _GetDamaged(vec3.x, vec3.y, vec3.z, radius, target.x, target.z, entities)
  return entities
end

function RaycastUtils.GetSector(vec3, radius, forwardVector, angle)
  local circle
end