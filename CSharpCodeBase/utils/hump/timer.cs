using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class timer {
 public void init(self, timeout);
       this.timeout = timeout;
       this._original_timeout = timeout;
 }})
 public void Tick(){
   this.timeout = this.timeout - GameController.deltaTime;
   return this.timeout < 0;
 }
 public void Reset(){
   this.timeout = this._original_timeout;
 }
 }}