local Class = require 'utils.hump.class'

local ObjectMap = Class({init=function(self)
      self._objects = {}
      self._map = {}
end})

function ObjectMap:AddObject(obj)
  self._objects[#self._objects + 1] = obj
  self:AddToIndex(obj)
end

function ObjectMap:RemoveObject(obj)
  local idx = Tables.Find(self._objects, obj)
  if idx > -1 then
    table.remove(self._objects, idx)
  end
  self:RemoveFromIndex(obj)
end

function ObjectMap:GetKey(obj, misscache)
  local pos = obj:GetPosition(misscache)
  local cx = math.floor(pos.x / 20)
  local cy = math.floor(pos.z / 20)
  return cx + cy*100000
end

function ObjectMap:AddToIndex(obj, misscache)
  local key = self:GetKey(obj, misscache)
  if not self._map[key] then
    self._map[key] = {}
  end
  local tbl = self._map[key] 
  tbl[#tbl + 1] = obj
  obj._index_last_key = key
end

function ObjectMap:RemoveFromIndex(obj)
  local tbl = self._map[obj._index_last_key]
  local idx = Tables.Find(tbl, obj)
  if idx > -1 then
    table.remove(tbl, idx)
  end
end

function ObjectMap:CheckIndex(obj)
  local key = self:GetKey(obj)
  if key ~= obj._index_last_key then
    -- migration
    self:RemoveFromIndex(obj)
    self:AddToIndex(obj)
  end
end

function ObjectMap:Reindex()
  for k,v in pairs(self._objects) do
    if v.enabled and not v.static then
      self:CheckIndex(v)
    end
  end
end

function ObjectMap:GetNearEntities(vec3, radius)
  local cx = math.floor(vec3.x / 20)
  local cy = math.floor(vec3.z / 20)
  local entities = {}
  for x = cx - 1, cx + 1 do
    for y = cy - 1, cy + 1 do
      local key = x + y*100000
      local tbl = self._map[key] or {}
      -- iterate over subtable, to find near entities
      for k, v in pairs(tbl) do
        if v:GetSimpleDistanceToVec(vec3) < radius then
          entities[#entities + 1] = v
        end
      end
    end
  end
  return entities
end

return ObjectMap