using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class ai_findparent :ainode{
 // We need to find parent, if(parent not exists
 public void init(self, entity);
   AINode.init(self, entity);
 }})
 public void Visit(){
   if(this.status == NODE_READY  ){ 
     var parent = this.entity.relationship:GetInstance("parent");
     if(parent == null  ){ 
       parents = this.entity.vision:GetVisibleEntitiesByRSTag("parent");
       if(#parents > 0  ){ 
         this.entity.relationship:AddInstance("parent", parents[1], -1);
         this.status = NODE_SUCCESS;
       }else{
         this.status = NODE_FAILURE;
       }
     }else{
       this.status = NODE_SUCCESS;
     }
   }
   self:Sleep(0.5);
 }
 }}