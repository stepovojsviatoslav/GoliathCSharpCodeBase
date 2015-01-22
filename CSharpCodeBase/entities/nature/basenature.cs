using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class basenature :unityexistsentity{
 var BaseNatureEntity = Class({__includes=UnityExistsEntity, init = public void (self, gameObject){
     UnityExistsEntity.init(self, gameObject);
     this.config = ConfigComponent("nature", this.name);
     self:AddComponent(PossibleActionsComponent);
     self:AddComponent(StateManagerComponent);
     self:AddComponent(TimeManagerComponent);
     self:AddComponent(BuilderComponent);
 }})
 public void BaseNatureEntity.Create(storage, x, y, z, v){
   storage:SetBool("isLua", true);
   storage:SetPosition(x, y, z);
   storage:SetRotation(0, math.random(0, 360), 0);
   if(v.scale  ){ 
     var scale = 1;
     for(key, value in string.gmatch(v.scale, "(%d%p%d):(%d%p%d)") ){
       if(tonumber(key)  &&  tonumber(value)  ){     
         if(key == value  ){ 
           scale = tonumber(value);
         }else{          
           scale = math.random() * tonumber(key) + tonumber(value);
         }      
       }
     }
     storage:SetScale(scale, scale, scale);
   }  
 }
 public void BaseNatureEntity:Save(storage){
   storage:SetBool("isLua", true);
   storage:SetTransform(this.transform);
   self:Message("Save", storage);
 }
 public void BaseNatureEntity:Load(storage){
   storage:LoadTransform(this.transform);
   this.spawnPosition = self:GetPosition();
   self:Message("Load", storage);
 }
 public void BaseNatureEntity:Message(name, params){
   UnityExistsEntity.Message(self, name, params);
   if(name == "OnInteract"  ){ 
     self:Message("OnStateEvent", "OnTake");
   }
 }
 public void BaseNatureEntity:OnCollisionEnter(targetEntity){
   self:Message("OnCollisionEnter", targetEntity);
 }
 public void BaseNatureEntity:OnFragile(){
   self:Message("OnStateEvent", "OnFragile");
 }
 public void BaseNatureEntity:OnFragileForce(damageData){
   self:Message("OnStateEvent", "OnFragileForce");
   self:Message("OnFragileForce");
 }
 public void BaseNatureEntity:OnRegenerate(incValue){
   self:Message("Increase", incValue);
   self:Message("OnRegeneration");
 }
 public void BaseNatureEntity:DestroyThing(damageData){
   self:Message("OnStateEvent", "OnHealthZero");
   self:Message("DropAfterDeath");
   self:Message("OnDeath", damageData);
 }
 public void BaseNatureEntity:Hit(damageData){
   self:Message("OnStateEvent", "OnHit");
   self:Message("DropAfterHit");
 }
 Entity}}