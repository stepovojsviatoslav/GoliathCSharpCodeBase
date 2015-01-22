using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class sleep :ainode{
 var AI_Sleep = Class({__includes=AINode, init = public void (self, entity, sleepPhases){
   AINode.init(self, entity);
   this.sleepPhases = sleepPhases  ||  {GameController.daytime.PHASE_EVENING, GameController.daytime.PHASE_NIGHT}
 }})
 public void AI_CheckSleepPhase(){
   return Tables.Find(this.sleepPhases, GameController.daytime.currentPhase) > -1;
 }
 public void AI_Visit(){
   if(this.status == NODE_READY  ){ 
     if(self:CheckSleepPhase()  &&  this.entity:HasEmptyHitlist()  &&  math.chance(75)  ){ 
       this.status = NODE_RUNNING;
       this.entity:Sleep();
       this.sleepScanTimeout = this.entity.config:Get("sleepScanTimeout")  ||  5;
     }else{
       this.status = NODE_FAILURE;
       this.parent:Sleep(10);
     }
   }
   
   if(this.status == NODE_RUNNING  ){ 
     this.sleepScanTimeout = this.sleepScanTimeout - GameController.deltaTime;
     if(this.sleepScanTimeout < 0  ){ 
       this.sleepScanTimeout = this.entity.config:Get("sleepScanTimeout")  ||  5;
       var targets = this.entity.vision:GetVisibleEntitiesByRSTag("enemy", this.entity.config:Get("visionRadius") * 0.3);
       if(#targets > 0  ){ 
         if(math.chance(this.entity.config:Get("sleepStopChance")  ||  10)  ){ 
           this.status = NODE_SUCCESS;
           this.entity:WakeUp();
           this.entity:ScanEnemies();
         }
       }
     }
     if(not self:CheckSleepPhase()  ||  not this.entity.isSleeping  ){ 
       this.status = NODE_SUCCESS;
       //this.entity:WakeUp()
     }
   }
   return this.status;
 }
 var AI_WakeUp = Class({__includes=AINode, init = public void (self, entity){
   AINode.init(self, entity);
 }})
 public void AI_WakeUp:Visit(){
   if(this.status == NODE_READY  ){ 
     if(this.entity.isSleeping  ){ 
       this.entity:WakeUp();
       this.status = NODE_RUNNING;
     }else{
       this.status = NODE_FAILURE;
     }
   }
   
   if(this.status == NODE_RUNNING  ){ 
     if(this.entity.mecanim:CheckStateName("Idle")  ){ 
       print("Awakened!");
       this.status = NODE_SUCCESS;
     }
   }
   return this.status;
 }
 var AI_SleepCycle = Class({__includes=AISequenceNode, init = public void (self, entity, sleepPhases){
       var childNodes = {
         AI_Sleep(entity, sleepPhases),;
         AI_WakeUp(entity),;
       }
       AISequenceNode.init(self, entity, childNodes);
 }})
 return AI_SleepCycle}}