using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class chest :unityexistsentity{
 var STATE_NORMAL = 0;
 var STATE_DAMAGED = 1;
 var STATE_DESTROYED = 2;
 var ChestEntity = Class({__includes=UnityExistsEntity, init = public void (self, gameObject){
     UnityExistsEntity.init(self, gameObject);
     this.config = ConfigComponent("constructions", this.name);
     self:AddComponent(PossibleActionsComponent);
     self:AddComponent(HealthComponent)    ;
     self:AddComponent(DamageVisualizerComponent);
     self:AddComponent(MecanimComponent);
     self:AddComponent(ResistComponent)    ;
     self:AddComponent(DamageReceiverComponent);
     self:AddComponent(ThingDamageReceiverComponent);
     
     this.currentState = STATE_NORMAL;
     this.nextState = STATE_NORMAL;
     
     this.gameObjects = {}      
     //this.gameObjects[STATE_NORMAL] = this.entity.gameObject.transform:Find("Group47944").gameObject    
     //this.gameObjects[STATE_DAMAGED] = this.entity.gameObject.transform:Find("Group47944").gameObject
     this.gameObjects[STATE_NORMAL] = this.gameObject;
     this.gameObjects[STATE_DAMAGED] = this.gameObject;
     this.gameObjects[STATE_DAMAGED]:SetActive(false);
     this.gameObjects[STATE_NORMAL]:SetActive(true) ;
     
     this.content = InventoryGrid(32, "chest");
 }})
 public void ChestEntity.Create(storage, x, y, z, v){
   storage:SetBool("isLua", true);
   storage:SetPosition(x, y, z)  ;
   storage:SetRotation(0, math.random(0, 360), 0);
 }
 public void ChestEntity:Save(storage){
   storage:SetBool("isLua", true);
   storage:SetTransform(this.transform)  ;
 }
 public void ChestEntity:Load(storage){
   storage:LoadTransform(this.transform);
   this.spawnPosition = self:GetPosition()  ;
 }
 public void ChestEntity:Update(){
   UnityExistsEntity.Update(self) ;
   if(this.currentState != this.nextState  ){ 
     self:SetState(this.nextState);
   }
 }
 public void ChestEntity:SetState(state){
   if(this.currentState != state  ){ 
     self:Deactivate(this.currentState);
     self:Activate(state);
     this.currentState = state;
   }
 }
 public void ChestEntity:Activate(state){
   this.gameObjects[state]:SetActive(true);
 }
 public void ChestEntity:Deactivate(state){
   this.gameObjects[state]:SetActive(false);
 }
 public void ChestEntity:DeactivateAll(){
   for(k, v in pairs (this.gameObjects) ){
     v:SetActive(false);
   }
 }
 public void ChestEntity:Hit(){
   var currentHealth = this.health.amount;
   if(this.state == STATE_NORMAL  ){ 
     if(currentHealth < 0.5 * this.health.maxAmount  ){ 
       this.nextState = STATE_DAMAGED    ;
     }
   } 
 }
 public void ChestEntity:DestroyThing(damageData){
   self:DeactivateAll();
   self:Destroy();
 }
 public void ChestEntity:Message(name, params)  {
   UnityExistsEntity.Message(self, name, params)  ;
   if(name == "OnInteract"  ){     
     GameController.eventSystem:Event("CHEST_PANEL_ACTIVATE", self);
   }
 }
 //work with grid
 public void ChestEntity:AddItem(item, count){
   if(count != null  ){ 
     item = InventoryItem(item, count);
   }  
   return this.content:AddItem(item);
 }
 public void ChestEntity:GetItemCount(item)  {
   return this.content:GetItemCount(item);
 }
 public void ChestEntity:SetSlotFromUI(idx, item, count){
   this.content:SetItem(idx, InventoryItem(item, count));
 }
 public void ChestEntity:GetSlotCount(idx){
   return this.content:GetItem(idx).count;
 }
 public void ChestEntity:RemoveFromSlot(idx, count){
   return this.content:RemoveFromSlot(idx, count);
 }
 public void ChestEntity:RemoveItems(item, count){
   this.content:RemoveItem(item, count)  ;
 }
 Entity}}