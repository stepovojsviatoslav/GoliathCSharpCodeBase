using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class invertor {
 var Inverter = Class({init = function(self, cond)
       this.cond = cond;
       this.returnCode = B_FAILURE;
 }})
 public void Inverter:Behave(creature){
   result = this.cond:Behave(creature) == B_SUCCESS  &&  B_FAILURE  ||  B_SUCCESS;
   this.returnCode = result;
   return this.returnCode;
 }
 return Inverter}}