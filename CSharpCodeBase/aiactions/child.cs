using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class child :aisequencenode{
 var AI_Child = Class({__includes=AISequenceNode, init = public void (self, entity){
       var childNodes = {
         AI_ConditionalNode(entity, public void (entity){
             if(not entity.isChild  ){  return false }
             var parent = entity.relationship:GetInstance("parent");
             var safeRadius = entity.config:Get("parentSafeRadiusMax");
             return parent == null  ||  entity:GetSimpleDistance(parent) > safeRadius;
         }),
         AI_FindParent(entity),;
         AI_FollowParent(entity),;
       }
       AISequenceNode.init(self, entity, childNodes);
 }})
 return AI_Child}}