using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class aidebugnode :ainode{
 var AI_DebugNode = Class({__includes=AINode, init = public void (self, entity, result, string){
       AINode.init(self, entity);
       this.result = result;
       this.string = string;
 }})
 public void AI_DebugNode:Visit(){
   print(string);
   this.status = this.result;
 }
 return AI_DebugNode}}