local Class = require 'utils.hump.class'
local Component = require 'components.component'

local PercentLifeRegenerationString = 'PercentLifeRegeneration'
local TimeUpdateLifeRegenerationString = 'TimeUpdateLifeRegeneration'
local LifeRegenerationNextStateString = 'LifeRegenerationNextState'

local ThingRegenerationComponent = Class({__includes=Component, init=function(self, entity)
  Component.init(self, entity)
  self.entity.regeneration = self
      
  self.percentRegeneration = self.entity.config:Get('percentRegeneration') or 10
  self.timeStep = self.entity.config:Get('timeStepRegeneration') or 25
  self:SetPhases(self.entity.config:Get('regenerationPhases') or 'none')
      
  self.nextRegenerationTimeCheckPoint = self.timeStep + GameController.gameTime
  self.healthRegenerationValue = (self.entity.health.maxAmount/100)*self.percentRegeneration      
  self.isStarted = false
  
  self:Start()
end})

function ThingRegenerationComponent:SetPhases(value)
   self.regenerationTime = StringUtils.split(value, ',')
end

function ThingRegenerationComponent:Start()
  self.nextRegenerationTimeCheckPoint = self.timeStep + GameController.gameTime
  self.isStarted = true
end

function ThingRegenerationComponent:Stop()
  self.isStarted = false
end

function ThingRegenerationComponent:Update()
  if self.isStarted then
    self:PassiveRegeneration()
  end
end

function ThingRegenerationComponent:PassiveRegeneration()
  if GameController.gameTime >= self.nextRegenerationTimeCheckPoint then
   for key, value in pairs(self.regenerationTime) do
      if GameController.daytime.currentPhase == GameController.daytime[value] or value == 'none' then 
        self.nextRegenerationTimeCheckPoint = self.timeStep + GameController.gameTime
        self.entity:OnRegenerate(self.healthRegenerationValue)
        return
      end
    end
  end
end

return ThingRegenerationComponent