local Class = require 'utils.hump.class'
local Component = require 'components.component'
local ActionMachine = require 'components.controlsystem.actionmachine'
local ActionMove = require 'components.controlsystem.actions.actionmove'
local ActionAttackMouse = require 'components.controlsystem.actions.actionattackmouse'
local ActionInteract = require 'components.controlsystem.actions.actioninteract'
local ActionTake = require 'components.controlsystem.actions.actiontake'
local ActionBlock = require 'components.controlsystem.actions.actionblock'

local ActionHit = require 'components.controlsystem.enemyactions.actionhit'
local ActionPush = require 'components.controlsystem.enemyactions.actionpush'
local Grenade = require 'entities.weapon.grenade'

local PlayerController = Class({__includes=Component, init=function(self, entity)
      Component.init(self, entity)
      self.actionMachine = ActionMachine()
      self.luaPool = GameController.luaPooler
      self.luaPool:CreatePool(ActionMove)
      self.luaPool:CreatePool(ActionBlock)
      self.luaPool:CreatePool(ActionInteract)
      self.luaPool:CreatePool(ActionHit)
      self.luaPool:CreatePool(ActionPush)
      self.luaPool:CreatePool(ActionTake)
      self.luaPool:CreatePool(ActionAttackMouse)
      self.spaceCommandTimer = 0
      self.raycastScreenTimer = 0
      self.currentLabelEntity = nil
      self.mouseLabel = luanet.GameFacade.uiMouseUnderLabel
      self.mouseLabel:Hide()
      self.entity.playerController = self
      self.inputService = GameController.inputService
end})

function PlayerController:IsMoveKeyPressed()
  return self.inputService:LeftStickYIsPressed() or self.inputService:LeftStickXIsPressed()
end

function PlayerController:FixedUpdate()
  self.actionMachine:FixedUpdate()
end

function PlayerController:GetInteractEntityUnderMouse()
  local target = Input.RaycastTargetEntity()
  if target ~= nil and target.possibleActions ~= nil and target.interactable then
    if target.possibleActions:GetAction(1) == 'take' or target.possibleActions:GetAction(1) == 'interact' then
      return target
    end
  end  
  return nil
end

function PlayerController:GetNearInteractEntity(radius)
  local entities = RaycastUtils.GetEntitiesInRadius(self.entity:GetPosition(), radius)
  local min = radius * 10
  local minEntity = nil
  
  for k, v in pairs(entities) do    
    if v ~= nil and v.possibleActions ~= nil and v.interactable then
      if v.possibleActions:GetAction(1) == 'take' or v.possibleActions:GetAction(1) == 'interact' then
        local dst = v:GetSimpleDistance(self.entity)
        if dst < min then
          min = dst
          minEntity = v
        end
      end
    end
  end  
  return minEntity
end

function PlayerController:Update()
  self:SearchingThings() 
  self:Interact()
  self:SpecialAction()
  self:Move()
  self:Attack()
  self.actionMachine:Update()
end

function PlayerController:SpecialAction()
  if (self.inputService:RightTriggerWasPressed() and not self.actionMachine:IsActionExists('block')) 
    or (self.inputService:RightTriggerWasPressed() and self.actionMachine:IsEmpty())then
    self.actionMachine:PushAction(self.luaPool:Fetch(ActionBlock, self.entity))
  end
 end
 
 function PlayerController:SearchingThings()
    self.raycastScreenTimer = self.raycastScreenTimer - GameController.deltaTime
    if self.raycastScreenTimer < 0 and not self.actionMachine:IsActionExists('take') then
      self.raycastScreenTimer = 0.2
      local previousLabelEntity = self.currentLabelEntity
      self.currentLabelEntity = self:GetInteractEntityUnderMouse() or self:GetNearInteractEntity(3)
      if self.currentLabelEntity ~= nil then
        local pos = self.currentLabelEntity:GetPosition()
        self.mouseLabel:Show(pos.x, pos.y + self.currentLabelEntity.height, pos.z, "'E' to take")
      elseif previousLabelEntity == nil then
        self.mouseLabel:Hide()
      end
    end
 end
 
 function PlayerController:Interact()
    if self.inputService:BottomButtonWasPressed() and self.currentLabelEntity ~= nil then
      self.mouseLabel:Hide()
      self.actionMachine:PushAction(self.luaPool:Fetch(ActionTake, self.entity, self.currentLabelEntity))
    end
 end
 
 function PlayerController:Move()
  self.spaceCommandTimer = self.spaceCommandTimer - GameController.deltaTime
  if not self.actionMachine:IsActionExists('block') and (self:IsMoveKeyPressed() or self.inputService:GetMouseButton(1)) then
    if not self.actionMachine:IsActionExists('move') then
      if self.inputService:GetMouseButtonDown(1) then
        self.actionMachine:PushAction(self.luaPool:Fetch(ActionMove, self.entity, Input.RaycastMouseOnTerrain()))
      else
        self.actionMachine:PushAction(self.luaPool:Fetch(ActionMove, self.entity))
      end
    end
  end
 end
 
 function PlayerController:Attack()
  local needAttack = self.inputService:LeftButtonWasPressed()
  if not self.inputService:IsGamepad() then
    if Input.IsPointerOverUI() then
      needAttack = false
    end
  end
  needAttack = needAttack and GameController.ui.mainInterface:IsAttackPossible()
  if needAttack then
    if self.entity.grenadeModeVisualizer.active then
      local grenade = Grenade(self.entity.grenadeModeVisualizer.name, self.entity, self.entity.grenadeModeVisualizer:GetSpeed())
    end
    
    local vec3
    if self.entity.gamepadRightStickController and self.entity.gamepadRightStickController:GetTarget() then
      vec3 = self.entity.gamepadRightStickController:GetTarget():GetPosition()
      self.entity.mover:LookAt(self.entity.gamepadRightStickController:GetTarget():GetPosition())
    else
      vec3 = self.entity:GetPosition()
    end
    self.actionMachine:PushAction(self.luaPool:Fetch(ActionAttackMouse, self.entity, vec3))
  end
 end
 
function PlayerController:Hit(damageData)  
  if damageData.summary ~= 0 and self.entity.characterClass <= damageData.source.characterClass then
    self.entity.mecanim:ForceSetState('Empty', 1)
    self.actionMachine:PushAction(self.luaPool:Fetch(ActionHit, self.entity))
  end
end

function PlayerController:Pushed(damageData)
  if self.entity.characterClass <= damageData.source.characterClass then
    self.entity.mecanim:ForceSetState('Empty', 1)
    self.actionMachine:PushAction(ActionPush(self.entity, damageData.source:GetPosition()))
  end
end

function PlayerController:OnDeselectCharacter()
  self.actionMachine:Flush()
end

function PlayerController:OnSelectCharacter()
end

function PlayerController:OnRespawn()
  self.actionMachine:Flush()
end

function PlayerController:OnEvent(data)
  local state = self.actionMachine.buffer[#self.actionMachine.buffer]
  if state ~= nil then
    state:OnEvent(data)
  end
end

function PlayerController:OnSpellCast(spellAction)  
  spellAction.entity = self.entity
  self.actionMachine:PushAction(spellAction)
  return true
end

return PlayerController