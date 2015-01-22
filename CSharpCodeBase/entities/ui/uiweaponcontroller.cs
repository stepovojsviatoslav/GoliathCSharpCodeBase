using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class uiweaponcontroller :entity{
 var UIWeaponController= Class({__includes=Entity, init=function(self, gameObject)            
       Entity.init(self)   ;
       this.gameObject = gameObject;
       this.weaponController = gameObject:GetComponent("UIWeaponController");
       this.weaponController:SetupLuaController(self);
       this.weaponController:SetCallback(this.Callback)     ;
       this.inputService = GameController.inputService;
       this.activeWeaponSlot = "weapon1";
 }})
 public void GetCount(){
   if(GameController.inventory  &&  GameController.gadgetController  &&  #GameController.gadgetController.activeGadget > 0  ){ 
     var count = GameController.inventory:GetItemCount(GameController.gadgetController.activeGadget[2] > 0);
     if(count > 0  ){ 
       return count;
     }else{
     return "";
     }
   }else{
     return "";
   }
 }
 public void SelectFirstWeapon(){
   this.activeWeaponSlot = "weapon1";
   var item = GameController.inventory.grids.equipment:GetItem(1);
   if(item  ){ 
     GameController.player.playerController:GetCurrentSlot():Message("OnWeaponSelect", item.name);
   }else{
     GameController.player.playerController:GetCurrentSlot():Message("OnWeaponSelect", "hero_hand");
   }
 }
 public void SelectSecondWeapon(){
   this.activeWeaponSlot = "weapon2";
   var item = GameController.inventory.grids.equipment:GetItem(2);
   if(item  ){ 
     GameController.player.playerController:GetCurrentSlot():Message("OnWeaponSelect", item.name);
   }else{
     GameController.player.playerController:GetCurrentSlot():Message("OnWeaponSelect", "hero_hand");
   }
 }
 public void SelectedSlotRepeated(weapon){
   if(this.activeWeaponSlot == weapon  ){ 
     return true;
   }else{
     return false;
   }
 }
 public void ActivateCurrentGadget(){
   
 }
 }}