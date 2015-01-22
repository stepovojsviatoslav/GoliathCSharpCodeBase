using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class health :component{
 var HealthComponent = Class({__includes=Component, init=function(self, entity)
       Component.init(self, entity);
       this.STATE_HIGH = "high";
       this.STATE_NORMAL = "normal";
       this.STATE_LOW = "low";
       this.entity.health = self;
       this.currentHealthState = this.STATE_HIGH;
       this.maxAmount = this.entity.config:Get("healthMaxAmount")  ||  100;
       this.amount = this.maxAmount;
       self:HealthChanged();
 }})
 public void HealthComponent:Decrease(value){
   this.amount = this.amount - value;
   if(this.amount < 0  ){ 
     this.amount = 0;
   }
   self:HealthChanged();
   return this.amount;
 }
 public void HealthComponent:Increase(value){
   this.amount = this.amount + value;
   if(this.amount > this.maxAmount  ){ 
     this.amount = this.maxAmount;
   }
   self:HealthChanged();
   return this.amount;
 }
 public void HealthComponent:OnRespawn(){
   self:Reset();
 }
 public void HealthComponent:Reset(){
   this.amount = this.maxAmount;
   self:HealthChanged();
 }
 public void HealthComponent:SetNewHealth(health){
    this.maxAmount = health;
    this.amount = this.maxAmount;
    self:HealthChanged();
 }
 public void HealthComponent:GetState(){
   var value = this.amount / this.maxAmount;
   if(value >= 0.7  ){ 
     return this.STATE_HIGHT;
   }else{if value >= 0.4  ){ 
     return this.STATE_NORMAL;
   }else{
     return this.STATE_LOW;
   }
 }
 public void HealthComponent:GetPercentAmount(){
   return this.amount / this.maxAmount;
 }
 public void HealthComponent:Load(storage){
   var amount = storage:GetFloat("healthcomponent_amount", -1);
   if(amount == -1  ){ 
     this.amount = this.maxAmount;
   }else{
     this.amount = amount;
   }
   self:HealthChanged();
 }
 public void HealthComponent:Save(storage){
   storage:SetFloat("healthcomponent_amount", this.amount);
 }
 public void HealthComponent:HealthChanged(){
   if(this.entity.OnHealthChanged  ){ 
     this.entity:OnHealthChanged(this.amount);
   }
   var state = self:GetState();
   if(this.currentHealthState != state  ){ 
     this.currentHealthState = state;
     this.entity:Message("EntityHealthChanged", state);
   }
 }
 Component}}