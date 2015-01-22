using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class player :unityexistsentity{
 public void init (self, gameObject);
       UnityExistsEntity.init(self, gameObject);
       self:AddComponent(PlayerController);
       self:AddComponent(GuiControl);
       this.luaMapper:SetAlwaysVisible(true);
       this.static = false;
 }})
 public void Update(){
   UnityExistsEntity.Update(self);
 }
 public void FixedUpdate(){
   UnityExistsEntity.FixedUpdate(self);
 }
 public void LateUpdate(){
   UnityExistsEntity.LateUpdate(self)  ;
 }
 public void StoreHomePosition(position){
   this.homePosition = position;
 }
 }}