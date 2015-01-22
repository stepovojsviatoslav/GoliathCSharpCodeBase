using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class ai_runawayfromtarget :ainode{
 // Move to enemy
 public void init (self, entity);
   AINode.init(self, entity);
   this.distance = this.entity.config:Get("scarySafeRadius");
 }})
 public void Visit(){
   var target = this.parent.target;
   if(this.status == NODE_READY  ){ 
     this.status = NODE_RUNNING;
   }else{if this.status == NODE_RUNNING  ){ 
     if(this.entity:GetSimpleDistance(target) > this.distance  ){ 
       this.status = NODE_SUCCESS;
       this.entity.mover:Stop();
     }else{
       this.entity.mover:SetSpeed(this.entity.config:Get("scaryRunSpeed"));
       this.entity.mover:SetInputFromVec(target:GetPosition(), false, this.entity.config:Get("scaryRunType"));
     }
   }
 }
 }}