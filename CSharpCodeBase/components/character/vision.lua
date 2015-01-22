local Class = require 'utils.hump.class'
local Component = require 'components.component'

local VisionComponent = Class({__includes=Component, init = function (self, entity)
      Component.init(self, entity)
      self.entity.vision = self
      self.radius = self.entity.config:Get("visionRadius")
      self._tags = {}
end})

function VisionComponent:IsTargetVisible(targetEntity)
  return targetEntity:GetEffectiveDistance(self.entity) < self.radius
end

function VisionComponent:GetVisibleEntities(radius)
  return RaycastUtils.GetEntitiesInRadius(self.entity:GetPosition(), radius or self.radius)
end

function VisionComponent:GetVisibleEntitiesByTypes(types, radius)
  return RaycastUtils.GetEntitiesInRadiusByTypes(self.entity:GetPosition(),
    radius or self.radius,
    types)
end

function VisionComponent:GetVisibleEntitiesByRSTag(tag, radius)
  return self:GetVisibleEntitiesByTypes(self.entity.relationship:GetTagTypes(tag), radius)
end

return VisionComponent