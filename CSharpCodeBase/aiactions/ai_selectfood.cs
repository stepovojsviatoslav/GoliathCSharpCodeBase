using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class ai_selectfood :ainode{
 // Select enemy using ai vision
 public void init(self, entity);
   AINode.init(self, entity);
 }})
 public void Visit(){
   if(this.status == NODE_READY  ){ 
     this.parent.selected_target = this.entity:GetPriorityFood();
     this.status = this.parent.selected_target != null  &&  NODE_SUCCESS  ||  NODE_FAILURE;
   }
   self:Sleep(0.5);
   return this.status;
 }
 }}