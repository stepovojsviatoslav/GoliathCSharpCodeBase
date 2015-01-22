using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class actionhit :action{
 public void init (self, entity);
       Action.init(self, 4, false, "hit");
       this.entity = entity;
 }})
 public void OnStartRunning(){
   //Action.OnStart(self)
   this.entity.mover:Stop();
   this.entity.mecanim:SetFloat("action_type", 0);
   this.entity.mecanim:ForceSetState("Hit");
   this.isHitStarted = false;
 }
 public void Update(){
   if(not this.isHitStarted  ){ 
     this.isHitStarted = this.entity.mecanim:CheckStateName("Hit");
   }
   return this.isHitStarted  &&  not this.entity.mecanim:CheckStateName("Hit");
 }
 }}