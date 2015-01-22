using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class grenade :unityexistsentity{
 var GrenageEntity = Class({__includes = UnityExistsEntity, init = public void (self, name, source, speed){
      var gameObject = GameController.pooler:Fetch(name);
      gameObject:GetComponent("Rigidbody").useGravity = false;
      GameController:AddEntity(self);
      UnityExistsEntity.init(self, gameObject);
      this.config = ConfigComponent("gadget", this.name);
      this.luaMapper:SetAlwaysVisible(true);
      self:SetPosition(Vector3(source.gameObject.transform.position.x, source.gameObject.transform.position.y + source.height / 2, source.gameObject.transform.position.z));
      this.speed = speed;
      this.dt = GameController.deltaTime;
      this.radius = this.config:Get("radius_exp")  ||  1;
      this.damage = this.config:Get("damage")  ||  {dType=100}
      this.currentCharacter = GameController.player.playerController:GetCurrentSlot();
      GameController.inventory:RemoveItems(this.name, 1);
 }})
 public void GrenageEntity:FixedUpdate(){
    this.speed.y = this.speed.y - (9.81 * this.dt);
    var position = Vector3(self:GetPosition().x + this.speed.x * this.dt, ;
                       self:GetPosition().y + this.speed.y * this.dt, ;
                       self:GetPosition().z + this.speed.z * this.dt);
    self:SetPosition(position);
   UnityExistsEntity.FixedUpdate(self);
 }
 public void GrenageEntity:OnCollisionEnter(targetEntity){
   var entities = RaycastUtils.GetEntitiesInRadius(self:GetPosition(), this.radius);
   for(k, v in pairs(entities) ){
     this.currentCharacter.damageProcessor:SendDamage(v, {damage=this.damage, effects = {}})
   }
   self:Destroy();
 }
 return GrenageEntity}}