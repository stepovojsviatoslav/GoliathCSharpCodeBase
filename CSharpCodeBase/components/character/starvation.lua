local Class = require 'utils.hump.class'
local Component = require 'components.component'
local Config = require 'components.services.config'

local StarvationComponent = Class({__includes=Component, init=function(self, entity)
      Component.init(self, entity)
      self.entity.starvation = self
      self.state = false
      self.timeout = self.entity.config:Get('starvationTimeout') or 2
      self.currentTimeout = self.timeout
end})

function StarvationComponent:SetState(state)
  self.state = state
  self.currentTimeout = self.timeout
end

function StarvationComponent:Update()
  Component.Update(self)
  if self.state == false and self.entity.health:GetState() ~= 'low' then
    local multiplier = self.entity.health:GetState() == 'high' and 1 or 2
    self.currentTimeout = self.currentTimeout - GameController.deltaTime * multiplier
    if self.currentTimeout < 0 then
      self.currentTimeout = self.timeout
      self.state = true
    end
  end
end

function StarvationComponent:OnRespawn()
  self:Reset()
end

function StarvationComponent:Reset()
  self:SetState(false)
end

function StarvationComponent:GetState()
  return self.state or self.entity.health:GetState() == 'low'
end

return StarvationComponent