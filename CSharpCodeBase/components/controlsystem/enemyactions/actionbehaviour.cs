using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class actionbehaviour :action{
 public void init (self, entity, tree);
       Action.init(self, 0, true, "behaviour");
       this.entity = entity;
       this.tree = tree;
 }})
 public void Update(){
   this.tree:Process();
   return false;
 }
 public void OnStopRunning(){
   this.tree:Reset();
 }
 public void OnEvent(data){
   this.tree:OnEvent(data);
 }
 }}