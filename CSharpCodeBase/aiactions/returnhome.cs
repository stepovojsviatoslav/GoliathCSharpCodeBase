using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class returnhome :ainode{
 var AI_ReturnHome = Class({__includes=AINode, init = public void (self, entity){
   AINode.init(self, entity);
   this.timeout = null;
   this.radius = this.entity.config:Get("returnHomeRadius");
 }})
 public void AI_Visit(){
   if(this.entity.isChild  &&  this.entity.relationship:GetInstance("parent") != null  ){ 
     this.status = NODE_FAILURE;
     return;
   }
   
   if(this.status == NODE_READY  ){ 
     this.status = NODE_FAILURE;
     if(this.entity.worldmap:GetDistanceTo("home") > this.radius  &&  this.timeout != null  ){ 
       this.timeout = this.timeout - GameController.deltaTime;
     }else{
       this.timeout = this.entity.config:Get("returnHomeTimeout");
     }
   }
   
   if(this.timeout < 0  &&  this.status != NODE_RUNNING  ){ 
     this.status = NODE_RUNNING;
     this.entity.mover:ResetSpeed();
     this.entity.mover:SetGoal(this.entity.worldmap:GetLocation("home"));
   }else{if this.status == NODE_RUNNING  ){ 
     if(not this.entity.mover:IsHaveGoal()  ){ 
       this.status = NODE_SUCCESS;
     }
   }
   return this.status;
 }
 return AI_ReturnHome}}