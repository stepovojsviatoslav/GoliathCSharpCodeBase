using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class mecanim :component{
 var MecanimComponent = Class({__includes=Component, init = public void (self, entity){
       Component.init(self, entity);
       this.animator = this.transform:GetComponent("Animator");
       this.entity.mecanim = self;
 }})
 public void MecanimComponent:ForceSetState(name, layer){
   this.animator:CrossFade(name, 0, layer  ||  -1, 0);
 }
 public void MecanimComponent:ForceSetNewState(name, time){
   this.animator:CrossFade(name, 0, -1, time);
 }
 public void MecanimComponent:SetFloat(name, val){
   this.animator:SetFloat(name, val);
 }
 public void MecanimComponent:SetBool(name, val){
   this.animator:SetBool(name, val);
 }
 public void MecanimComponent:ResetTrigger(name){
   this.animator:ResetTrigger(name);
 }
 public void MecanimComponent:SetTrigger(name){
   this.animator:SetTrigger(name);
 }
 public void MecanimComponent:CheckStateName(name, layer){
   Utils.CheckCurrentStateName(this.animator, name, layer  ||  0);
 }
 public void MecanimComponent:GetFloat(name){
   return this.animator:GetFloat(name);
 }
 Component}}