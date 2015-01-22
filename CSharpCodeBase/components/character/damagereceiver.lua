local Class = require 'utils.hump.class'
local Component = require 'components.component'
local Vector2 = require 'utils.hump.vector2'
local Vector3 = require 'utils.hump.vector3'

local DamageReceiverComponent = Class({__includes=Component, init=function(self, entity)
      Component.init(self, entity)
      self.entity.damageReceiver = self
      self.overrideSideResist = {front=-1,side=-1,back=-1}
end})

function DamageReceiverComponent:SetOverrideSideResist(side, resist)
  self.overrideSideResist[side] = resist
end

function DamageReceiverComponent:DropOverrideSideResist(side)
  self.overrideSideResist[side] = -1
end

function DamageReceiverComponent:OnDamageReceive(damageData)
  local source = damageData.source
  local damage = damageData.damage
  damage = self:ProcessResist(damage)
  damage = self:ProcessSideResist(damage, source:GetForwardVector())
  local summaryDamage = self:GetSummaryDamage(damage)
  self.entity:Message("OnApplyDamage", {summary=summaryDamage, damage=damage, effects=damageData.effects, source=source})
  --print("Apply damage: " .. summaryDamage)
end

function DamageReceiverComponent:GetSummaryDamage(damage)
  local sum = 0
  for k, v in pairs(damage) do
    sum = sum + v
  end
  return sum
end

function DamageReceiverComponent:ProcessResist(damage)
  local resistDamage = {}
  for k, v in pairs(damage) do
    resistDamage[k] = v * self.entity.resist:GetResist(k)
  end
  return resistDamage
end

function DamageReceiverComponent:ProcessSideResist(damage, sourceForward)
  local sideValue = self:GetSideResistValue(sourceForward)
  local sideResist = self.overrideSideResist[sideValue] > -1 and self.overrideSideResist[sideValue] or self.entity.resist:GetSideResist(sideValue)
  local resistDamage = {}
  for k, v in pairs(damage) do
    resistDamage[k] = v * sideResist
  end
  return resistDamage
end

function DamageReceiverComponent:GetSideResistValue(sourceForward)
  local myForward = self.entity:GetForwardVector()
  local sf2 = Vector2(sourceForward.x, sourceForward.z)
  local mf2 = Vector2(myForward.x, myForward.z)
  local dotProduct = sf2:Dot(mf2)
  local angle = math.acos(dotProduct/(sf2:Length() * mf2:Length()))
  local degrees = angle * (180/math.pi)
  if degrees < 45 and degrees > -45 then
    return 'back'
  elseif degrees > 45 and degrees < 90 + 45 then
    return 'side'
  else
    return 'front'
  end
end

return DamageReceiverComponent