using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class wander :ainode{
 var AI_Wander = Class({__includes=AINode, init = public void (self, entity){
   AINode.init(self, entity);
   this.wanderPosition = null ;
   this.timeout = null;
   this.wanderRadius = this.entity.config:Get("wanderRadius");
 }})
 public void AI_Visit(){
   if(this.status == NODE_READY  ){ 
     // Choose new position
     self:NewPosition();
     this.status = NODE_RUNNING;
     this.entity.mover:ResetSpeed();
     this.entity.mover:SetGoal(this.wanderPosition);
   }else{if this.status == NODE_RUNNING  ){ 
     if(not this.entity.mover:IsHaveGoal()  ){ 
       if(this.idleTimeout > 0  ){ 
         this.idleTimeout = this.idleTimeout - GameController.deltaTime;
         if(this.idleTimeout <= 0  ){ 
           this.entity.mecanim:SetFloat("idle_type", math.random(0, this.entity.config:Get("wanderIdleTypes")));
           this.entity.mecanim:SetTrigger("idle");
         }
       }
       if(this.timeout > 0  ){ 
         this.timeout = this.timeout - GameController.deltaTime;
       }else{
         this.status = NODE_SUCCESS;
       }
     }
   }
   return this.status;
 }
 public void AI_NewPosition(){
   var aroundPosition = this.entity.worldmap:GetLocation("home");
   if(this.entity.isChild  &&  this.entity.relationship:GetInstance("parent") != null  ){ 
     aroundPosition = this.entity.relationship:GetInstance("parent"):GetPosition();
   }
   this.wanderPosition = aroundPosition;
     + Vector3(math.random(-this.wanderRadius, this.wanderRadius), ;
         0,;
         math.random(-this.wanderRadius, this.wanderRadius));
   this.timeout = math.random(this.entity.config:Get("wanderIdleMin"),;
     this.entity.config:Get("wanderIdleMax"));
   this.idleTimeout = math.random(0, this.timeout);
 }
 return AI_Wander}}