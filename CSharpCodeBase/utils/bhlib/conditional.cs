using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class conditional {
 // Conditionals
 public void init(self, func);
     this.func = func;
     this.returnCode = B_FAILURE;
 }})
 public void Behave(creature){
   this.returnCode = this.func(creature)  &&  B_SUCCESS  ||  B_FAILURE;
   return this.returnCode;
 }
 public void Bool(creature){
   return this.func(creature);
 }
 }}