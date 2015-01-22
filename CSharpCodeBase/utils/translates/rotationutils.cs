using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class rotationutils {
 RotationUtils = {}
 var _LookRotation = luanet.RotationUtils.LookRotation;
 public void RotationUtils.LookRotation(vec3){
   var result = {}
   _LookRotation(vec3.x, vec3.y, vec3.z, result);
   return Vector3(result.x, result.y, result.z);
 }}}