using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class weaponcontainer :component{
 var CharacterWeaponContainer = Class({__includes=Component, init = public void (self, entity){
       Component.init(self, entity);
       this.entity.weaponContainer = self;
       var defaultWeaponName = this.entity.config:Get("defaultWeapon");
       var interactWeaponName = this.entity.config:Get("interactWeapon")  ||  defaultWeaponName;
       var takeWeaponName = this.entity.config:Get("takeWeapon")  ||  defaultWeaponName;
       self:SetWeapon(defaultWeaponName);
       this.interactWeapon = self:LoadWeapon(interactWeaponName);
       this.takeWeapon = self:LoadWeapon(takeWeaponName);
 }})
 public void CharacterLoadWeapon(name){
   var classPath = GameController.database:Get("weapons", name .."/classPath");
   var class = require(classPath);
   var weapon = class(this.entity, name);
   return weapon;
 }
 public void CharacterSetWeapon(weapon){
   if(weapon == null  ||  weapon == ""  ){ 
     weapon = this.entity.config:Get("defaultWeapon");
   }
   if(this.currentWeapon != null  ){ 
     this.currentWeapon:OnDeactivate();
   }
   this.currentWeapon = self:LoadWeapon(weapon);
   this.currentWeapon:OnActivate();
 }
 public void CharacterGetActionTypes(target, weapon){
   // if(target is hardcoded action, just replace action target here
   var currentWeapon = weapon  ||  this.currentWeapon;
   return currentWeapon:GetActionTypes(target);
 }
 public void CharacterCanAttack(target, weapon){
   var currentWeapon = weapon  ||  this.currentWeapon;
   return currentWeapon:CanAttack(target);
 }
 public void CharacterGetAttackDistance(weapon){
   var currentWeapon = weapon  ||  this.currentWeapon;
   return currentWeapon:GetAttackDistance();
 }
 public void CharacterGetAttackTimeout(weapon){
   var currentWeapon = weapon  ||  this.currentWeapon;
   return currentWeapon:GetAttackTimeout();
 }
 public void CharacterGetAttackPrepareTimeout(weapon){
   var currentWeapon = weapon  ||  this.currentWeapon;
   return currentWeapon:GetAttackPrepareTimeout();
 }
 public void CharacterOnWeaponSelect(weapon){
   if(this.currentWeapon.name != weapon  ){ 
     if(weapon != ""  &&  weapon != null  ){ 
       if(GameController.database:Get("items", weapon .."/subTag") == this.entity.config:Get("weaponTag")  ){ 
         self:SetWeapon(weapon);
       }else{
         self:SetWeapon(nil);
       }
     }else{
       self:SetWeapon(nil);
     }
   }
 }
 public void CharacterAttack(target, weapon){
   var currentWeapon = weapon  ||  this.currentWeapon;
   currentWeapon:Attack(target);
 }
 public void CharacterSetComboState(state, weapon){
   var currentWeapon = weapon  ||  this.currentWeapon;
   currentWeapon.currentComboState = state;
 }
 return CharacterWeaponContainer}}