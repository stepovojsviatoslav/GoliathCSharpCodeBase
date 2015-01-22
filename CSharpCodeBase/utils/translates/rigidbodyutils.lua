local Vector3 = require 'utils.hump.vector3'

RigidbodyUtils = {}

local _CreateCharacterRigidbody = luanet.RigidbodyUtils.CreateCharacterRigidbody
function RigidbodyUtils.CreateCharacterRigidbody(t)
  _CreateCharacterRigidbody(t)
end

local _Move = luanet.RigidbodyUtils.Move
function RigidbodyUtils.Move(r, vec3)
  _Move(r, vec3.x, vec3.y, vec3.z)
end

local _MoveNotRotate = luanet.RigidbodyUtils.MoveNotRotate
function RigidbodyUtils.MoveNotRotate(r, vec3)
  _MoveNotRotate(r, vec3.x, vec3.y, vec3.z)
end

local _MoveLookAtMouse = luanet.RigidbodyUtils.MoveLookAtMouse
function RigidbodyUtils.MoveLookAtMouse(r, vec3, h)
  _MoveLookAtMouse(r, vec3.x, vec3.y, vec3.z, h)
end

local _AvoidMove = luanet.RigidbodyUtils.AvoidMove
function RigidbodyUtils.AvoidMove(go, vec31, vec32, sphereRadius, forwardDistance, sphericalDistance, offsetAngle)
  local result = {}
  _AvoidMove(go, nil, result, sphericalDistance, sphereRadius,
     offsetAngle, forwardDistance,
     vec31.x, vec31.y, vec31.z,
     vec32.x, vec32.y, vec32.z)
  return result
end

local _ResetVelocity = luanet.RigidbodyUtils.ResetVelocity
function RigidbodyUtils.ResetVelocity(r)
  _ResetVelocity(r)
end

local _MoveDown = luanet.RigidbodyUtils.MoveDown
function RigidbodyUtils.MoveDown(g, e)
  _MoveDown(g, e)
end

local _SetRotation = luanet.RigidbodyUtils.SetRotation
function RigidbodyUtils.SetRotation(r, vec3)
  _SetRotation(r, vec3.x, vec3.y, vec3.z)
end

local _LookAt = luanet.RigidbodyUtils.LookAt
function RigidbodyUtils.LookAt(r, vec3)
  _LookAt(r, vec3.x, vec3.y, vec3.z)
end

local _ApplyImpulse = luanet.RigidbodyUtils.ApplyImpulse
function RigidbodyUtils.ApplyImpulse(r, vec3)
  _ApplyImpulse(r, vec3.x, vec3.y, vec3.z)
end

local _IgnoreCollision = luanet.RigidbodyUtils.IgnoreCollision
function RigidbodyUtils.IgnoreCollision(t1, t2)
  _IgnoreCollision(t1, t2)
end

local _RotateGrenadeTrajectory = luanet.RigidbodyUtils.RotateGrenadeTrajectory
function RigidbodyUtils.RotateGrenadeTrajectory(x, y, vec1, vec2)
  local result = {}
  _RotateGrenadeTrajectory(result, x, y, vec1.x, vec1.y, vec1.z, vec2.x, vec2.y, vec2.z)
  return Vector3(result['x'], result['y'], result['z'])
end

RigidbodyUtils.TryGetRadius = luanet.RigidbodyUtils.TryGetRadius
RigidbodyUtils.TryGetHeight = luanet.RigidbodyUtils.TryGetHeight
RigidbodyUtils.GetCapsuleColliderData = luanet.RigidbodyUtils.GetCapsuleColliderData
RigidbodyUtils.Rotate = luanet.RigidbodyUtils.Rotate
RigidbodyUtils.MoveTransform = luanet.RigidbodyUtils.MoveTransform
RigidbodyUtils.MoveTransformInRadius = luanet.RigidbodyUtils.MoveTransformInRadius
RigidbodyUtils.SetPointPositionToLineRenderer = luanet.RigidbodyUtils.SetPointPositionToLineRenderer
