using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class aisequence :ainode{
 var AISequenceNode = Class({__includes=AINode, init = public void (self, entity, childNodes){
       AINode.init(self, entity, childNodes);
       this.idx = 1;
 }})
 public void AISequenceNode:Reset(){
   AINode.Reset(self);
   this.idx = 1;
 }
 public void AISequenceNode:Visit(){
   if(this.status != NODE_RUNNING  ){ 
     this.idx = 1;
   }
   
   while(this.idx <= #this.childNodes ){
     var childNode = this.childNodes[this.idx];
     var childStatus = NODE_FAILURE;
     if(not childNode:IsSleeping()  ){ 
       childNode:Visit();
       childStatus = childNode.status;
     }
     if(childStatus == NODE_RUNNING  ||  childStatus == NODE_FAILURE  ){ 
       if(childNode:IsSleeping()  ){ 
         this.status = NODE_FAILURE;
       }else{
         this.status = childNode.status;
       }
       return;
     }
     this.idx = this.idx + 1;
   }
   
   this.status = NODE_SUCCESS;
 }
 Node}}