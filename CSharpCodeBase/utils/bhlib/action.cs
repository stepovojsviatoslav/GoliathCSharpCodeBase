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
 // Status codes
 B_FAILED = 0;
 B_FAILURE = B_FAILED;
 B_SUCCESS = 1;
 B_RUNNING = 2;
 // Actions
 public void init(self, func);
     this.func = func;
     this.returnCode = B_FAILURE;
 }})
 public void Behave(creature){
   this.returnCode = this.func(creature);
   return this.returnCode;
 }
 }}