local Class = require 'utils.hump.class'
local Entity = require 'unity.entity'
local RecipeList = require 'components.inventory.recipelist'
local UIGadgetController = require 'entities.ui.uigadgetcontroller'

local UIWeaponController= Class({__includes=Entity, init=function(self, gameObject)            
      Entity.init(self)   
      self.gameObject = gameObject
      self.weaponController = gameObject:GetComponent("UIWeaponController")
      self.weaponController:SetupLuaController(self)
      self.weaponController:SetCallback(self.Callback)     
      self.inputService = GameController.inputService
      self.activeWeaponSlot = 'weapon1'
end})

function UIWeaponController:GetCount()
  if GameController.inventory and GameController.gadgetController and #GameController.gadgetController.activeGadget > 0 then
    local count = GameController.inventory:GetItemCount(GameController.gadgetController.activeGadget[2] > 0)
    if count > 0 then
      return count
    else
    return ""
    end
  else
    return ""
  end
end

function UIWeaponController:SelectFirstWeapon()
  self.activeWeaponSlot = 'weapon1'
  local item = GameController.inventory.grids.equipment:GetItem(1)
  if item then
    GameController.player.playerController:GetCurrentSlot():Message("OnWeaponSelect", item.name)
  else
    GameController.player.playerController:GetCurrentSlot():Message("OnWeaponSelect", "hero_hand")
  end
end

function UIWeaponController:SelectSecondWeapon()
  self.activeWeaponSlot = 'weapon2'
  local item = GameController.inventory.grids.equipment:GetItem(2)
  if item then
    GameController.player.playerController:GetCurrentSlot():Message("OnWeaponSelect", item.name)
  else
    GameController.player.playerController:GetCurrentSlot():Message("OnWeaponSelect", "hero_hand")
  end
end

function UIWeaponController:SelectedSlotRepeated(weapon)
  if self.activeWeaponSlot == weapon then
    return true
  else
    return false
  end
end

function UIWeaponController:ActivateCurrentGadget()
  
end

return UIWeaponController
