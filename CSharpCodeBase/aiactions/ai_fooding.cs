using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class ai_fooding :aisequencenode{
 public void init (self, entity);
       var childNodes = {
         AI_SelectFood(entity),;
         AI_MoveToEnemy(entity),;
         AI_InteractEnemy(entity),;
       }
       AISequenceNode.init(self, entity, childNodes);
 }})
 }}