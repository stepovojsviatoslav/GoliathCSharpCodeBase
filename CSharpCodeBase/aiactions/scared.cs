using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class scared :aisequencenode{
 var AI_Scared = Class({__includes=AISequenceNode, init = public void (self, entity){
       var childNodes = {
         AI_ConditionalNode(entity, public void (entity){
             return #entity.relationship:GetTagTypes("scary") > 0;
         }),
         AI_GetTarget(entity),;
         AI_RunawayFromTarget(entity),;
       }
       AISequenceNode.init(self, entity, childNodes);
 }})
 return AI_Scared}}