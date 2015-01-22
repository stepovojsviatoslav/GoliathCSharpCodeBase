using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class combat :component{
 var CombatComponent = Class({__includes=Component, init=function(self, entity)
   Component.init(self, entity);
   this.comboTimeout = 0;
   this.currentAction = 1;
   this.entity.combat = self;
 }})
 public void CombatComponent:Attack(target, weapon){
   var actionTypes = this.entity.weaponContainer:GetActionTypes(this.target, weapon);
   
   if(os.clock() > this.comboTimeout  ||  this.currentAction > #actionTypes  ){ 
     this.currentAction = 1;
   }
   
   this.entity.weaponContainer:SetComboState(this.currentAction, weapon);
   //print("Attack action type: " .. actionTypes[this.currentAction][1])
   this.entity.mecanim:SetFloat("action_type", actionTypes[this.currentAction][1]);
   this.entity.mecanim:SetTrigger("action")    ;
   
   this.currentAction = this.currentAction + 1;
   if(this.currentAction > #actionTypes  ){ 
     this.currentAction = 1;
   }
   this.comboTimeout = os.clock() + actionTypes[this.currentAction][2];
   
   this.entity.mover:LookAt(target:GetPosition());
 }
 public void CombatComponent:ResetCombo(){
   this.currentAction = 1;
 }
 public void CombatComponent:AttackVector(vec3, weapon){
   var actionTypes = this.entity.weaponContainer:GetActionTypes(nil, weapon);
   if(os.clock() > this.comboTimeout  ||  this.currentAction > #actionTypes  ){ 
     this.currentAction = 1;
   }
   this.entity.weaponContainer:SetComboState(this.currentAction, weapon);
   this.entity.mecanim:SetFloat("action_type", actionTypes[this.currentAction][1]);
   this.entity.mecanim:SetTrigger("action")    ;
   this.currentAction = this.currentAction + 1;
   if(this.currentAction > #actionTypes  ){ 
     this.currentAction = 1;
   }
   this.comboTimeout = os.clock() + actionTypes[this.currentAction][2];
   this.entity.mover:LookAt(vec3);
 }
 Component}}