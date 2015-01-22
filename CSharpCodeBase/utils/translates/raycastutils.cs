using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class raycastutils {
 RaycastUtils = {}
 var _GetHeight = luanet.RaycastUtils.GetHeight;
 public void RaycastUtils.GetHeight(vec3){
   return _GetHeight(vec3.x, vec3.y, vec3.z);
 }
 var _GetEntitiesInRadius = luanet.RaycastUtils.GetEntitiesInRadius;
 public void RaycastUtils.GetEntitiesInRadius(vec3, radius){
   //local entities = {}
   var entities = GameController.objectMap:GetNearEntities(vec3, radius);
   //_GetEntitiesInRadius(vec3.x, vec3.y, vec3.z, radius, entities)
   return entities;
 }
 var _GetEntitiesInRadiusByTypes = luanet.RaycastUtils.GetEntitiesInRadiusByTypes;
 public void RaycastUtils.GetEntitiesInRadiusByTypes(vec3, radius, types){
   //local entities = {}
   var entities = GameController.objectMap:GetNearEntities(vec3, radius);
   var result = {}
   for(k, v in pairs(entities) ){
     if(Tables.Find(types, v.name) > -1  ){ 
       result[#result + 1] = v;
     }
   }
   //_GetEntitiesInRadiusByTypes(vec3.x, vec3.y, vec3.z, radius, types, entities)
   //return entities
   return result;
 }
 var _GetDamaged = luanet.RaycastUtils.GetDamaged;
 public void RaycastUtils.GetDamaged(vec3, radius, target){
   var entities = {}
   _GetDamaged(vec3.x, vec3.y, vec3.z, radius, target.x, target.z, entities);
   return entities;
 }
 public void RaycastUtils.GetSector(vec3, radius, forwardVector, angle){
   var circle;
 }}}