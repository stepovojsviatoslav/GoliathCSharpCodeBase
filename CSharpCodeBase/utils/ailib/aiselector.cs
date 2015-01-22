using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class aiselector :ainode{
 var AISelectorNode = Class({__includes=AINode, init = public void (self, entity, childNodes, reactive){
       AINode.init(self, entity, childNodes);
       this.idx = 1;
       this.reactive = reactive  &&  true  ||  false;
 }})
 public void AISelectorNode:Reset(){
   AINode.Reset(self);
   this.idx = 1;
 }
 public void AISelectorNode:Visit(){
   if(this.status != NODE_RUNNING  ||  this.reactive  ){ 
     this.idx = 1;
   }
  
   while(this.idx <= #this.childNodes ){
     var childNode = this.childNodes[this.idx];
     var childStatus = NODE_FAILURE;
     
     if(not childNode:IsSleeping()  ){ 
       childNode:Visit();
       childStatus = childNode.status;
     }
     
     if(childStatus == NODE_RUNNING  ||  childStatus == NODE_SUCCESS  ){ 
       this.status = childNode.status;
       return;
     }
     this.idx = this.idx + 1;
   }
   
   this.status = NODE_FAILED;
 }
 Node}}