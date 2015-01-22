using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class scanenemies :ainode{
 var AI_ScanEnemies = Class({__includes=AINode, init=function(self, entity)
   AINode.init(self, entity);
 }})
 public void AI_Visit(){
   if(this.status == NODE_READY  ){ 
     this.entity:ScanEnemies();
     this.status = NODE_FAILURE // keep it failure to running tree
   }
   self:Sleep(0.3);
   return this.status;
 }
 return AI_ScanEnemies}}