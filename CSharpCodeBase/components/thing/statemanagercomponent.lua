local Class = require 'utils.hump.class'
local Component = require 'components.component'

local StateManagerComponent = Class({__includes=Component, init=function(self, entity)
      Component.init(self, entity)
      self.entity.stateManager = self
      self.stateName = self.entity.config:Get('state_name')
      self.currentState = #self.stateName
      
      self.stateTransform = {}
      self.stateAction = {}
      self.stateEnabled = {}
      self.stateTake = {}
      self.stateSwitchPhase = {}
      
      for key, value in pairs(self.stateName) do
        table.insert(self.stateTransform, self.entity.config:Get(value .. '_transform'))
        table.insert(self.stateAction, self.entity.config:Get(value .. '_action') or 'examine')
        table.insert(self.stateEnabled, self.entity.config:Get(value .. '_enabled') or true)
        table.insert(self.stateTake, self.entity.config:Get(value .. '_take') or 'none')
        table.insert(self.stateSwitchPhase, self.entity.config:Get(value .. '_switch_phase') or 'none')
      end
      
      self.gameObject = {}
      self:SetPossibleActions(self.currentState)
      self:InitiateGameObjects()
      self:SetPhases(self.stateSwitchPhase[self.currentState])
      self.entity:ChangeRadiusAndHeight(self.gameObject[self.currentState][1])
      self.timer = 0
end})

function StateManagerComponent:InitiateGameObjects() 
  for i, word in pairs(self.stateTransform) do
    local go = {}
    local mTable = StringUtils.split(word, ',')
    for key, value in pairs(mTable) do
      table.insert(go, self.entity.gameObject.transform:Find(value).gameObject)
    end
    table.insert(self.gameObject, go)
  end
end

function StateManagerComponent:SetPhases(value)
   self.restoreTime = StringUtils.split(value, ',')
end


function StateManagerComponent:OnStateEvent(event)
  local actionsString = self.entity.config:Get(self.stateName[self.currentState] .. '_event_' .. event)
  local tempTable = StringUtils.split(actionsString, ',')
  local actions = {}
  for i=1, #tempTable, 1 do
    for key, value in string.gmatch(tempTable[i], '([_%a][_%w]*):([_%w]+)') do
        actions[key] = value
    end
  end
  
  if actions['action']  then
    if actions['action'] == 'take' then
      self:TakeItem()
    elseif actions['timeout'] then
      self.entity.timeManager:Add(actions['action'], tonumber(actions['timeout']))
    end
  end
  
  if actions['animation'] then
    self:PlayAnimation(actions['animation'])
  end
  
  if actions['reset'] then
    if actions['reset']=='health' then
      self.entity.health:SetNewHealth(self.entity.config:Get(self.stateName[self.currentState] .. '_health') or self.entity.config:Get('healthMaxAmount') or 100)
    end
  end
  
  if actions['transite'] then
    if actions['transite'] == 'death' then
      self.entity:Destroy()
    else
      self:SetStateByName(actions['transite'])
    end
  end
  
  if actions['changeDrop'] and self.entity.dropManager then
    self.entity.dropManager:LoadDrop(actions['changeDrop'])
  end
end

function StateManagerComponent:EntityHealthChanged(state)
  if state == self.entity.health.STATE_HIGHT then
    self.entity:Message('OnStateEvent', 'OnHealthHigh')
  elseif state == self.entity.health.STATE_NORMAL then
    self.entity:Message('OnStateEvent', 'OnHealthNormal')
  elseif state == self.entity.health.STATE_LOW then
    self.entity:Message('OnStateEvent', 'OnHealthLow')
  end
end

function StateManagerComponent:TakeItem() 
  GameController.inventory:AddItem(self:GetItemName(), self:GetItemCount())
end

function StateManagerComponent:GetNumberByName(name)
  for key, value in pairs(self.stateName) do
    if value == name then
      return key
    end
  end
end

function StateManagerComponent:PlayAnimation(name)
  if self.entity.mecanim then
    self.entity.mecanim:ForceSetState(name)
  end
end

function StateManagerComponent:SetPossibleActions(index)
  self.entity.possibleActions:SetActions(StringUtils.split(self.stateAction[index], ','))
end


function StateManagerComponent:SetStateByName(name)
  local number = self:GetNumberByName(name)
  self:SetStateByNumber(number)
end

function StateManagerComponent:SetStateByNumber(number)
  self.entity:Message('OnStateEvent', 'OnExit')
  self.currentState = number
  self:ActivateGameObject(self.currentState)
  self.entity.interactable = self.stateEnabled[self.currentState]
  self:SetPossibleActions(self.currentState)
  self:SetPhases(self.stateSwitchPhase[self.currentState])
  self.entity:Message('Clear')
  self.entity:Message('OnStateEvent', 'OnEnter')
end

function StateManagerComponent:DeactivateAllGameObjects()
   for i=1, #self.gameObject, 1 do 
    for j=1, #self.gameObject[i], 1 do 
      self.gameObject[i][j]:SetActive(false)
    end
   end
end

function StateManagerComponent:ActivateGameObject(i)
  self:DeactivateAllGameObjects()
  for j=1, #self.gameObject[i], 1 do 
    self.gameObject[i][j]:SetActive(true)
  end
  self.entity:ChangeRadiusAndHeight(self.gameObject[i][1])
end

function StateManagerComponent:GetItemName()
  local itemName = ''
  for key, value in string.gmatch(self.stateTake[self.currentState], '(%w+):(%w+)') do
      itemName=key
  end
  return itemName
end

function StateManagerComponent:GetItemCount()
  local itemCount = 0
  for key, value in string.gmatch(self.stateTake[self.currentState], '(%w+):(%w+)') do
      itemCount=tonumber(value)
  end
  return itemCount
end

function StateManagerComponent:Grow(result)
  for key, value in pairs(self.restoreTime) do
    if GameController.daytime.currentPhase == GameController.daytime[value] or value == 'none' then 
      self.entity:Message('OnStateEvent', 'OnGrow')
      result:Complete()
    end
  end
end

function StateManagerComponent:Load(storage)
  self.currentState = storage:GetInt("state", self.currentState)
  self:SetStateByNumber(self.currentState)
end

function StateManagerComponent:Save(storage)
  storage:SetInt("state", self.currentState)
end

return StateManagerComponent