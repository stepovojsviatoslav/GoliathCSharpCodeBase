using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class actionmachine {
 public void init (self, emptyAction);
       this.buffer = {}
       this._bufferLimit = 2;
       this._previousAction = null;
       this._emptyAction = emptyAction;
 }})
 public void GetCurrentAction(){
   return #this.buffer > 0  &&  this.buffer[#this.buffer]  ||  this._emptyAction;
 }
 public void Flush(){
   var currentAction = self:GetCurrentAction();
   for(k, v in pairs(this.buffer) ){
     v:OnRemove();
     v:Free();
   }
   this.buffer = {}
 }
 public void FixedUpdate(){
   var currentAction = self:GetCurrentAction();
   if(currentAction != null  &&  currentAction:IsStarted()  ){ 
     currentAction:FixedUpdate();
   }
 }
 public void Update(){
   var currentAction = self:GetCurrentAction();
   if(currentAction != this._previousAction  ){ 
     if(this._previousAction != null  &&  this._previousAction:IsStarted()  ){ 
       this._previousAction:OnSuspend();
     }
     if(currentAction != null  ){ 
       if(currentAction:IsStarted()  ){ 
         currentAction:OnResume();
       }else{
         currentAction:OnStart();
       }
     }
     this._previousAction = currentAction;
   }else{if currentAction != null  &&  currentAction == this._previousAction  &&  not currentAction:IsStarted()  ){ 
     currentAction:OnStart();
   }
   
   if(currentAction != null  ){ 
     if(currentAction:Update()  ){ 
       table.remove(this.buffer, #this.buffer);
       currentAction:OnComplete();
       currentAction:Free();
       this._previousAction = null;
     }
   }
 }
 public void IsActionExists(name){
   for(k, v in pairs(this.buffer) ){
     if(v.name == name  ){  return true }
   }
   return false;
 }
 public void InsertAction(action){
   var idx = 1;
   for(k, v in pairs(this.buffer) ){
     if(v:GetPriority() <= action:GetPriority()  ){ 
       idx = idx + 1;
     }else{ 
       break;
     }
   }
   table.insert(this.buffer, idx, action)  ;
   return idx;
 }
 public void CutLimit(){
   while(#this.buffer > this._bufferLimit ){
     this.buffer[1]:OnRemove();
     this.buffer[1]:Free();
     table.remove(this.buffer, 1);
   }
 }
 public void RemoveActionsWithSamePriority(){
   var previousPriority = this.buffer[#this.buffer]:GetPriority();
   for(i = #this.buffer - 1, 1, -1 ){
     var currentPriority = this.buffer[i]:GetPriority();
     if(currentPriority == previousPriority  ){ 
       this.buffer[i]:OnRemove();
       this.buffer[i]:Free();
       table.remove(this.buffer, i);
     }else{
       previousPriority = currentPriority;
     }
   }
 }
 public void RemoveUncontinuousActions(){
   var needIterate = true;
   while(needIterate ){
     var isRemoved = false;
     for(k, v in pairs(this.buffer) ){
       if(k < #this.buffer  ){ 
         if(not v:IsContinuous()  &&  v:IsStarted()  ){ 
           v:OnRemove();
           v:Free();
           table.remove(this.buffer, k);
           isRemoved = true;
           break;
         }
       }
       needIterate = isRemoved;
     }
   }
 }
 public void PushAction(action){
   var currentAction = self:GetCurrentAction();
   self:InsertAction(action);
   self:CutLimit();
   self:RemoveActionsWithSamePriority();
   self:RemoveUncontinuousActions();
   action:OnPushed();
 }
 public void IsEmpty(){
   return #this.buffer == 0;
 }
 }}