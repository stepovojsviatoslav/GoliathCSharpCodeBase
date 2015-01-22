using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class relationship :component{
 var RelationshipComponent = Class({__includes=Component, init = public void (self, entity){
       Component.init(self, entity);
       this.entity.relationship = self;
       this._tagTypes = {}
       this._tagInstances = {}
       self:RegisterTagTypes("enemy", this.entity.config:Get("enemy")  ||  {})
       self:RegisterTagTypes("friend", this.entity.config:Get("friend")  ||  {})
       self:RegisterTagTypes("parent", this.entity.config:Get("parent")  ||  {})
       self:RegisterTagTypes("alertness", this.entity.config:Get("alertness")  ||  {})
       self:RegisterTagTypes("scary", this.entity.config:Get("scary")  ||  {})
       self:RegisterTagTypes("food", this.entity.config:Get("food")  ||  {})
 }})
 public void RelationshipComponent:RegisterTagTypes(tag, types){
   this._tagTypes[tag] = types;
 }
 public void RelationshipComponent:GetTagTypes(tag){
   return this._tagTypes[tag];
 }
 public void RelationshipComponent:AddInstance(tag, entity, timeout){
   if(this._tagInstances[tag] == null  ){ 
     this._tagInstances[tag] = TimeoutList(timeout  ||  this.entity.config:Get("rsReactiveTimeout"));
   }else{if timeout != null  ){ 
     this._tagInstances[tag]:SetGlobalTimeout(timeout);
   }
   this._tagInstances[tag]:Add(entity);
 }
 public void RelationshipComponent:GetInstances(tag){
   if(this._tagInstances[tag] == null  ){ 
     this._tagInstances[tag] = TimeoutList(this.entity.config:Get("rsReactiveTimeout"));
   }  
   return this._tagInstances[tag]._items;
 }
 public void RelationshipComponent:HasInstance(tag, instance){
   if(this._tagInstances[tag] == null  ){ 
     return false;
   }
   return this._tagInstances[tag]:Exists(instance);
 }
 public void RelationshipComponent:GetInstance(tag){
   if(this._tagInstances[tag] == null  ){ 
     this._tagInstances[tag] = TimeoutList(this.entity.config:Get("rsReactiveTimeout"));
   }  
   return this._tagInstances[tag]._items[1];
 }
 public void RelationshipComponent:Update(){
   for(k, v in pairs(this._tagInstances) ){
     v:Update();
   }
 }
 Component}}