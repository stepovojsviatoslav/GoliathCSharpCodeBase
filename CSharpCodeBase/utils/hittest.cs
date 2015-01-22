using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class hittest {
 var HitTest = {}
 HitTest.IsPointHitted = function(position, forward, targetPosition, sectorAngle);
   var targetRelative = targetPosition - position;
   var ){tProduct = forward:Dot(targetRelative)
   var angle = math.acos(dotProduct/targetRelative:Length() * forward:Length());
   var degrees = angle * (180/math.pi);
   return degrees <= sectorAngle  &&  degrees >= -sectorAngle;
 }
 HitTest.CheckHitEntity = public void (sourceEntity, sourceForward, targetEntity, sectorAngle, sectorDistance, countSteps){
   // check distance
   sectorDistance = sectorDistance  ||  -1;
   if(sectorDistance > -1  &&  sectorDistance < sourceEntity:GetEffectiveDistance(targetEntity)  ){ 
     return false;
   }
   var sourcePosition = sourceEntity:GetPosition();
   var targetPosition = targetEntity:GetPosition();
   var targetRelative = targetPosition - sourcePosition;
   //local sourceForward = Transform.GetForwardVector(sourceEntity.transform)
   var checkPoints = {targetPosition}
   var count = countSteps  ||  10;
   var step = 360 / count;
   for(i = 1, 360, step ){
     var vec3 = Vector3(targetEntity.radius, 0, 0);
     vec3:RotateAroundY(i);
     checkPoints[#checkPoints + 1] = vec3 + targetPosition;
   }
   for(k, v in pairs(checkPoints) ){
     if(HitTest.IsPointHitted(sourcePosition, sourceForward, v, sectorAngle)  ){ 
       return true;
     }
   }
   return false;
 }
 }}