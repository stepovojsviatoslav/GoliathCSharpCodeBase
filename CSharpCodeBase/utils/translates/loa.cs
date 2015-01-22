using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class loa {
 LOA = {}
 _ProcessInput = luanet.LOA.ProcessInput;
 public void LOA.ProcessInput(position, vec3, radius){
   corrected = Vector3();
   _ProcessInput(position.x, position.y, position.z, vec3.x, vec3.y, vec3.z, radius, radius * 2, corrected);
   corrected:Normalize();
   return corrected;
 }}}