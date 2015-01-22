using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class thingdamagereceiver :component{
 var ThingDamageReceiverComponent = Class({__includes=Component, init=function(self, entity)
       Component.init(self, entity);
 }})
 public void ThingDamageReceiverComponent:OnApplyDamage(damageData){
   var isDeath = this.entity.health:Decrease(damageData.summary) == 0;
   if(not isDeath  ){ 
     this.entity:Hit(damageData);
   }else{
     this.entity:DestroyThing(damageData);
   }
 }
 Component}}