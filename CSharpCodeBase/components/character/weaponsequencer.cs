using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class weaponsequencer :component{
 var WeaponSequencerComponent = Class({__includes=Component, init=function(self, entity)
       Component.init(self, entity);
       this.index = 1;
       this.weapons = this.entity.config:Get("weaponSequence")  ||  {}
       this.entity.weaponSequencer = self;
 }})
 public void WeaponSequencerComponent:GetNextWeapon(){
   if(#this.weapons == 0  ){ 
     return null;
   }
   var result = this.index;
   this.index = this.index + 1;
   if(this.index > #this.weapons  ){ 
     this.index = 1;
   }
   return this.weapons[result];
 }
 Component}}