using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class builderentity :entity{
 public void init (self);
       Entity.init(self);
       this.enabled = true;
       this.active = false;
       this.static = false;
       this.doneCallback = null;
       this.interruptCallback = null;
 }})
 public void StartConstruction(entityName, ){neCallback, interruptCallback){
   this.entity = GameController.entityFactory:Create(entityName, Input.RaycastMouseOnTerrain());
   this.entity:Enable();
   this.active = true;
   this.doneCallback = ){neCallback
   this.interruptCallback = interruptCallback;
 }
 public void InterruptConstruction(){
   this.entity:Disable();
   this.active = false;
   if(this.interruptCallback  ){ 
       self:interruptCallback();
     }
 }
 public void DoneConstruction(){
     GameController.entityFactory:CreateInWorld("TreeFirEntity", this.entity:GetPosition());
     this.entity:Disable();
     this.active = false;
     if(this.doneCallback  ){ 
       self:doneCallback();
     }
 }
 public void Update(){
   Entity.Update(self);
   if(this.active  ){ 
     if(Input.GetKeyDown(KeyCode.Escape)  ){ 
       self:InterruptConstruction();
     }
     
     if(GameController.inputService:BottomButtonWasPressed()  ||  Input.GetKeyDown(KeyCode.KeypadEnter)  ){ 
       if(this.entity.count == 0  ){ 
         self:DoneConstruction();
       }
     }
     
     if(Input.GetKey(KeyCode.KeypadPlus)  ){ 
       //повернуть +
       this.entity:Rotate(1);
     }else{if Input.GetKey(KeyCode.KeypadMinus)  ){ 
       this.entity:Rotate(-1);
        //повернуть -
     }
     
     if(Input.GetKey(KeyCode.UpArrow)  ){ 
       this.entity:MoveUpTransform(5);
     }else{if Input.GetKey(KeyCode.DownArrow)  ){ 
       this.entity:MoveUpTransform(-5);
     }
     if(Input.GetKey(KeyCode.RightArrow)  ){ 
       this.entity:MoveRightTransform(5);
     }else{if Input.GetKey(KeyCode.LeftArrow)  ){ 
       this.entity:MoveRightTransform(-5);
     }
     
   }else{
     if(Input.GetKeyDown(KeyCode.B)  ){ 
       self:StartConstruction("MockupEntity");
     }
   }
 }
 }}