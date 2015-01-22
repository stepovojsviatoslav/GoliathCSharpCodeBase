using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class statemanagercomponent :component{
 public void init(self, entity);
       Component.init(self, entity);
       this.entity.stateManager = self;
       this.stateName = this.entity.config:Get("state_name");
       this.currentState = #this.stateName;
       
       this.stateTransform = {}
       this.stateAction = {}
       this.stateEnabled = {}
       this.stateTake = {}
       this.stateSwitchPhase = {}
       
       for(key, value in pairs(this.stateName) ){
         table.insert(this.stateTransform, this.entity.config:Get(value .. "_transform"));
         table.insert(this.stateAction, this.entity.config:Get(value .. "_action")  ||  "examine");
         table.insert(this.stateEnabled, this.entity.config:Get(value .. "_enabled")  ||  true);
         table.insert(this.stateTake, this.entity.config:Get(value .. "_take")  ||  "none");
         table.insert(this.stateSwitchPhase, this.entity.config:Get(value .. "_switch_phase")  ||  "none");
       }
       
       this.gameObject = {}
       self:SetPossibleActions(this.currentState);
       self:InitiateGameObjects();
       self:SetPhases(this.stateSwitchPhase[this.currentState]);
       this.entity:ChangeRadiusAndHeight(this.gameObject[this.currentState][1]);
       this.timer = 0;
 }})
 public void InitiateGameObjects() {
   for(i, word in pairs(this.stateTransform) ){
     var go = {}
     var mTable = StringUtils.split(word, ",");
     for(key, value in pairs(mTable) ){
       table.insert(go, this.entity.gameObject.transform:Find(value).gameObject);
     }
     table.insert(this.gameObject, go);
   }
 }
 public void SetPhases(value){
    this.restoreTime = StringUtils.split(value, ",");
 }
 public void OnStateEvent(event){
   var actionsString = this.entity.config:Get(this.stateName[this.currentState] .. "_event_" .. event);
   var tempTable = StringUtils.split(actionsString, ",");
   var actions = {}
   for(i=1, #tempTable, 1 ){
     for(key, value in string.gmatch(tempTable[i], "([_%a][_%w]*):([_%w]+)") ){
         actions[key] = value;
     }
   }
   
   if(actions["action"]   ){ 
     if(actions["action"] == "take"  ){ 
       self:TakeItem();
     }else{if actions["timeout"]  ){ 
       this.entity.timeManager:Add(actions["action"], tonumber(actions["timeout"]));
     }
   }
   
   if(actions["animation"]  ){ 
     self:PlayAnimation(actions["animation"]);
   }
   
   if(actions["reset"]  ){ 
     if(actions["reset"]=="health"  ){ 
       this.entity.health:SetNewHealth(this.entity.config:Get(this.stateName[this.currentState] .. "_health")  ||  this.entity.config:Get("healthMaxAmount")  ||  100);
     }
   }
   
   if(actions["transite"]  ){ 
     if(actions["transite"] == "death"  ){ 
       this.entity:Destroy();
     }else{
       self:SetStateByName(actions["transite"]);
     }
   }
   
   if(actions["changeDrop"]  &&  this.entity.dropManager  ){ 
     this.entity.dropManager:LoadDrop(actions["changeDrop"]);
   }
 }
 public void EntityHealthChanged(state){
   if(state == this.entity.health.STATE_HIGHT  ){ 
     this.entity:Message("OnStateEvent", "OnHealthHigh");
   }else{if state == this.entity.health.STATE_NORMAL  ){ 
     this.entity:Message("OnStateEvent", "OnHealthNormal");
   }else{if state == this.entity.health.STATE_LOW  ){ 
     this.entity:Message("OnStateEvent", "OnHealthLow");
   }
 }
 public void TakeItem() {
   GameController.inventory:AddItem(self:GetItemName(), self:GetItemCount());
 }
 public void GetNumberByName(name){
   for(key, value in pairs(this.stateName) ){
     if(value == name  ){ 
       return key;
     }
   }
 }
 public void PlayAnimation(name){
   if(this.entity.mecanim  ){ 
     this.entity.mecanim:ForceSetState(name);
   }
 }
 public void SetPossibleActions(index){
   this.entity.possibleActions:SetActions(StringUtils.split(this.stateAction[index], ","));
 }
 public void SetStateByName(name){
   var number = self:GetNumberByName(name);
   self:SetStateByNumber(number);
 }
 public void SetStateByNumber(number){
   this.entity:Message("OnStateEvent", "OnExit");
   this.currentState = number;
   self:ActivateGameObject(this.currentState);
   this.entity.interactable = this.stateEnabled[this.currentState];
   self:SetPossibleActions(this.currentState);
   self:SetPhases(this.stateSwitchPhase[this.currentState]);
   this.entity:Message("Clear");
   this.entity:Message("OnStateEvent", "OnEnter");
 }
 public void DeactivateAllGameObjects(){
    for(i=1, #this.gameObject, 1 ){ 
     for(j=1, #this.gameObject[i], 1 ){ 
       this.gameObject[i][j]:SetActive(false);
     }
    }
 }
 public void ActivateGameObject(i){
   self:DeactivateAllGameObjects();
   for(j=1, #this.gameObject[i], 1 ){ 
     this.gameObject[i][j]:SetActive(true);
   }
   this.entity:ChangeRadiusAndHeight(this.gameObject[i][1]);
 }
 public void GetItemName(){
   var itemName = "";
   for(key, value in string.gmatch(this.stateTake[this.currentState], "(%w+):(%w+)") ){
       itemName=key;
   }
   return itemName;
 }
 public void GetItemCount(){
   var itemCount = 0;
   for(key, value in string.gmatch(this.stateTake[this.currentState], "(%w+):(%w+)") ){
       itemCount=tonumber(value);
   }
   return itemCount;
 }
 public void Grow(result){
   for(key, value in pairs(this.restoreTime) ){
     if(GameController.daytime.currentPhase == GameController.daytime[value]  ||  value == "none"  ){  
       this.entity:Message("OnStateEvent", "OnGrow");
       result:Complete();
     }
   }
 }
 public void Load(storage){
   this.currentState = storage:GetInt("state", this.currentState);
   self:SetStateByNumber(this.currentState);
 }
 public void Save(storage){
   storage:SetInt("state", this.currentState);
 }
 }}