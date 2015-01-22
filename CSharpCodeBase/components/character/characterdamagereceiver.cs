using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class characterdamagereceiver :component{
 var CharacterDamageReceiverComponent = Class({__includes=Component, init=function(self, entity)
       Component.init(self, entity);
 }})
 public void CharacterDamageReceiverComponent:OnApplyDamage(damageData){
   if(this.entity.WakeUp  ){ 
     this.entity:WakeUp();
   }
   var isDeath = this.entity.health:Decrease(damageData.summary) == 0;
   
   if(not isDeath  ){ 
     if(this.entity.relationship != null  ){ 
       this.entity.relationship:AddInstance("enemy", damageData.source);
     }
     if(damageData.effects.punch == "push"  ){ 
       print("Pushed!");
       if(this.entity.Pushed  ){ 
         this.entity:Pushed(damageData)//.source:GetPosition())
       }
     }else{
       if(this.entity.Hit  ){ 
         this.entity:Hit(damageData);
       }
     }
   }else{
     // death
     if(this.entity.Death  ){ 
       this.entity:Death();
     }
     //print("Enemy is death!")
   }
 }
 Component}}