using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class bullet :unityexistsentity{
 var BulletEntity = Class({__includes = UnityExistsEntity, init = public void (self, bulletName, source, target, weapon){
       var gameObject = GameController.pooler:Fetch(bulletName);
       this._pooled = true;
       UnityExistsEntity.init(self, gameObject);
       this.luaMapper:SetAlwaysVisible(true);
       self:SetPosition(source:GetPosition() + Vector3(0, source.height / 2, 0));
       this.weapon = weapon;
       this.dv = Transform.GetForwardVector(source.transform);
       this.dv.y = 0;
       this.dv:Normalize();
       
       this.timeout = weapon.config:Get("bulletTimeout")  ||  2;
       this.speed = weapon.config:Get("bulletSpeed");
       this.damageRadius = weapon.config:Get("bulletDamageRadius")  ||  1;
       this.source = source;
       this.target = target;
       this.rigidbody = this.gameObject:GetComponent("Rigidbody");
       this.rigidbody.useGravity = false;
       RigidbodyUtils.ApplyImpulse(this.rigidbody, this.dv * this.speed);
 }})
 public void BulletEntity:OnCollisionEnter(targetEntity){
   //print("Bullet collision! " .. targetEntity.name)
   //this.entity.damageProcessor:SendDamage(target, self:GetDamage())  
   if(targetEntity != this.source  ){ 
     this.source.damageProcessor:SendDamage(targetEntity, this.weapon:GetDamage());
     this.timeout = 0;
   }
 }
 public void BulletEntity:Update(){
   UnityExistsEntity.Update(self);
   this.timeout = this.timeout - GameController.deltaTime;
   if(this.timeout < 0  ){ 
     self:Destroy();
   }
 }
 Entity}}