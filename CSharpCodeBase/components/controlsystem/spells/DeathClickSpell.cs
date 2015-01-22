using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class DeathClickSpell :action{
 var STATE_MOVING = 0;
 var STATE_REQUEST_ATTACK = 1;
 var STATE_ATTACK = 2;
 var SpellAction = Class({__includes=Action, init = public void (self, name){
       Action.init(self, 3, false, name);
       this.position = null;
 }})
 public void SpellAction:OnStartRunning()    {
   // call spell animation
   if(this.position  ){     
     this.target = Input.RaycastTargetEntityWithRadius(1, this.position)        ;
   }else{
     this.target = Input.RaycastTargetEntityWithRadius(1)    ;
   }
 }
 public void SpellAction:OnStopRunning(){
   // stop spell animation?
 }
 public void SpellAction:Update(){
   if(not this.target  ){  return true }
   
   if(this.target.Death  ){ 
     this.target:Death();
   }
   // cast spell
   return true;
 }
 public void SpellAction:OnEvent(data){
   // Cast spell by animation here
 }
 public void SpellAction:IsContinuous(){
   return false;
 }
 public void SpellAction:BeginDraw(){
 }
 public void SpellAction:StopDraw(){
 }
 public void SpellAction.CanApply(position)    {
   if(position  ){         
     return Input.RaycastTargetEntityWithRadius(1, position) != null;
   }else{        
     return Input.RaycastTargetEntityWithRadius(1) != null;
   }
 }
 return SpellAction}}