using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class dropmanagercomponent :component{
 public void init(self, entity);
       Component.init(self, entity);
       this.config = ConfigComponent("drop", this.entity.name);
       this.entity.dropManager = self;
       
       this.healthCheckPoints = {}
       this.healthStep = {}
       self:LoadDrop(this.entity.config:Get("defaultDrop")  ||  "none");
 }})
 public void LoadDrop(entityName){
   if(entityName == "none"  ||  entityName == null  ){ 
     this.isActive = false;
     return ;
   }else{
     this.isActive = true;
   }
   this.dropItems = this.config:Get("drop_item_" .. entityName);
   this.dropEvents = this.config:Get("drop_event_" .. entityName);
   this.dropPerlifes = this.config:Get("drop_perlife_" .. entityName);
   this.dropChance = this.config:Get("drop_chance_" .. entityName);
   this.healthCheckPoints = {}
   this.healthStep = {}
   
   for(i=1, #this.dropEvents, 1 ){
     table.insert(this.healthStep,(this.entity.health.maxAmount/100)*this.dropPerlifes[i]);
     table.insert(this.healthCheckPoints, this.entity.health.maxAmount - (this.entity.health.maxAmount/100)*this.dropPerlifes[i]);
   }
 }
 public void DropAfterHit(){
   if(this.isActive == false  ){  return }
   for(i=1, #this.dropEvents, 1 ){
     if(this.dropEvents[i] == "hit"  ){ 
       var count = 0;
       count = self:Drop(i, count);
       if(count !=0  ){ 
         // вставить выпадание дропа
       }
     }
   }
 }
 public void DropAfterDeath(){
   if(this.isActive == false  ){  return }
   self:DropAfterHit();
   for(i=1, #this.dropEvents, 1 ){
     if(this.dropEvents[i] == "death"  ){ 
       if(self:IsDrop(i)  ){ 
         // вставить выпадание дропа
       }
     }
   }
 }
 public void Drop(i, dropCount){
   if(this.healthCheckPoints[i] >= this.entity.health.amount  ){ 
     this.healthCheckPoints[i] = this.healthCheckPoints[i] - this.healthStep[i];
     if(self:IsDrop(i)  ){ 
       dropCount = dropCount + 1;
     }
     dropCount = self:Drop(i, dropCount);
   }
   return dropCount;
 }
 public void IsDrop(i){
   if(math.chance(this.dropChance[i])  ){ 
     return true;
   }
   return false;
 }
 public void OnRegeneration(){
   if(this.isActive == false  ){  return }
   for(i=1, #this.healthCheckPoints, 1 ){
     if(this.healthCheckPoints[i] <= this.entity.health.amount  ){ 
       this.healthCheckPoints[i] = this.healthCheckPoints[i] + this.healthStep[i];
     }
   }
 }
 }}