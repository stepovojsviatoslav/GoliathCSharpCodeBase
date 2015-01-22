using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class aiifnode :ainode{
 var AI_ConditionalNode = Class({__includes=AINode, init = public void (self, entity, conditional){
       AINode.init(self, entity);
       this.conditional = conditional;
 }})
 public void AI_ConditionalNode:Visit(){
   if(this.status == NODE_READY  ){ 
     this.status = this.conditional(this.entity, self)  &&  NODE_SUCCESS  ||  NODE_FAILURE;
   }
 }
 return AI_ConditionalNode}}