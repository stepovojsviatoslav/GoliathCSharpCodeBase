using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class rigidbodyutils {
 RigidbodyUtils = {}
 var _CreateCharacterRigidbody = luanet.RigidbodyUtils.CreateCharacterRigidbody;
 public void RigidbodyUtils.CreateCharacterRigidbody(t){
   _CreateCharacterRigidbody(t);
 }
 var _Move = luanet.RigidbodyUtils.Move;
 public void RigidbodyUtils.Move(r, vec3){
   _Move(r, vec3.x, vec3.y, vec3.z);
 }
 var _MoveNotRotate = luanet.RigidbodyUtils.MoveNotRotate;
 public void RigidbodyUtils.MoveNotRotate(r, vec3){
   _MoveNotRotate(r, vec3.x, vec3.y, vec3.z);
 }
 var _MoveLookAtMouse = luanet.RigidbodyUtils.MoveLookAtMouse;
 public void RigidbodyUtils.MoveLookAtMouse(r, vec3, h){
   _MoveLookAtMouse(r, vec3.x, vec3.y, vec3.z, h);
 }
 var _AvoidMove = luanet.RigidbodyUtils.AvoidMove;
 public void RigidbodyUtils.AvoidMove(go, vec31, vec32, sphereRadius, forwardDistance, sphericalDistance, offsetAngle){
   var result = {}
   _AvoidMove(go, null, result, sphericalDistance, sphereRadius,;
      offsetAngle, forwardDistance,;
      vec31.x, vec31.y, vec31.z,;
      vec32.x, vec32.y, vec32.z);
   return result;
 }
 var _ResetVelocity = luanet.RigidbodyUtils.ResetVelocity;
 public void RigidbodyUtils.ResetVelocity(r){
   _ResetVelocity(r);
 }
 var _MoveDown = luanet.RigidbodyUtils.MoveDown;
 public void RigidbodyUtils.MoveDown(g, e){
   _MoveDown(g, e);
 }
 var _SetRotation = luanet.RigidbodyUtils.SetRotation;
 public void RigidbodyUtils.SetRotation(r, vec3){
   _SetRotation(r, vec3.x, vec3.y, vec3.z);
 }
 var _LookAt = luanet.RigidbodyUtils.LookAt;
 public void RigidbodyUtils.LookAt(r, vec3){
   _LookAt(r, vec3.x, vec3.y, vec3.z);
 }
 var _ApplyImpulse = luanet.RigidbodyUtils.ApplyImpulse;
 public void RigidbodyUtils.ApplyImpulse(r, vec3){
   _ApplyImpulse(r, vec3.x, vec3.y, vec3.z);
 }
 var _IgnoreCollision = luanet.RigidbodyUtils.IgnoreCollision;
 public void RigidbodyUtils.IgnoreCollision(t1, t2){
   _IgnoreCollision(t1, t2);
 }
 var _RotateGrenadeTrajectory = luanet.RigidbodyUtils.RotateGrenadeTrajectory;
 public void RigidbodyUtils.RotateGrenadeTrajectory(x, y, vec1, vec2){
   var result = {}
   _RotateGrenadeTrajectory(result, x, y, vec1.x, vec1.y, vec1.z, vec2.x, vec2.y, vec2.z);
   return Vector3(result["x"], result["y"], result["z"]);
 }
 RigidbodyUtils.TryGetRadius = luanet.RigidbodyUtils.TryGetRadius;
 RigidbodyUtils.TryGetHeight = luanet.RigidbodyUtils.TryGetHeight;
 RigidbodyUtils.GetCapsuleColliderData = luanet.RigidbodyUtils.GetCapsuleColliderData;
 RigidbodyUtils.Rotate = luanet.RigidbodyUtils.Rotate;
 RigidbodyUtils.MoveTransform = luanet.RigidbodyUtils.MoveTransform;
 RigidbodyUtils.MoveTransformInRadius = luanet.RigidbodyUtils.MoveTransformInRadius;
 RigidbodyUtils.SetPointPositionToLineRenderer = luanet.RigidbodyUtils.SetPointPositionToLineRenderer}}