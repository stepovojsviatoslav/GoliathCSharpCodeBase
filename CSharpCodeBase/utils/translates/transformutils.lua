local Vector3 = require 'utils.hump.vector3'

Transform = {}

local _GetPosition = luanet.TransformUtils.GetPosition
function Transform.GetPosition(transform)
  local result = {}
  _GetPosition(transform, result)
  return Vector3(result.x, result.y, result.z)
end

local _SetPosition = luanet.TransformUtils.SetPosition
function Transform.SetPosition(transform, vec3)
  _SetPosition(transform, vec3.x, vec3.y, vec3.z)
end

local _SetScale = luanet.TransformUtils.SetScale
function Transform.SetScale(transform, vec3)
  _SetScale(transform, vec3.x, vec3.y, vec3.z)
end

local _GetRotation = luanet.TransformUtils.GetRotation
function Transform.GetRotation(transform)
  local result = {}
  _GetRotation(transform, result)
  return Vector3(result.x, result.y, result.z)
end

local _SetRotation = luanet.TransformUtils.SetRotation
function Transform.SetRotation(transform, vec3)
  _SetRotation(transform, vec3.x, vec3.y, vec3.z)
end

local _GetForwardVector = luanet.TransformUtils.GetForwardVector
function Transform.GetForwardVector(transform)
  local result = {}
  _GetForwardVector(transform, result)
  return Vector3(result.x, result.y, result.z)
end

local _TransformPoint = luanet.TransformUtils.TransformPoint
function Transform.TransformPoint(transform, vec3)
  local result = {}
  _TransformPoint(transform, vec3.x, vec3.y, vec3.z, result)
  local vec = Vector3(result.x, result.y, result.z)
  return vec
end

Transform.GetMapper = luanet.TransformUtils.GetMapper
Transform.AddComponent = luanet.TransformUtils.AddComponent
Transform.SetRendererState = luanet.TransformUtils.SetRendererState
Transform.SetColliderState = luanet.TransformUtils.SetColliderState
Transform.SetObjectState = luanet.TransformUtils.SetObjectState
Transform.SetMaterialColor = luanet.TransformUtils.SetMaterialColor
Transform.Lerp = luanet.TransformUtils.Lerp
Transform.GetRigidbody = luanet.TransformUtils.GetRigidbody