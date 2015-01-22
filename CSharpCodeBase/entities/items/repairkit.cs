using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class repairkit :entity{
 var RepairKitEntity= Class({__includes=Entity, init=function(self, entity, name)            
   Entity.init(self);
   this.name = name;
   this.owner = entity;
 }})
 public void RepairKitEntity:Apply(){
   if(this.owner.characterClass > 0  ){ 
     this.owner:Message("Increase",  GameController.database:Get("items", this.name .."/hpcount"));
     GameController.inventory:RemoveItems(this.name, 1);
   }
 }
 Entity}}