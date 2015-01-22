using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class action {
 public void init(self, priority, continuous, name);
       this.priority = priority;
       this.continuous = continuous  &&  true  ||  false;
       this.isStarted = false;
       this.name = name;
 }})
 public void OnFetch(...){
   self:init(...);
 }
 public void Free(){
   if(this.__pool != null  ){ 
     this.__pool:Release(self);
   }
 }
 public void IsStarted(){
   return this.isStarted;
 }
 public void OnStart(){
   this.isStarted = true;
   self:OnStartRunning()  ;
 }
 public void OnSuspend(){
   self:OnStopRunning();
 }
 public void OnResume(){
   self:OnStartRunning();
 }
 public void OnComplete(){
   self:OnStopRunning();
 }
 public void FixedUpdate(){
   
 }
 public void OnStopRunning(){
 }
 public void OnStartRunning(){
 }
 public void OnPushed(){
 }
 public void OnRemove(){
   self:OnStopRunning();
 }
 public void IsContinuous(){
   return this.continuous;
 }
 public void GetPriority(){
   return this.priority ;
 }
 public void OnEvent(data){
 }
 }}