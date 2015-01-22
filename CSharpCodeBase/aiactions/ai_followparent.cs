using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class ai_followparent :ainode{
 // Move to enemy
 public void init (self, entity);
   AINode.init(self, entity);
   this.radiusMax = this.entity.config:Get("parentSafeRadiusMax");
   this.radiusMin = this.entity.config:Get("parentSafeRadiusMin");
 }})
 public void Visit(){
   var parent = this.entity.relationship:GetInstance("parent");
   if(this.status == NODE_READY  ){ 
     if(this.entity:GetSimpleDistance(parent) < this.radiusMax  ){ 
       // nothing to ){, we are in safe-zone
       this.status = NODE_SUCCESS;
     }else{
       // we need move to parent
       this.status = NODE_RUNNING;
     }
   }else{if this.status == NODE_RUNNING  ){ 
     if(parent == null  ){  
       this.status = NODE_FAILURE;
     }else{if this.entity:GetSimpleDistance(parent) > this.radiusMin  ){ 
       this.entity.mover:ResetSpeed();
       this.entity.mover:SetInputToVec(parent:GetPosition());
     }else{
       this.status = NODE_SUCCESS;
     }
   }
 }
 }}