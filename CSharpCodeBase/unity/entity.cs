using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class entity {
 public void init (self);
       this.components = {}
       this._awakened = false;
 }})
 public void AddComponent(componentType){
   this.components[componentType] = componentType(self);
   if(this._awakened  ){  this.components[componentType]:Awake() }
   return this.components[componentType];
 }
 public void GetComponent(componentType){
   return this.components[componentType];
 }
 public void Awake(){
   for(k, v in pairs(this.components) ){ v:Awake() }
   this._awakened = true;
 }
 public void Update() {
   for(k, v in pairs(this.components) ){ v:Update() }
 }
 public void FixedUpdate(){
   for(k, v in pairs(this.components) ){ v:FixedUpdate() }
 }
 public void LateUpdate() {
   for(k, v in pairs(this.components) ){ v:LateUpdate() }
 }
 public void GetType(){
   return "Entity";
 }
 public void Message(funcname, data){
   for(k, v in pairs(this.components) ){
     func = v[funcname];
     if(func != null  ){ 
       func(v, data);
     }
   }
 }
 public void Enable(){
   this.enabled = true;
 }
 public void Disable(){
   this.enabled = false;
 }
 public void Callback(method, ...)  {
   if(self[method] != null  ){       
     var result = self[method](self, ...);
     return result;
   }else{
     return {}  
   }
 }
 }}