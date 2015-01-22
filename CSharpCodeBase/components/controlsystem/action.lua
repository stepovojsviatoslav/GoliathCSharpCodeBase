local Class = require 'utils.hump.class' 

local Action = Class({init=function(self, priority, continuous, name)
      self.priority = priority
      self.continuous = continuous and true or false
      self.isStarted = false
      self.name = name
end})

function Action:OnFetch(...)
  self:init(...)
end

function Action:Free()
  if self.__pool ~= nil then
    self.__pool:Release(self)
  end
end

function Action:IsStarted()
  return self.isStarted
end

function Action:OnStart()
  self.isStarted = true
  self:OnStartRunning()  
end

function Action:OnSuspend()
  self:OnStopRunning()
end

function Action:OnResume()
  self:OnStartRunning()
end

function Action:OnComplete()
  self:OnStopRunning()
end

function Action:FixedUpdate()
  
end

function Action:OnStopRunning()
end

function Action:OnStartRunning()
end

function Action:OnPushed()
end

function Action:OnRemove()
  self:OnStopRunning()
end

function Action:IsContinuous()
  return self.continuous
end

function Action:GetPriority()
  return self.priority 
end

function Action:OnEvent(data)
end

return Action