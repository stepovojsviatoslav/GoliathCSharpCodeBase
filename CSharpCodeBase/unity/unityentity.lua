local Class = require 'utils.hump.class'
local Vector2 = require 'utils.hump.vector2'
local Vector3 = require 'utils.hump.vector3'
local Entity = require 'unity.entity'

local UnityExistsEntity = Class({__includes=Entity, init = function (self, gameObject)      
      Entity.init(self)      
      self.gameObject = gameObject
      self.name = gameObject.name
      self:InitRadiusAndHeight(gameObject)      
      self.transform = self.gameObject.transform
      self.luaMapper = self.gameObject:GetComponent("LuaMapper")
      self.luaMapper:SetEntity(self)
      
      -- Public api fields
      self.static = true
      self.visible = false
      self.enabled = false
      self.interactable = true
      
      self._cachedPosition = nil
      self._cachedPositionValid = false
      self.visibilityTime = 0
end})

function UnityExistsEntity:InitRadiusAndHeight(gameObject)
  local result = {}
  RigidbodyUtils.GetCapsuleColliderData(gameObject, result)
  self.radius = result.radius or 0.5
  self.height = result.height or 0.5
  self.capsuleOffset = Vector3(result.ox or 0, result.oy or 0, result.oz or 0)
end

function UnityExistsEntity:OnChangeVisibility(state)
  self.visible = state
  self.enabled = state
  self:Message("OnChangeVisibility", state)
  self.visibilityTime = 0
end

function UnityExistsEntity:Update()
  if self.visible then
    self.visibilityTime = self.visibilityTime  + GameController.deltaTime
  end
  if not self.static then self._cachedPositionValid = false end
  Entity.Update(self)
end

function UnityExistsEntity:OnUnload()
  self:Message('RemoveFromTimeService')
  GameController.entityFactory:Destroy(self)
  --GameController:RemoveEntity(self)
  --GameController.pooler:Release(self.gameObject)
end

function UnityExistsEntity:FixedUpdate()
  if not self.static then self._cachedPositionValid = false end
  Entity.FixedUpdate(self)
end

function UnityExistsEntity:LateUpdate()
  if not self.static then self._cachedPositionValid = false end
  Entity.LateUpdate(self)
end

function UnityExistsEntity:OnEvent(data)
  self:Message("OnEvent", data)
end

function UnityExistsEntity:Destroy()
  GameController.entityFactory:Destroy(self)
end

function UnityExistsEntity:GetType()
  return "UnityExistsEntity"
end

function UnityExistsEntity:LookAt(vec3)
  local rot = RotationUtils.LookRotation(Vector3(vec3.x, 0, vec3.z))
  self:SetRotation(rot)
end

function UnityExistsEntity:SetPosition(vec3)
  self._cachedPosition = vec3
  Transform.SetPosition(self.transform, vec3)
end

function UnityExistsEntity:ResetPositionCache()
  self._cachedPositionValid = false
end

function UnityExistsEntity:GetPosition(misscache)
  local nocache = misscache or false
  if not self._cachedPositionValid or nocache then
    local x = self.luaMapper:_X()
    local y = self.luaMapper:_Y()
    local z = self.luaMapper:_Z()
    self._cachedPosition = Vector3(x,y,z)
    self._cachedPositionValid = true
  end
  return self._cachedPosition
end

function UnityExistsEntity:GetForwardVector()
  return Transform.GetForwardVector(self.transform)
end

function UnityExistsEntity:GetPosition2D()
  local pos = self:GetPosition()
  return Vector2(pos.x, pos.z)
end

function UnityExistsEntity:SetRotation(vec3)
  Transform.SetRotation(self.transform, vec3)
end

function UnityExistsEntity:GetRotation()
  local x = self.luaMapper:_RX()
  local y = self.luaMapper:_RY()
  local z = self.luaMapper:_RZ()
  return Vector3(x,y,z)  
end

function UnityExistsEntity:GetEffectiveDistanceToVec(vec3)
  local localPos = self:GetPosition2D()
  local targetPos = Vector2(vec3.x, vec3.z)
  local result = (localPos - targetPos):Length() - self.radius
  Console:Message(result)
  if result < 0 then result = 0 end
  return result
end

function UnityExistsEntity:GetSimpleDistance(targetEntity)
  local localPos = self:GetPosition2D()
  local targetPos = targetEntity:GetPosition2D()
  local result = (localPos - targetPos):Length()
  return result
end

function UnityExistsEntity:GetSimpleDistanceToVec(vec3)
  local localPos = self:GetPosition2D()
  local targetPos = Vector2(vec3.x, vec3.z)
  local result = (localPos - targetPos):Length()
  return result
end

function UnityExistsEntity:GetEffectiveDistance(targetEntity)  
  local localPos = Transform.TransformPoint(self.transform, self.capsuleOffset)
  local targetPos = Transform.TransformPoint(targetEntity.transform, targetEntity.capsuleOffset)
  localPos.y = 0
  targetPos.y = 0
  local result = (localPos - targetPos):Length() - self.radius - targetEntity.radius
  if result < 0 then result = 0 end
  return result 
end

function UnityExistsEntity:SetScale(vec3)
  Transform.SetScale(self.transform, vec3)
end

function UnityExistsEntity:ChangeRadiusAndHeight(gameObject)
   self.radius = RigidbodyUtils.TryGetRadius(gameObject) or 0.5
   self.height = RigidbodyUtils.TryGetHeight(gameObject) or 0.5
end 

return UnityExistsEntity