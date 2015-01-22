using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class scaredclass :aisequencenode{
 var AI_ScaredClass = Class({__includes=AISequenceNode, init = public void (self, entity){
       var childNodes = {
         AI_GetTargetClass(entity),;
         AI_RunawayFromTarget(entity),;
       }
       AISequenceNode.init(self, entity, childNodes);
 }})
 return AI_ScaredClass}}