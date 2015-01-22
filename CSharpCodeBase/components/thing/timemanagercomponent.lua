local Class = require 'utils.hump.class'
local Component = require 'components.component'

local TimerResult = Class({init=function(self, name, timeLeft)
      self.name = name
      self.complete = false
      self.timeLeft = timeLeft      
end})

function TimerResult:Complete()
  self.complete = true
end

local TimeManagerComponent = Class({__includes=Component, init=function(self, entity)
      Component.init(self, entity)
      self.entity.timeManager = self
      self.actions = {}
      self.timeAppend = 1
      GameController.timeService:Add(self)
end})

function TimeManagerComponent:StableUpdate()  
  local completed = {}
  for i=1, #self.actions, 1 do
    if self.actions[i]['timerType'] == 'default' then
      if GameController.gameTime - self.actions[i]['currentTime'] > self.actions[i]['timeout'] then
        local tResult = TimerResult(self.actions[i]['name'], self.actions[i]['timeout'] - (GameController.gameTime - self.actions[i]['currentTime']))
        if self.entity[self.actions[i]['method']] then          
          self.entity[self.actions[i]['method']](self.entity, tResult)
        else              
          self.entity:Message(self.actions[i]['method'], tResult)
        end
        if tResult.complete then
          table.insert(completed, i)
        else
          self.actions[i]['currentTime'] = self.actions[i]['currentTime'] + self.timeAppend
        end
      end
    else
      if GameController.gameTime - self.actions[i]['currentTime'] > self.timeAppend then
        self.actions[i]['currentTime'] = self.actions[i]['currentTime'] + self.timeAppend
        self.actions[i]['timeout'] = self.actions[i]['timeout'] - self.timeAppend
        local tResult = TimerResult(self.actions[i]['name'], self.actions[i]['timeout'])
        if self.entity[self.actions[i]['method']] then          
          self.entity[self.actions[i]['method']](self.entity, tResult)
        else              
          self.entity:Message(self.actions[i]['method'], tResult)
        end
        if tResult.complete then
          table.insert(completed, i)
        end
      end
    end
  end  
  for _, v in pairs(completed) do
    table.remove(self.actions, v)
  end
end

function TimeManagerComponent:Add(method, timeout, name, timerType)
  local tbl = {}
  tbl['method'] = method
  tbl['timeout'] = timeout
  tbl['currentTime'] = GameController.gameTime
  tbl['name'] = name or ''
  tbl['timerType'] = timerType or 'default'
  table.insert(self.actions, tbl)  
end

function TimeManagerComponent:Clear()
   self.actions = {}
end

function TimeManagerComponent:RemoveFromTimeService()
  GameController.timeService:Remove(self)
end

function TimeManagerComponent:Load(storage)
  self.actions = {}
  local count = storage:GetInt('actionscout', 0)
  for i=1, count, 1 do
    local tbl = {}
    tbl['method'] = storage:GetString('method' .. tostring(i))
    tbl['timeout'] = storage:GetFloat('timeout' .. tostring(i))
    tbl['currentTime'] = storage:GetFloat('currentTime' .. tostring(i))
    table.insert(self.actions, tbl)
  end
end

function TimeManagerComponent:Save(storage)
  for i=1, #self.actions, 1 do
    storage:SetString('method' .. tostring(i), self.actions[i]['method'])
    storage:SetFloat('timeout' .. tostring(i), self.actions[i]['timeout'])
    storage:SetFloat('currentTime' .. tostring(i), self.actions[i]['currentTime'])
  end
  storage:SetInt('actionscout', #self.actions)
end

return TimeManagerComponent