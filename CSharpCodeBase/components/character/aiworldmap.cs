using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class aiworldmap :component{
 public void init (self, entity);
       Component.init(self, entity);
       this._map = {}
       this.entity.worldmap = self;
 }})
 public void AddLocation(locName, vec3){
   this._map[locName] = vec3;
 }
 public void GetLocation(locName){
   return this._map[locName]  ||  this.entity:GetPosition();
 }
 public void GetDistanceTo(locName){
   return this.entity:GetSimpleDistanceToVec(self:GetLocation(locName));
 }
 public void Save(storage){
   var i = 0;
   for(k, v in pairs(this._map) ){
     storage:SetFloat("aiworldmap_" .. i .. "x", v.x);
     storage:SetFloat("aiworldmap_" .. i .. "y", v.y);
     storage:SetFloat("aiworldmap_" .. i .. "z", v.z);
     storage:SetString("aiworldmap_" .. i .. "key", k);
     i = i + 1;
   }
   storage:SetInt("aiworldmap_count", #this._map);
 }
 public void Load(storage){
   var count = storage:GetInt("aiworldmap_count", 0);
   if(count == 0  ){ 
     self:AddLocation("home", this.entity:GetPosition(true));
   }else{
     for(i = 0, count ){
       var x = storage:GetFloat("aiworldmap_" .. i .. "x");
       var y = storage:GetFloat("aiworldmap_" .. i .. "y");
       var z = storage:GetFloat("aiworldmap_" .. i .. "z");
       var key = storage:GetString("aiworldmap_" .. i .. "key") ;
       self:AddLocation(key, Vector3(x, y, z));
     }
   }
 }
 }}