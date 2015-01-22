local Class = require 'utils.hump.class'

local ActionMachine = Class({init=function (self, emptyAction)
      self.buffer = {}
      self._bufferLimit = 2
      self._previousAction = nil
      self._emptyAction = emptyAction
end})

function ActionMachine:GetCurrentAction()
  return #self.buffer > 0 and self.buffer[#self.buffer] or self._emptyAction
end

function ActionMachine:Flush()
  local currentAction = self:GetCurrentAction()
  for k, v in pairs(self.buffer) do
    v:OnRemove()
    v:Free()
  end
  self.buffer = {}
end

function ActionMachine:FixedUpdate()
  local currentAction = self:GetCurrentAction()
  if currentAction ~= nil and currentAction:IsStarted() then
    currentAction:FixedUpdate()
  end
end

function ActionMachine:Update()
  local currentAction = self:GetCurrentAction()
  if currentAction ~= self._previousAction then
    if self._previousAction ~= nil and self._previousAction:IsStarted() then
      self._previousAction:OnSuspend()
    end
    if currentAction ~= nil then
      if currentAction:IsStarted() then
        currentAction:OnResume()
      else
        currentAction:OnStart()
      end
    end
    self._previousAction = currentAction
  elseif currentAction ~= nil and currentAction == self._previousAction and not currentAction:IsStarted() then
    currentAction:OnStart()
  end
  
  if currentAction ~= nil then
    if currentAction:Update() then
      table.remove(self.buffer, #self.buffer)
      currentAction:OnComplete()
      currentAction:Free()
      self._previousAction = nil
    end
  end
end

function ActionMachine:IsActionExists(name)
  for k, v in pairs(self.buffer) do
    if v.name == name then return true end
  end
  return false
end

function ActionMachine:InsertAction(action)
  local idx = 1
  for k, v in pairs(self.buffer) do
    if v:GetPriority() <= action:GetPriority() then
      idx = idx + 1
    else 
      break
    end
  end
  table.insert(self.buffer, idx, action)  
  return idx
end

function ActionMachine:CutLimit()
  while #self.buffer > self._bufferLimit do
    self.buffer[1]:OnRemove()
    self.buffer[1]:Free()
    table.remove(self.buffer, 1)
  end
end

function ActionMachine:RemoveActionsWithSamePriority()
  local previousPriority = self.buffer[#self.buffer]:GetPriority()
  for i = #self.buffer - 1, 1, -1 do
    local currentPriority = self.buffer[i]:GetPriority()
    if currentPriority == previousPriority then
      self.buffer[i]:OnRemove()
      self.buffer[i]:Free()
      table.remove(self.buffer, i)
    else
      previousPriority = currentPriority
    end
  end
end

function ActionMachine:RemoveUncontinuousActions()
  local needIterate = true
  while needIterate do
    local isRemoved = false
    for k, v in pairs(self.buffer) do
      if k < #self.buffer then
        if not v:IsContinuous() and v:IsStarted() then
          v:OnRemove()
          v:Free()
          table.remove(self.buffer, k)
          isRemoved = true
          break
        end
      end
      needIterate = isRemoved
    end
  end
end

function ActionMachine:PushAction(action)
  local currentAction = self:GetCurrentAction()
  self:InsertAction(action)
  self:CutLimit()
  self:RemoveActionsWithSamePriority()
  self:RemoveUncontinuousActions()
  action:OnPushed()
end

function ActionMachine:IsEmpty()
  return #self.buffer == 0
end
return ActionMachine