local Class = require 'utils.hump.class'
local Entity = require 'unity.entity'
local RecipeList = require 'components.inventory.recipelist'

local UIGadgetController= Class({__includes=Entity, init=function(self)            
      Entity.init(self)   
      self.gameObject = luanet.UnityEngine.GameObject.Find("UIGadgetsPanel")
      self.gadgetController = self.gameObject:GetComponent("UIGadgetController")
      self.gadgetController:SetupLuaController(self)
      self.gadgetController:SetCallback(self.Callback)     
      self.inputService = GameController.inputService
      self.enabled = true
      self.topGadget = {}
      self.botGadget = {}
      self.leftGadget = {}
      self.rightGadget = {}
      self.activeGadget = {}
      self.activePanel = -1
      self.gadgetController:Hide()
      self.gadgetController:SetText('')
end})

function UIGadgetController:OnGadgetButtonClick()
  if #self.activeGadget > 0 and GameController.inventory:GetItemCount(self.activeGadget[1]) > 0 then
    local characterEntity = GameController.player.playerController:GetCurrentSlot()
    local path = GameController.database:Get('items', self.activeGadget[1] ..'/require')
    local Gadget = require(path)
    local gadget = Gadget(characterEntity, self.activeGadget[1])
    gadget:Apply()
  else
    self.activeGadget = {}
    self.activePanel = -1
  end
end

function UIGadgetController:OnInventaryChanged()
  if #self.activeGadget > 0 and GameController.inventory:GetItemCount(self.activeGadget[1]) > 0 then
    self.gadgetController:SetText(GameController.inventory:GetItemCount(self.activeGadget[1]))
  else
    self.gadgetController:SetText('')
  end
end

function UIGadgetController:Update()
  Entity.Update(self)
  if self.inputService:RightButtonWasPressed() then
    self.gadgetController:Hide()
  end
  
  if self.inputService:TopButtonWasPressed() then
    self:OnGadgetButtonClick()
  end
  
  if self.inputService:DPadRightIsPressed() then
    self:DPadClick(2)
  elseif self.inputService:DPadLeftIsPressed() then
    self:DPadClick(1)
  elseif self.inputService:DPadUpIsPressed() then
    self:DPadClick(0)
  elseif self.inputService:DPadDawnIsPressed() then
    self:DPadClick(3)
  elseif self.inputService:GetKey(KeyCode.H) then
    if not self.gadgetController.active  then
      self.gadgetController:Show()
    end
  elseif self.gadgetController.active then
    self.gadgetController:Hide()
  end
end

function UIGadgetController:DPadClick(count)
   self.activePanel = count
   
   if not self.gadgetController.active  then
      self.gadgetController:Show()
   end
   self.gadgetController:ShowPanel(self.activePanel)
end

function UIGadgetController:GetGadgets()
  local tbl = {}
  for k,v in pairs(GameController.inventory.grids.inventory:GetFilteredItems('gadget')) do 
    local subTbl = {}
    table.insert(subTbl, v.name)
    table.insert(subTbl, v.count)
    table.insert(subTbl, GameController.database:Get('items', v.name ..'/middle_icon'))
    table.insert(tbl, subTbl)
  end
  return tbl
end

function UIGadgetController:OnHide()
  self.buttonClickTime = 0
  self.inputService:PopFrame()
end

function UIGadgetController:OnShow()
  self.inputService:PushFrame('gadgets_menu')
end

function UIGadgetController:SetLeftGadget(name, count)
  self.leftGadget = {}
  table.insert(self.leftGadget, name)
  table.insert(self.leftGadget, count)
  table.insert(self.leftGadget, GameController.database:Get('items', name ..'/middle_icon'))
end

function UIGadgetController:SetRightGadget(name, count)
  self.rightGadget = {}
  table.insert(self.rightGadget, name)
  table.insert(self.rightGadget, count)
  table.insert(self.rightGadget, GameController.database:Get('items', name ..'/middle_icon'))
end

function UIGadgetController:SetTopGadget(name, count)
  self.topGadget = {}
  table.insert(self.topGadget, name)
  table.insert(self.topGadget, count)
  table.insert(self.topGadget, GameController.database:Get('items', name ..'/middle_icon'))
end

function UIGadgetController:SetBotGadget(name, count)
  self.botGadget = {}
  table.insert(self.botGadget, name)
  table.insert(self.botGadget, count)
  table.insert(self.botGadget, GameController.database:Get('items', name ..'/middle_icon'))
end

function UIGadgetController:SetActiveGadget(name, count)
  self.activeGadget = {}
  table.insert(self.activeGadget, name)
  table.insert(self.activeGadget, count)
  table.insert(self.activeGadget, GameController.database:Get('items', name ..'/middle_icon'))
  self.gadgetController:SetText(count)
end

function UIGadgetController:GetRightGadget()
  return self.rightGadget
end

function UIGadgetController:GetLeftGadget()
  return self.leftGadget
end

function UIGadgetController:GetTopGadget()
  return self.topGadget
end

function UIGadgetController:GetBotGadget()
  return self.botGadget
end

function UIGadgetController:GetActivePanel()
  return self.activePanel
end


return UIGadgetController
