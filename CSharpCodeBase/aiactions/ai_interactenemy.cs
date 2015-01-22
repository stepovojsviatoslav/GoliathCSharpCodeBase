using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class ai_interactenemy :ainode{
 // Attack enemy
 public void init (self, entity);
   AINode.init(self, entity);
 }})
 public void Visit(){
   if(this.status == NODE_READY  ){ 
     this.timeout = this.entity.weaponContainer:GetAttackTimeout(this.entity.weaponContainer.interactWeapon);
     this.status = NODE_RUNNING;
     this.attack = false;
     // start attacking
     this.entity.combat:Attack(this.parent.selected_target, this.entity.weaponContainer.interactWeapon);
   }
   
   if(this.status == NODE_RUNNING  &&  this.attack  ){ 
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
     this.attack = true;
     if(this.parent.selected_target.interactable  &&  this.entity.weaponContainer:CanAttack(this.parent.selected_target, this.entity.weaponContainer.interactWeapon)  ){ 
       var canBeFood = true;
       if(this.parent.selected_target.CanBeFood  ){ 
         canBeFood = this.parent.selected_target:CanBeFood();
       }
       if(canBeFood  ){ 
         this.parent.selected_target:Message("OnInteract", this.entity);
       }
       this.entity.starvation:SetState(false);
     }        
   }
 }
 }}