using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class ai_following :ainode{
 public void init (self, entity, r1, r2, callback1, callback2);
   AINode.init(self, entity);
   this.minRadius = r1;
   this.maxRadius = r2;
   this.callback1 = callback1;
   if(callback2 == null  ){ 
     this.callback2 = callback1;
   }else{
     this.callback2 = callback2;
   }
 }})
 public void Visit(){
   this.target = this.callback1(this.entity, self);
   this.avoidanceTarget = this.callback2(this.entity, self);
   
   if(this.status == NODE_READY  ){ 
     this.status = NODE_RUNNING;
   }
   
   if(this.status == NODE_RUNNING  ){ 
     var distance1 = this.entity:GetSimpleDistance(this.target);
     var distance2 = this.entity:GetSimpleDistance(this.avoidanceTarget);
     self:Moving(distance1, distance2);
     if(distance1 < this.maxRadius  &&  distance2 > this.minRadius  ){ 
       this.status = NODE_SUCCESS;
       this.entity.mover:Stop();
     }
   }
   return this.status;
 }
 public void Moving(distance1, distance2){
   if(distance2 < this.minRadius  ){ 
     var tmp = this.entity:GetPosition() - this.target:GetPosition();
     this.entity.mover:SetInput(tmp , 0, false);
   }else{if distance1 > this.maxRadius   ){ 
     var tmp = this.target:GetPosition() - this.entity:GetPosition();
     this.entity.mover:LookAt(this.target:GetPosition());
     this.entity.mover:SetInput(tmp , 0, false);
   }
 }
 }}