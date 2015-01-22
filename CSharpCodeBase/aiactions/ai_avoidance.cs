using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class ai_avoidance :ainode{
 public void init (self, entity);
   AINode.init(self, entity);
 }})
 public void Visit(){
   this.target = this.entity.goalProsecution;
   if(this.target  ){ 
     if(this.status == NODE_READY  ){ 
       this.status = NODE_RUNNING;
       self:Moving();
     }else{if this.status == NODE_RUNNING  ){ 
       if(not this.target.interactable  ){ 
         this.status = NODE_FAILURE;
         return;
       }
       if(not this.entity.mover:IsHaveGoal()  ){ 
         this.status = NODE_SUCCESS;
       }
     }
   }
   return this.status;
 }
 public void Moving(){
   this.entity.mover:ResetSpeed();
   var tmp = this.entity:GetPosition() - this.target:GetPosition();
   this.entity.mover:SetInput(tmp , 0, true);
 }
 }}