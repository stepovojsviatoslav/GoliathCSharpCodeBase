using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class timemanagercomponent :component{
 var TimerResult = Class({init=function(self, name, timeLeft)
       this.name = name;
       this.complete = false;
       this.timeLeft = timeLeft      ;
 }})
 public void TimerResult:Complete(){
   this.complete = true;
 }
 var TimeManagerComponent = Class({__includes=Component, init=function(self, entity)
       Component.init(self, entity);
       this.entity.timeManager = self;
       this.actions = {}
       this.timeAppend = 1;
       GameController.timeService:Add(self);
 }})
 public void StableUpdate()  {
   var completed = {}
   for(i=1, #this.actions, 1 ){
     if(this.actions[i]["timerType"] == "default"  ){ 
       if(GameController.gameTime - this.actions[i]["currentTime"] > this.actions[i]["timeout"]  ){ 
         var tResult = TimerResult(this.actions[i]["name"], this.actions[i]["timeout"] - (GameController.gameTime - this.actions[i]["currentTime"]));
         if(this.entity[this.actions[i]["method"//  ){           
           this.entity[this.actions[i]["method"//(this.entity, tResult)
         }else{              
           this.entity:Message(this.actions[i]["method"], tResult);
         }
         if(tResult.complete  ){ 
           table.insert(completed, i);
         }else{
           this.actions[i]["currentTime"] = this.actions[i]["currentTime"] + this.timeAppend;
         }
       }
     }else{
       if(GameController.gameTime - this.actions[i]["currentTime"] > this.timeAppend  ){ 
         this.actions[i]["currentTime"] = this.actions[i]["currentTime"] + this.timeAppend;
         this.actions[i]["timeout"] = this.actions[i]["timeout"] - this.timeAppend;
         var tResult = TimerResult(this.actions[i]["name"], this.actions[i]["timeout"]);
         if(this.entity[this.actions[i]["method"//  ){           
           this.entity[this.actions[i]["method"//(this.entity, tResult)
         }else{              
           this.entity:Message(this.actions[i]["method"], tResult);
         }
         if(tResult.complete  ){ 
           table.insert(completed, i);
         }
       }
     }
   }  
   for(_, v in pairs(completed) ){
     table.remove(this.actions, v);
   }
 }
 public void Add(method, timeout, name, timerType){
   var tbl = {}
   tbl["method"] = method;
   tbl["timeout"] = timeout;
   tbl["currentTime"] = GameController.gameTime;
   tbl["name"] = name  ||  "";
   tbl["timerType"] = timerType  ||  "default";
   table.insert(this.actions, tbl)  ;
 }
 public void Clear(){
    this.actions = {}
 }
 public void RemoveFromTimeService(){
   GameController.timeService:Remove(self);
 }
 public void Load(storage){
   this.actions = {}
   var count = storage:GetInt("actionscout", 0);
   for(i=1, count, 1 ){
     var tbl = {}
     tbl["method"] = storage:GetString("method" .. tostring(i));
     tbl["timeout"] = storage:GetFloat("timeout" .. tostring(i));
     tbl["currentTime"] = storage:GetFloat("currentTime" .. tostring(i));
     table.insert(this.actions, tbl);
   }
 }
 public void Save(storage){
   for(i=1, #this.actions, 1 ){
     storage:SetString("method" .. tostring(i), this.actions[i]["method"]);
     storage:SetFloat("timeout" .. tostring(i), this.actions[i]["timeout"]);
     storage:SetFloat("currentTime" .. tostring(i), this.actions[i]["currentTime"]);
   }
   storage:SetInt("actionscout", #this.actions);
 }
 }}