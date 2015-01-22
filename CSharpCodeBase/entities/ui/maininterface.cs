using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class maininterface :unityexistsentity{
 public void init(self, gameObject);
       UnityExistsEntity.init(self, gameObject);
       this.unityInterface = gameObject:GetComponent("MainInterface");
       this.unityInterface:SetupLuaController(self);
       self:ToggleInventory();
       this.luaMapper:SetAlwaysVisible(true);
       GameController.eventSystem:AddListener("INVENTORY_SLOT_CHANGED_CORE", self);
       this.unityInterface:SetCallback(this.Callback);
     
       this.simpleRecipeList = RecipeList("data/config/recipes_simple.dat");
       this.unityInterface.GetCraftRecipe = public void (item1, item2){
         var result = this.simpleRecipeList:GetResult(item1, item2);
         return result;
       }
       this.unityInterface.GetMaxCraftValue = public void (resultItem) {
         var item1, item2 = unpack(this.simpleRecipeList:GetBackhashResult(resultItem));
         var count1 = GameController.inventory:GetItemCount(item1);
         var count2 = GameController.inventory:GetItemCount(item2);
         return math.min(count1, count2)        ;
       }
       this.containers = {}
 }})
 public void AddContainer(containerName, container){
   this.containers[containerName] = container;
 }
 public void RemoveContainer(containerName){
   this.containers[containerName] = null;
 }
 public void IsInventory(containerName){
   var result = false;
   for(k, v in pairs(GameController.inventory.grids) ){
     if(v._name == containerName  ){ 
       result = true;
     }
   }
   return result;
 }
 //using API
 public void ToggleInventory(){
   this.unityInterface:ToggleInventory();
 }
 public void SetSlot(container, index, item, count){
   this.unityInterface:SetSlot(container, index, item, count);
 }
 public void SelectSlot(container, index){
   this.unityInterface:SelectInContainer(container, index);
 }
 public void SetCharacterInfo(text){
   this.unityInterface:SetCharacterInfo(text);
 }
 public void SetCharacterSlot(index, name){
   this.unityInterface:SetCharacterSlot(index, name);
 }
 public void SelectCharacterSlot(index){
   this.unityInterface:SelectCharacterSlot(index);
 }
 public void ResetSpells()  {
   this.unityInterface:ResetSpells();
 }
 public void SetupSpell(...){
   this.unityInterface:SetupSpell(...);
 }
 public void GetSpellSlot(index){
   return this.unityInterface:GetSpellSlot(index);
 }
 public void IsAttackPossible(){
   return this.unityInterface:IsAttackPossible();
 }
 public void GetSpellStatus(index){
   return this.unityInterface:GetSpellStatus(index);
 }
 public void SetSpellStatus(index, status){
   this.unityInterface:SetSpellStatus(index, status);
 }
 public void Update(){
   if(Input.GetKeyDown(KeyCode.Tab)  ){ 
     this.unityInterface:ToggleInventory();
   }
 }
 public void OnEvent(e, params){
   if(e == "INVENTORY_SLOT_CHANGED_CORE"  ){ 
     var container, idx, item, count = unpack(params);
     self:SetSlot(container, idx - 1, item, count);
     GameController.gadgetController:OnInventaryChanged();
   }  
 }
 // Callbacks
 public void Callback(method, ...)  {
   if(self[method] != null  ){       
     var result = self[method](self, ...);
     return result;
   }else{
     return {}  
   }
 }
 public void OnSlotChanged(containerName, slotIndex, slotName, slotItem, slotCount)  {
   if(self:IsInventory(containerName)  ){ 
     GameController.inventory:SetSlotFromUI(containerName, slotIndex + 1, slotItem, slotCount);
     
     if(GameController.weaponController  &&  slotName == GameController.weaponController.activeWeaponSlot  ){ 
       GameController.player.playerController:GetCurrentSlot():Message("OnWeaponSelect", slotItem);
     }
     
     // check equipment
   }else{
     this.containers[containerName]:SetSlotFromUI(slotIndex + 1, slotItem, slotCount);
   }
 }
 public void OnSlotSelected(containerName, slotIndex, slotName, slotItem, slotCount){
   //print("Slot selected: " .. slotItem .. " - " .. slotCount)  
   //local characterEntity = GameController.player.playerController:GetCurrentSlot()
   //characterEntity.grenadeModeVisualizer:Disable()
   //if slotItem == "grenade"  &&  slotCount > 0  ){ 
   //  characterEntity.grenadeModeVisualizer:Enable(slotItem)
   //end
 }
 public void OnCharacterChanged(idx){
   // show inventory for(new character
 }
 public void OnSlotDropped(item, count){
   // drop items
 }
 public void OnSpellCursorBegin(spell, idx){
   if((not GameController.inputService:CheckLayer("Default Layer"))  ){ 
     this.unityInterface:CancelSpellCursor()    ;
   }    
 }
 public void OnSpellCursorEnd(spell, idx)  {
 }
 public void OnSpellApply(spell, idx, position)  {
   if(position  ){     
     return GameController.spellSystem:ApplySpell(spell, position);
   }else{    
     return GameController.spellSystem:ApplySpell(spell);
   }
 }
 public void OnSpellCursorCancel(spell, idx){
 }
 //without left trigger
 public void OnStartChoiseTarget(spell, idx) {
   if((GameController.player.playerController:GetCurrentSlot().gamepadRightStickController)  ){ 
     GameController.player.playerController:GetCurrentSlot().gamepadRightStickController:Disable();
   }
 }
 public void OnStopChoiseTarget(spell, idx){
   if((GameController.player.playerController:GetCurrentSlot().gamepadRightStickController)  ){ 
     GameController.player.playerController:GetCurrentSlot().gamepadRightStickController:Enable();
   }
 }
 ////
 public void OnCraftRequest(container1, idx1, container2, idx2, result, count){
   var resultIsMech = GameController.database.data.heroes[result] != null;
   var slotSource = idx1 + 1;
   var slotTarget = idx2 + 1  ;
   var item1, item2 =  unpack(this.simpleRecipeList:GetBackhashResult(result));
   // here we need to get items from container1(2)/id1(2) at first
   var count1 = count    ;
   var slotCount1;
   if(self:IsInventory(container1)  ){ 
     slotCount1 = GameController.inventory:GetSlotCount(container1, slotSource);
     count1 = count1 - GameController.inventory:RemoveFromSlot(container1, slotSource, count1);
     GameController.inventory:RemoveItems(item1, count1);
   }else{
     slotCount1 = this.containers[container1]:GetSlotCount(slotSource);
     count1 = count1 - this.containers[container1]:RemoveFromSlot(slotSource, count1);
     this.containers[container1]:RemoveItems(item1, count1);
   }
   
   var count2 = count;
   var slotCount2;
   var restItem = 0;
   if(self:IsInventory(container2)  ){ 
     slotCount2 = GameController.inventory:GetSlotCount(container2, slotTarget);
     count2 = count2 - GameController.inventory:RemoveFromSlot(container2, slotTarget, count2);
     GameController.inventory:RemoveItems(item2, count2);
     if(resultIsMech  ){ 
       GameController.player.playerController:LoadAndAdd(result);
     }else{
       restItem = GameController.inventory:AddItem(result, count);
     }
   }else{
     slotCount2 = this.containers[container2]:GetSlotCount(slotTarget);
     count2 = count2 - this.containers[container2]:RemoveFromSlot(slotTarget, count2);
     this.containers[container2]:RemoveItems(item2, count2);
     if(resultIsMech  ){ 
       GameController.player.playerController:LoadAndAdd(result);
     }else{
       restItem = this.containers[container2]:AddItem(result, count);
     }
   }
   
   if(restItem.count > 0  ){ 
     GameController.dropManager:DropItem(restItem.name, restItem.count, GameController.player:GetPosition());
   }    
 }
 }}