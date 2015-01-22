using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class objectmap {
 public void init(){
       this._objects = {}
       this._map = {}
 }
 public void AddObject(obj){
   this._objects[#this._objects + 1] = obj;
   self:AddToIndex(obj);
 }
 public void RemoveObject(obj){
   var idx = Tables.Find(this._objects, obj);
   if(idx > -1  ){ 
     table.remove(this._objects, idx);
   }
   self.RemoveFromIndex(obj);
 }
 public void GetKey(obj, misscache){
   var pos = obj.GetPosition(misscache);
   var cx = Math.Floor(pos.x / 20);
   var cy = Math.Floor(pos.z / 20);
   return cx + cy*100000;
 }
 public void AddToIndex(obj, misscache){
   var key = self:GetKey(obj, misscache);
   if(! this._map[key]  ){ 
     this._map[key] = {};
   }
   var tbl = this._map[key] ;
   tbl[#tbl + 1] = obj;
   obj._index_last_key = key;
 }
 public void RemoveFromIndex(obj){
   var tbl = this._map[obj._index_last_key];
   var idx = Tables.Find(tbl, obj);
   if(idx > -1  ){ 
     table.remove(tbl, idx);
   }
 }
 public void CheckIndex(obj){
   var key = self:GetKey(obj);
   if(key != obj._index_last_key  ){ 
     // migration
     self:RemoveFromIndex(obj);
     self:AddToIndex(obj);
   }
 }
 public void Reindex(){
   for(k,v in pairs(this._objects) ){
     if(v.enabled  &&  not v.static  ){ 
       self:CheckIndex(v);
     }
   }
 }
 public void GetNearEntities(vec3, radius){
   var cx = math.floor(vec3.x / 20);
   var cy = math.floor(vec3.z / 20);
   var entities = {}
   for(x = cx - 1, cx + 1 ){
     for(y = cy - 1, cy + 1 ){
       var key = x + y*100000;
       var tbl = this._map[key]  ||  {}
       // iterate over subtable, to find near entities
       for(k, v in pairs(tbl) ){
         if(v:GetSimpleDistanceToVec(vec3) < radius  ){ 
           entities[#entities + 1] = v;
         }
       }
     }
   }
   return entities;
 }
 }}