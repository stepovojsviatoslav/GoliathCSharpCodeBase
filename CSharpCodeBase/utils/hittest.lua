local Vector2 = require 'utils.hump.vector2'
local Vector3 = require 'utils.hump.vector3'
local Class = require 'utils.hump.class'

local HitTest = {}

HitTest.IsPointHitted = function(position, forward, targetPosition, sectorAngle)
  local targetRelative = targetPosition - position
  local dotProduct = forward:Dot(targetRelative)
  local angle = math.acos(dotProduct/targetRelative:Length() * forward:Length())
  local degrees = angle * (180/math.pi)
  return degrees <= sectorAngle and degrees >= -sectorAngle
end

HitTest.CheckHitEntity = function (sourceEntity, sourceForward, targetEntity, sectorAngle, sectorDistance, countSteps)
  -- check distance
  sectorDistance = sectorDistance or -1
  if sectorDistance > -1 and sectorDistance < sourceEntity:GetEffectiveDistance(targetEntity) then
    return false
  end
  local sourcePosition = sourceEntity:GetPosition()
  local targetPosition = targetEntity:GetPosition()
  local targetRelative = targetPosition - sourcePosition
  --local sourceForward = Transform.GetForwardVector(sourceEntity.transform)
  local checkPoints = {targetPosition}
  local count = countSteps or 10
  local step = 360 / count
  for i = 1, 360, step do
    local vec3 = Vector3(targetEntity.radius, 0, 0)
    vec3:RotateAroundY(i)
    checkPoints[#checkPoints + 1] = vec3 + targetPosition
  end
  for k, v in pairs(checkPoints) do
    if HitTest.IsPointHitted(sourcePosition, sourceForward, v, sectorAngle) then
      return true
    end
  end
  return false
end

return HitTest