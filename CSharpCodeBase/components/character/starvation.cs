using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class starvation :component{
 var StarvationComponent = Class({__includes=Component, init=function(self, entity)
       Component.init(self, entity);
       this.entity.starvation = self;
       this.state = false;
       this.timeout = this.entity.config:Get("starvationTimeout")  ||  2;
       this.currentTimeout = this.timeout;
 }})
 public void StarvationComponent:SetState(state){
   this.state = state;
   this.currentTimeout = this.timeout;
 }
 public void StarvationComponent:Update(){
   Component.Update(self);
   if(this.state == false  &&  this.entity.health:GetState() != "low"  ){ 
     var multiplier = this.entity.health:GetState() == "high"  &&  1  ||  2;
     this.currentTimeout = this.currentTimeout - GameController.deltaTime * multiplier;
     if(this.currentTimeout < 0  ){ 
       this.currentTimeout = this.timeout;
       this.state = true;
     }
   }
 }
 public void StarvationComponent:OnRespawn(){
   self:Reset();
 }
 public void StarvationComponent:Reset(){
   self:SetState(false);
 }
 public void StarvationComponent:GetState(){
   return this.state  ||  this.entity.health:GetState() == "low";
 }
 Component}}