using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class daytime {
 public void init(self);
       
     this.PHASE_MORNING = 1;
     this.PHASE_DAY_START = 2;
     this.PHASE_DAY_PROGRESS = 3;
     this.PHASE_DAY_END = 4;
     this.PHASE_EVENING = 5;
     this.PHASE_NIGHT = 6;
     var configPhases = {"morning", "daystart", "day", "dayend", "evening", "night"}
     
     this.currentDay = 0;
     this.phaseTimes = {}
     this.phaseColors = {}
     this.phaseAmbientColors = {}
     
     for(k, v in pairs(configPhases) ){
       this.phaseTimes[k] = GameController.database:Get("daytime", v .. "/time");
       this.phaseColors[k] = GameController.database:Get("daytime", v .. "/color");
       this.phaseAmbientColors[k] = GameController.database:Get("daytime", v .. "/acolor");
     }
     this.currentPhase = this.PHASE_MORNING;
     this.currentPhaseTime = this.phaseTimes[this.currentPhase];
     this.currentPhaseTimeRunning = 0;
     self:OnLoadPhase(this.currentPhase);
 }})
 public void Update(){
   this.currentPhaseTimeRunning = this.currentPhaseTimeRunning + GameController.deltaTime;
   this.currentPhaseTime = this.currentPhaseTime - GameController.deltaTime;
   if(this.currentPhaseTime <= 0  ){ 
     self:SwitchPhase();
   }
 }
 public void SwitchPhase(){
   var previousPhase = this.currentPhase;
   this.currentPhase = this.currentPhase + 1;
   if(this.currentPhase > this.PHASE_NIGHT  ){ 
     this.currentPhase = this.PHASE_MORNING;
     this.currentDay = this.currentDay + 1;
   }
   this.currentPhaseTime = this.phaseTimes[this.currentPhase];
   this.currentPhaseTimeRunning = 0;
   self:OnUnloadPhase(previousPhase);
   self:OnLoadPhase(this.currentPhase);
 }
 public void OnUnloadPhase(phase){
   
 }
 public void OnLoadPhase(phase){
   print("Current phase " .. phase);
   GameController.eventSystem:Event("DAY_PHASE_CHANGED",;
     {
       current = phase,;
       color=this.phaseColors[phase],;
       ambientColor=this.phaseAmbientColors[phase],;
     })
 }
 }}