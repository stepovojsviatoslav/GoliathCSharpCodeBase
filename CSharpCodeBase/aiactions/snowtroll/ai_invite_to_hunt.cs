using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class ai_invite_to_hunt :ainode{
 // Select enemy using ai vision
 var AI_InviteToHunt = Class({__includes=AINode, init=function(self, entity)
   AINode.init(self, entity);
 }})
 public void AI_InviteToHunt:Visit(){
   if(this.status == NODE_READY  ){ 
     this.status = NODE_RUNNING;
     this.entity:InviteToHunt();
   }
   if(this.status == NODE_RUNNING  ){ 
     this.status = NODE_SUCCESS;
   }
   return this.status;
 }
 return AI_InviteToHunt}}