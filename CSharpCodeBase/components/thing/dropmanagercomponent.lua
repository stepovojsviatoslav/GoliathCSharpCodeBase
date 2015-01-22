local Class = require 'utils.hump.class'
local Component = require 'components.component'
local ConfigComponent = require 'components.services.config'

local DropManagerComponent = Class({__includes=Component, init=function(self, entity)
      Component.init(self, entity)
      self.config = ConfigComponent('drop', self.entity.name)
      self.entity.dropManager = self
      
      self.healthCheckPoints = {}
      self.healthStep = {}
      self:LoadDrop(self.entity.config:Get('defaultDrop') or 'none')
end})

function DropManagerComponent:LoadDrop(entityName)
  if entityName == 'none' or entityName == nil then
    self.isActive = false
    return 
  else
    self.isActive = true
  end
  self.dropItems = self.config:Get("drop_item_" .. entityName)
  self.dropEvents = self.config:Get("drop_event_" .. entityName)
  self.dropPerlifes = self.config:Get("drop_perlife_" .. entityName)
  self.dropChance = self.config:Get("drop_chance_" .. entityName)
  self.healthCheckPoints = {}
  self.healthStep = {}
  
  for i=1, #self.dropEvents, 1 do
    table.insert(self.healthStep,(self.entity.health.maxAmount/100)*self.dropPerlifes[i])
    table.insert(self.healthCheckPoints, self.entity.health.maxAmount - (self.entity.health.maxAmount/100)*self.dropPerlifes[i])
  end
end

function DropManagerComponent:DropAfterHit()
  if self.isActive == false then return end
  for i=1, #self.dropEvents, 1 do
    if self.dropEvents[i] == "hit" then
      local count = 0;
      count = self:Drop(i, count)
      if count ~=0 then
        -- вставить выпадание дропа
      end
    end
  end
end

function DropManagerComponent:DropAfterDeath()
  if self.isActive == false then return end
  self:DropAfterHit()
  for i=1, #self.dropEvents, 1 do
    if self.dropEvents[i] == "death" then
      if self:IsDrop(i) then
        -- вставить выпадание дропа
      end
    end
  end
end

function DropManagerComponent:Drop(i, dropCount)
  if self.healthCheckPoints[i] >= self.entity.health.amount then
    self.healthCheckPoints[i] = self.healthCheckPoints[i] - self.healthStep[i]
    if self:IsDrop(i) then
      dropCount = dropCount + 1
    end
    dropCount = self:Drop(i, dropCount)
  end
  return dropCount
end

function DropManagerComponent:IsDrop(i)
  if math.chance(self.dropChance[i]) then
    return true
  end
  return false
end

function DropManagerComponent:OnRegeneration()
  if self.isActive == false then return end
  for i=1, #self.healthCheckPoints, 1 do
    if self.healthCheckPoints[i] <= self.entity.health.amount then
      self.healthCheckPoints[i] = self.healthCheckPoints[i] + self.healthStep[i]
    end
  end
end

return DropManagerComponent