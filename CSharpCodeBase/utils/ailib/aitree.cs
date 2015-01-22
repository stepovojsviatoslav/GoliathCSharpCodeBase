using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class aitree {
 public void init (self, entity, root);
   this.entity = entity;
   this.root = root;
 }})
 public void Process(){
   this.root:Visit();
   this.root:Save();
   this.root:Process();
 }
 public void Reset(){
   this.root:Reset();
 }
 public void OnEvent(data){
   this.root:OnEvent(data);
 }
 }}