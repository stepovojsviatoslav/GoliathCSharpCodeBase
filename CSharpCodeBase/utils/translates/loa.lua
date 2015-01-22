local Vector3 = require 'utils.hump.vector3'

LOA = {}

_ProcessInput = luanet.LOA.ProcessInput
function LOA.ProcessInput(position, vec3, radius)
  corrected = Vector3()
  _ProcessInput(position.x, position.y, position.z, vec3.x, vec3.y, vec3.z, radius, radius * 2, corrected)
  corrected:Normalize()
  return corrected
end