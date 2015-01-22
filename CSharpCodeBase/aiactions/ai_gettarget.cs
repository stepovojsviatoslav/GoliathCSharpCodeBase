using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class ai_gettarget :ainode{
 // We need to find parent, if(parent not exists
 public void init(self, entity);
   AINode.init(self, entity);
 }})
 public void Visit(){
   if(this.status == NODE_READY  ){ 
     var entities = this.entity.vision:GetVisibleEntitiesByRSTag("scary",;
       this.entity.config:Get("scaryRadius"));
     if(#entities > 0  ){ 
       this.status = NODE_SUCCESS;
       this.parent.target = entities[1];
     }else{
       this.status = NODE_FAILURE;
     }
   }
   self:Sleep(0.5);
 }
 }}