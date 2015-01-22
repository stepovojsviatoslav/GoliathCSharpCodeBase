using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class vision :component{
 var VisionComponent = Class({__includes=Component, init = public void (self, entity){
       Component.init(self, entity);
       this.entity.vision = self;
       this.radius = this.entity.config:Get("visionRadius");
       this._tags = {}
 }})
 public void VisionComponent:IsTargetVisible(targetEntity){
   return targetEntity:GetEffectiveDistance(this.entity) < this.radius;
 }
 public void VisionComponent:GetVisibleEntities(radius){
   return RaycastUtils.GetEntitiesInRadius(this.entity:GetPosition(), radius  ||  this.radius);
 }
 public void VisionComponent:GetVisibleEntitiesByTypes(types, radius){
   return RaycastUtils.GetEntitiesInRadiusByTypes(this.entity:GetPosition(),;
     radius  ||  this.radius,;
     types);
 }
 public void VisionComponent:GetVisibleEntitiesByRSTag(tag, radius){
   return self:GetVisibleEntitiesByTypes(this.entity.relationship:GetTagTypes(tag), radius);
 }
 Component}}