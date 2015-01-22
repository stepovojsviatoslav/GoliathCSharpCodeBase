using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class grenade :entity{
 var GrenadeInventoryEntity= Class({__includes=Entity, init=function(self, entity, name)            
   Entity.init(self);
   this.name = name;
   this.owner = entity;
 }})
 public void GrenadeInventoryEntity:Apply(){
     this.owner.grenadeModeVisualizer:Disable();
     this.owner.grenadeModeVisualizer:Enable(this.name);
 }
 InventoryEntity}}