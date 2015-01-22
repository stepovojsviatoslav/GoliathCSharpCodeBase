using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class targetzone :ainode{
 var AI_TargetZone = Class({__includes=AINode, init = public void (self, entity){
   AINode.init(self, entity);
   this.targetZoneRadius=this.entity.config:Get("targetZoneRadius");
   this.targetZoneTimeout=this.entity.config:Get("targetZoneTimeout");
   self:ResetTimeoutAndTarget();
   this.currentTarget = null;
 }})
 public void AI_ResetTimeoutAndTarget(){
   this.timeout = this.targetZoneTimeout;
   this.currentTarget=nil;
 }
 public void AI_Visit(){
   if(this.status == NODE_READY  ){ 
     self:ResetTimeoutAndTarget();
     var possibleTargets = this.entity.vision:GetVisibleEntitiesByRSTag("alertness");
     this.currentTarget = possibleTargets[1];
     if(this.currentTarget != null  ){ 
       this.status = NODE_RUNNING;
       this.entity.mover:Stop();
     }else{
       this.status = NODE_FAILURE;
       self:Sleep(0.5);
     }
   }
   
   if(this.status == NODE_RUNNING  ){ 
     if(this.entity:GetSimpleDistance(this.currentTarget) > this.targetZoneRadius  ){ 
       this.status = NODE_FAILURE;
     }else{
       this.entity.mover:LookAt(this.currentTarget:GetPosition());
       this.timeout = this.timeout - GameController.deltaTime;
       if(this.timeout < 0  ){ 
         this.entity.relationship:AddInstance("enemy", this.currentTarget);
         this.entity._screamTrigger = true;
         this.entity._screamTarget = this.currentTarget;
         this.status = NODE_SUCCESS;
       }
     }
   }
 }
 return AI_TargetZone}}