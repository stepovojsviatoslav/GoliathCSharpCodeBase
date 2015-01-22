using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class ai_selectweapon :ainode{
 // Select enemy using ai vision
 public void init(self, entity);
   AINode.init(self, entity);
 }})
 public void Visit(){
   if(this.status == NODE_READY  ){ 
     var nextWeapon = this.entity.weaponSequencer:GetNextWeapon();
     this.entity.weaponContainer:SetWeapon(nextWeapon);
     this.status = NODE_SUCCESS;
   }
   return this.status;
 }
 }}