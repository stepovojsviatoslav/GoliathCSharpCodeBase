using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class uislotscontroller :unityexistsentity{
 public void init(self, gameObject);
       UnityExistsEntity.init(self, gameObject);
       this.slotsController = gameObject:GetComponent("UISlotsController");
       this.slotsController:Select("weapon", 0)      ;
       this.slotsController:SetupLuaController(self);
       this.luaMapper:SetAlwaysVisible(true);
       GameController.eventSystem:AddListener("INVENTORY_SLOT_CHANGED_CORE", self);
       this.simpleRecipeList = RecipeList("data/config/recipes_simple.dat");
       this.slotsController:Toggle();
 }})
 public void SetContainerSubTag(container, tag){
   this.slotsController:ChangeSubTag(container, tag);
 }
 public void OnEvent(e, params){
   if(e == "INVENTORY_SLOT_CHANGED_CORE"  ){ 
     var container, idx, item, count = unpack(params);
     this.slotsController:SetSlotFromCore(container, idx - 1, item, count);
   }
 }
 public void UserSetupSlot(container, slotId, item, count){
   GameController.inventory:SetSlotFromUI(container, slotId + 1, item, count);
 }
 public void UserSelectSlot(container, slotId, item, count){
   print("Slot: " .. slotId .. " selected in " .. container .. " to " .. item .. ":" .. count);
   if(container == "weapon"  &&  (count > 0  ||  item == "")  ){ 
     GameController.player.playerController:GetCurrentSlot():Message("OnWeaponSelect", item);
   }
 }
 public void UserUseSlot(container, slotId, item, count){
   print("Slot: " .. slotId .. " used in " .. container .. " to " .. item .. ":" .. count);
 }
 public void UserDropItem(item, count){
   GameController.dropManager:DropItem(item, count, GameController.player:GetPosition(), ;
     GameController.player.playerController:GetCurrentSlot(), ;
     Transform.GetForwardVector(GameController.player.transform));
 }
 public void GetCraftRecipe(item1, item2){
   return this.simpleRecipeList:GetResult(item1, item2);
 }
 public void GetMaxCraftValue(resultItem){
   var item1, item2 = unpack(this.simpleRecipeList:GetBackhashResult(resultItem));
   var count1 = GameController.inventory:GetItemCount(item1);
   var count2 = GameController.inventory:GetItemCount(item2);
   return math.min(count1, count2);
 }
 public void CraftRequest(item, count, id1, id2, container1, container2){
   var slotSource = id1 + 1;
   var slotTarget = id2 + 1;
   var item1, item2 =  unpack(this.simpleRecipeList:GetBackhashResult(item));
   // here we need to get items from container1(2)/id1(2) at first
   var count1 = count;
   var slotCount1 = GameController.inventory:GetSlotCount(container1, slotSource);
   count1 = count1 - GameController.inventory:RemoveFromSlot(container1, slotSource, count1);
   GameController.inventory:RemoveItems(item, count1);
   
   var count2 = count;
   var slotCount2 = GameController.inventory:GetSlotCount(container2, slotTarget);
   count2 = count2 - GameController.inventory:RemoveFromSlot(container2, slotTarget, count2);
   GameController.inventory:RemoveItems(item, count2);
   
   var restItem = GameController.inventory:AddItem(item, count);
   if(restItem.count > 0  ){ 
     GameController.dropManager:DropItem(restItem.name, restItem.count, GameController.player:GetPosition());
   }  
 }
 public void Update(){
   UnityExistsEntity.Update(self);
   if(Input.GetKeyDown(KeyCode.K)  ){ 
     GameController.inventory:AddItem("hero_axe", 1);
   }
   if(Input.GetKeyDown(KeyCode.Tab)  ){ 
     this.slotsController:Toggle();
   }
   if(Input.GetKeyDown(KeyCode.Escape)  ){ 
     this.slotsController:Toggle(false);
   }
 }
 }}