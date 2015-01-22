using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class ai_selectenemy :ainode{
 // Select enemy using ai vision
 public void init(self, entity);
   AINode.init(self, entity);
 }})
 public void Visit(){
   if(this.status == NODE_READY  ){ 
     this.parent.selected_target = this.entity:GetPriorityTarget();
     this.status = this.parent.selected_target != null  &&  NODE_SUCCESS  ||  NODE_FAILURE;
   }
   return this.status;
 }
 }}