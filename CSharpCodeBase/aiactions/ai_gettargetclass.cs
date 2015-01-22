using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class ai_gettargetclass :ainode{
 // We need to find parent, if(parent not exists
 var AI_GetTarget = Class({__includes=AINode, init=function(self, entity)
   AINode.init(self, entity);
 }})
 public void AI_GetTarget:Visit(){
   if(this.status == NODE_READY  ){ 
     this.parent.target = null;
     var entities = this.entity.vision:GetVisibleEntities(this.entity.config:Get("scaryRadius"));
     for(k, v in pairs(entities) ){
       if(v.characterClass != null  &&  v.characterClass > this.entity.characterClass  ){ 
         this.parent.target = v;
         break;
       }
     }
     if(this.parent.target != null  ){ 
       this.status = NODE_SUCCESS;
     }else{
       this.status = NODE_FAILURE;
     }
   }
   self:Sleep(0.5);
 }
 return AI_GetTarget}}