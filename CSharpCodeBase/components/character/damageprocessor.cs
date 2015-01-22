using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class damageprocessor :component{
 var DamageProcessorComponent = Class({__includes=Component, init = public void (self, entity){
       Component.init(self, entity);
       this.entity.damageProcessor = self;
 }})
 public void DamageProcessorComponent:SendDamage(target, damage){
   var damageData = {
     target=target,;
     damage=damage.damage,;
     effects=damage.effects,;
     source=this.entity,;
   }
   target:Message("OnDamageReceive", damageData);
 }
 Component}}