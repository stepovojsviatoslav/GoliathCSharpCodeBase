local Vector3 = require 'utils.hump.vector3'

RotationUtils = {}

local _LookRotation = luanet.RotationUtils.LookRotation
function RotationUtils.LookRotation(vec3)
  local result = {}
  _LookRotation(vec3.x, vec3.y, vec3.z, result)
  return Vector3(result.x, result.y, result.z)
end