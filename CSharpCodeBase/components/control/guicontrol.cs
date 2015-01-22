using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class guicontrol :component{
 public void init(self, entity);
       Component.init(self, entity);
       this.entity.guiControl = self      ;
 } })
 public void Update(){
   if(Input.GetKey(KeyCode.Alpha0)  ){ 
     this.entity.playerController:SelectSlot(1)  ;
   }else{if Input.GetKey(KeyCode.Alpha9)  ){ 
     this.entity.playerController:SelectSlot(2);
   }
 }
 }}