using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class attackenemies :aisequencenode{
 var AI_AttackEnemies = Class({__includes=AISequenceNode, init = public void (self, entity){
       var childNodes = {
         AI_SelectEnemy(entity),;
         AI_SelectWeapon(entity),;
         AI_MoveToEnemy(entity),;
         AI_Timeout(entity, entity.weaponContainer:GetAttackPrepareTimeout()),;
         AI_AttackEnemy(entity),;
       }
       AISequenceNode.init(self, entity, childNodes);
 }})
 return AI_AttackEnemies}}