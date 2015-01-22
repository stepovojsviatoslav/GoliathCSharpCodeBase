using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class ai_movetoenemy :ainode{
 // Move to enemy
 public void init (self, entity);
   AINode.init(self, entity);
   this.timer = Timer(this.entity.config:Get("chaseTimeout"));
 }})
 public void Visit(){
   if(this.status == NODE_READY  ){ 
     this.status = NODE_RUNNING;
     this.entity.mover:ResetSpeed();
     this.entity.mover:SetGoal(this.parent.selected_target, null, this.entity.weaponContainer:GetAttackDistance());
     this.timer:Reset();
   }else{if this.status == NODE_RUNNING  ){ 
     if(not this.parent.selected_target.interactable  ){ 
       this.status = NODE_FAILURE;
       return;
     }
     if(not this.entity.mover:IsHaveGoal()  ){ 
       this.status = NODE_SUCCESS;
     }else{
       if(this.timer:Tick()  ){ 
         this.status = NODE_FAILURE;
       }
     }
   }
   return this.status;
 }
 }}