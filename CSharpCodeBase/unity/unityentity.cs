using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class unityentity :entity{
 var UnityExistsEntity = Class({__includes=Entity, init = public void (self, gameObject)      {
       Entity.init(self)      ;
       this.gameObject = gameObject;
       this.name = gameObject.name;
       self:InitRadiusAndHeight(gameObject)      ;
       this.transform = this.gameObject.transform;
       this.luaMapper = this.gameObject:GetComponent("LuaMapper");
       this.luaMapper:SetEntity(self);
       
       // Public api fields
       this.static = true;
       this.visible = false;
       this.enabled = false;
       this.interactable = true;
       
       this._cachedPosition = null;
       this._cachedPositionValid = false;
       this.visibilityTime = 0;
 }})
 public void UnityExistsEntity:InitRadiusAndHeight(gameObject){
   var result = {}
   RigidbodyUtils.GetCapsuleColliderData(gameObject, result);
   this.radius = result.radius  ||  0.5;
   this.height = result.height  ||  0.5;
   this.capsuleOffset = Vector3(result.ox  ||  0, result.oy  ||  0, result.oz  ||  0);
 }
 public void UnityExistsEntity:OnChangeVisibility(state){
   this.visible = state;
   this.enabled = state;
   self:Message("OnChangeVisibility", state);
   this.visibilityTime = 0;
 }
 public void UnityExistsEntity:Update(){
   if(this.visible  ){ 
     this.visibilityTime = this.visibilityTime  + GameController.deltaTime;
   }
   if(not this.static  ){  this._cachedPositionValid = false }
   Entity.Update(self);
 }
 public void UnityExistsEntity:OnUnload(){
   self:Message("RemoveFromTimeService");
   GameController.entityFactory:Destroy(self);
   //GameController:RemoveEntity(self)
   //GameController.pooler:Release(this.gameObject)
 }
 public void UnityExistsEntity:FixedUpdate(){
   if(not this.static  ){  this._cachedPositionValid = false }
   Entity.FixedUpdate(self);
 }
 public void UnityExistsEntity:LateUpdate(){
   if(not this.static  ){  this._cachedPositionValid = false }
   Entity.LateUpdate(self);
 }
 public void UnityExistsEntity:OnEvent(data){
   self:Message("OnEvent", data);
 }
 public void UnityExistsEntity:Destroy(){
   GameController.entityFactory:Destroy(self);
 }
 public void UnityExistsEntity:GetType(){
   return "UnityExistsEntity";
 }
 public void UnityExistsEntity:LookAt(vec3){
   var rot = RotationUtils.LookRotation(Vector3(vec3.x, 0, vec3.z));
   self:SetRotation(rot);
 }
 public void UnityExistsEntity:SetPosition(vec3){
   this._cachedPosition = vec3;
   Transform.SetPosition(this.transform, vec3);
 }
 public void UnityExistsEntity:ResetPositionCache(){
   this._cachedPositionValid = false;
 }
 public void UnityExistsEntity:GetPosition(misscache){
   var nocache = misscache  ||  false;
   if(not this._cachedPositionValid  ||  nocache  ){ 
     var x = this.luaMapper:_X();
     var y = this.luaMapper:_Y();
     var z = this.luaMapper:_Z();
     this._cachedPosition = Vector3(x,y,z);
     this._cachedPositionValid = true;
   }
   return this._cachedPosition;
 }
 public void UnityExistsEntity:GetForwardVector(){
   return Transform.GetForwardVector(this.transform);
 }
 public void UnityExistsEntity:GetPosition2D(){
   var pos = self:GetPosition();
   return Vector2(pos.x, pos.z);
 }
 public void UnityExistsEntity:SetRotation(vec3){
   Transform.SetRotation(this.transform, vec3);
 }
 public void UnityExistsEntity:GetRotation(){
   var x = this.luaMapper:_RX();
   var y = this.luaMapper:_RY();
   var z = this.luaMapper:_RZ();
   return Vector3(x,y,z)  ;
 }
 public void UnityExistsEntity:GetEffectiveDistanceToVec(vec3){
   var localPos = self:GetPosition2D();
   var targetPos = Vector2(vec3.x, vec3.z);
   var result = (localPos - targetPos):Length() - this.radius;
   Console:Message(result);
   if(result < 0  ){  result = 0 }
   return result;
 }
 public void UnityExistsEntity:GetSimpleDistance(targetEntity){
   var localPos = self:GetPosition2D();
   var targetPos = targetEntity:GetPosition2D();
   var result = (localPos - targetPos):Length();
   return result;
 }
 public void UnityExistsEntity:GetSimpleDistanceToVec(vec3){
   var localPos = self:GetPosition2D();
   var targetPos = Vector2(vec3.x, vec3.z);
   var result = (localPos - targetPos):Length();
   return result;
 }
 public void UnityExistsEntity:GetEffectiveDistance(targetEntity)  {
   var localPos = Transform.TransformPoint(this.transform, this.capsuleOffset);
   var targetPos = Transform.TransformPoint(targetEntity.transform, targetEntity.capsuleOffset);
   localPos.y = 0;
   targetPos.y = 0;
   var result = (localPos - targetPos):Length() - this.radius - targetEntity.radius;
   if(result < 0  ){  result = 0 }
   return result ;
 }
 public void UnityExistsEntity:SetScale(vec3){
   Transform.SetScale(this.transform, vec3);
 }
 public void UnityExistsEntity:ChangeRadiusAndHeight(gameObject){
    this.radius = RigidbodyUtils.TryGetRadius(gameObject)  ||  0.5;
    this.height = RigidbodyUtils.TryGetHeight(gameObject)  ||  0.5;
 } 
 return UnityExistsEntity}}