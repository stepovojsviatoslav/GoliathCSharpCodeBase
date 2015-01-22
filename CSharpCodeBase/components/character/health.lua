local Class = require 'utils.hump.class'
local Component = require 'components.component'
local Config = require 'components.services.config'

local HealthComponent = Class({__includes=Component, init=function(self, entity)
      Component.init(self, entity)
      self.STATE_HIGH = 'high'
      self.STATE_NORMAL = 'normal'
      self.STATE_LOW = 'low'
      self.entity.health = self
      self.currentHealthState = self.STATE_HIGH
      self.maxAmount = self.entity.config:Get('healthMaxAmount') or 100
      self.amount = self.maxAmount
      self:HealthChanged()
end})

function HealthComponent:Decrease(value)
  self.amount = self.amount - value
  if self.amount < 0 then
    self.amount = 0
  end
  self:HealthChanged()
  return self.amount
end

function HealthComponent:Increase(value)
  self.amount = self.amount + value
  if self.amount > self.maxAmount then
    self.amount = self.maxAmount
  end
  self:HealthChanged()
  return self.amount
end

function HealthComponent:OnRespawn()
  self:Reset()
end

function HealthComponent:Reset()
  self.amount = self.maxAmount
  self:HealthChanged()
end


function HealthComponent:SetNewHealth(health)
   self.maxAmount = health
   self.amount = self.maxAmount
   self:HealthChanged()
end

function HealthComponent:GetState()
  local value = self.amount / self.maxAmount
  if value >= 0.7 then
    return self.STATE_HIGHT
  elseif value >= 0.4 then
    return self.STATE_NORMAL
  else
    return self.STATE_LOW
  end
end

function HealthComponent:GetPercentAmount()
  return self.amount / self.maxAmount
end

function HealthComponent:Load(storage)
  local amount = storage:GetFloat('healthcomponent_amount', -1)
  if amount == -1 then
    self.amount = self.maxAmount
  else
    self.amount = amount
  end
  self:HealthChanged()
end

function HealthComponent:Save(storage)
  storage:SetFloat('healthcomponent_amount', self.amount)
end

function HealthComponent:HealthChanged()
  if self.entity.OnHealthChanged then
    self.entity:OnHealthChanged(self.amount)
  end
  local state = self:GetState()
  if self.currentHealthState ~= state then
    self.currentHealthState = state
    self.entity:Message('EntityHealthChanged', state)
  end
end

return HealthComponent