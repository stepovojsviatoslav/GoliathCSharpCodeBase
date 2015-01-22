using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class thingregenerationcomponent :component{
 var PercentLifeRegenerationString = "PercentLifeRegeneration";
 var TimeUpdateLifeRegenerationString = "TimeUpdateLifeRegeneration";
 var LifeRegenerationNextStateString = "LifeRegenerationNextState";
 public void init(self, entity);
   Component.init(self, entity);
   this.entity.regeneration = self;
       
   this.percentRegeneration = this.entity.config:Get("percentRegeneration")  ||  10;
   this.timeStep = this.entity.config:Get("timeStepRegeneration")  ||  25;
   self:SetPhases(this.entity.config:Get("regenerationPhases")  ||  "none");
       
   this.nextRegenerationTimeCheckPoint = this.timeStep + GameController.gameTime;
   this.healthRegenerationValue = (this.entity.health.maxAmount/100)*this.percentRegeneration      ;
   this.isStarted = false;
   
   self:Start();
 }})
 public void SetPhases(value){
    this.regenerationTime = StringUtils.split(value, ",");
 }
 public void Start(){
   this.nextRegenerationTimeCheckPoint = this.timeStep + GameController.gameTime;
   this.isStarted = true;
 }
 public void Stop(){
   this.isStarted = false;
 }
 public void Update(){
   if(this.isStarted  ){ 
     self:PassiveRegeneration();
   }
 }
 public void PassiveRegeneration(){
   if(GameController.gameTime >= this.nextRegenerationTimeCheckPoint  ){ 
    for(key, value in pairs(this.regenerationTime) ){
       if(GameController.daytime.currentPhase == GameController.daytime[value]  ||  value == "none"  ){  
         this.nextRegenerationTimeCheckPoint = this.timeStep + GameController.gameTime;
         this.entity:OnRegenerate(this.healthRegenerationValue);
         return;
       }
     }
   }
 }
 }}