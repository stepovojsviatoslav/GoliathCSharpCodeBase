using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class ai_timeout :ainode{
 // Select enemy using ai vision
 public void init(self, entity, timeout);
   AINode.init(self, entity);
   this.timeout = timeout;
   this.currentTimeout = timeout;
 }})
 public void Visit(){
   if(this.status == NODE_READY  ){ 
     this.status = NODE_RUNNING;
     this.currentTimeout = this.timeout;
   }
   if(this.status == NODE_RUNNING   ){ 
     ////
     if(this.parent.selected_target != null  ){ 
       this.entity.mover:LookAt(this.parent.selected_target:GetPosition());
     }
     //
     this.currentTimeout = this.currentTimeout - GameController.deltaTime;
     if(this.currentTimeout <= 0  ){ 
       this.status = NODE_SUCCESS;
     }
   }
   return this.status;
 }
 }}