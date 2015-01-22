using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class possibleactions :component{
 public void init(self, entity);
   Component.init(self, entity);
   this.actions = {}
   this.defaultAction = "attack";
   self:LoadActions();
   this.entity.possibleActions = self;
 }})
 public void LoadActions(){
   this.actions = this.entity.config:Get("action")  ||  {}
 }
 public void GetAction(idx){
   if(idx < 1  ){ 
     return this.actions[1]  ||  this.defaultAction;
   }else{if idx > #this.actions  ){ 
     return this.actions[#this.actions]  ||  this.defaultAction;
   }else{
     return this.actions[idx]  ||  this.defaultAction;
   }
 }
 public void SetActions(actions){
   this.actions = actions;
 }
 }}