using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class ai_attackenemy :ainode{
 // Attack enemy
 public void init (self, entity);
   AINode.init(self, entity);
 }})
 public void Visit(){
   if(this.status == NODE_READY  ){ 
     this.timeout = this.entity.weaponContainer:GetAttackTimeout();
     this.status = NODE_RUNNING;
     this.attack = false;
     // start attacking
     this.entity.combat:Attack(this.parent.selected_target);
   }
   
   if(this.status == NODE_RUNNING  &&  this.attack  ){ 
     ////
     if(this.parent.selected_target != null  ){ 
       this.entity.mover:LookAt(this.parent.selected_target:GetPosition());
     }
     //
     // calc timeout
     this.timeout = this.timeout - GameController.deltaTime;
     if(this.timeout <= 0  ){ 
       this.status = NODE_SUCCESS;
     }
   }
   
   return this.status;
 }
 public void OnEvent(data){
   if(data == "punch"  ){         
     this.entity.relationship:AddInstance("enemy", this.parent.selected_target);
     //print("Punch!")
     this.attack = true;
     // OK, now need to calculate damage
     if(this.entity.weaponContainer:CanAttack(this.parent.selected_target)  &&  this.parent.selected_target.interactable  ){ 
       if(this.entity.weaponContainer.currentWeapon.remote > 0  ){ 
         this.entity.weaponContainer:Attack(this.parent.selected_target);
       }else{
         if(HitTester.CheckHitEntity(this.entity, Transform.GetForwardVector(this.entity.transform), this.parent.selected_target, 80)  ){ 
           this.entity.weaponContainer:Attack(this.parent.selected_target);
         }
         //this.entity.damageProcessor:SendDamage(this.parent.selected_target, this.entity.weaponContainer.currentWeapon:GetDamage())
       }
     }else{
       print("Can not attack!");
     }
   }
 }
 }}