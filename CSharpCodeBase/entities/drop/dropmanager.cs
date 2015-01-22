using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class dropmanager {
 public void init(self);
 }})
 public void CreateDrop(items, position, sourceEntity){
   for(k,v in pairs(items) ){
     if(v > 0  ){ 
       self:DropItem(k, v, position, sourceEntity);
     }
   }
 }
 public void DropItem(item, count, position, sourceEntity, forceVector){
   var entity = GameController.entityFactory:CreateInWorld(item, position);
   entity:SetupDrop(item, count, sourceEntity, forceVector);
   return entity;
 }
 }}