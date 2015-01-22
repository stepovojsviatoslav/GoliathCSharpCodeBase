using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class enemysupportcomponent :component{
 public void init(self, entity);
       Component.init(self, entity);
 }})
 public void OnFriendAttacked(data){
   this.entity:WakeUp();
   if(not this.entity.relationship:HasInstance("enemy", data.target)  ){ 
     if(math.chance(this.entity.config:Get("supportScreamChance"))  ){ 
       this.entity._screamTrigger = true;
       this.entity._screamTarget = data.target;
     }
   }
   this.entity.relationship:AddInstance("enemy", data.target);
 }
 }}