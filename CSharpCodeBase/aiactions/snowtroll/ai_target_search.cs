using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class ai_target_search :ainode{
 var AI_TargetSearch = Class({__includes=AINode, init = public void (self, entity, callback){
   AINode.init(self, entity);
   this.callback = callback;
   self:ResetTarget();
 }})
 public void AI_TargetSearch:ResetTarget(){
   this.currentTarget=nil;
 }
 public void AI_TargetSearch:Visit(){
   if(this.status == NODE_READY  ){ 
     self:ResetTarget();
     var possibleTargets = this.entity.vision:GetVisibleEntitiesByRSTag("alertness")  ||  this.entity.vision:GetVisibleEntitiesByRSTag("enemy");
     this.currentTarget = possibleTargets[1];
     if(this.currentTarget != null  ){ 
       this.status = NODE_RUNNING;
     }else{
       this.status = NODE_FAILURE;
     }
   }
   
   if(this.status == NODE_RUNNING  ){ 
     this.entity.goalProsecution = this.currentTarget;
     this.entity.lead = this.entity;
     this.entity._screamTrigger = true;
     this.entity._screamTarget = this.currentTarget;
     this.status = NODE_SUCCESS;
     this.callback(this.entity, self);
   }
 }
 return AI_TargetSearch}}