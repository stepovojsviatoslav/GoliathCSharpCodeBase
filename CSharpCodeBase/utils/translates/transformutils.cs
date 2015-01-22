using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class transformutils {
 Transform = {}
 var _GetPosition = luanet.TransformUtils.GetPosition;
 public void Transform.GetPosition(transform){
   var result = {}
   _GetPosition(transform, result);
   return Vector3(result.x, result.y, result.z);
 }
 var _SetPosition = luanet.TransformUtils.SetPosition;
 public void Transform.SetPosition(transform, vec3){
   _SetPosition(transform, vec3.x, vec3.y, vec3.z);
 }
 var _SetScale = luanet.TransformUtils.SetScale;
 public void Transform.SetScale(transform, vec3){
   _SetScale(transform, vec3.x, vec3.y, vec3.z);
 }
 var _GetRotation = luanet.TransformUtils.GetRotation;
 public void Transform.GetRotation(transform){
   var result = {}
   _GetRotation(transform, result);
   return Vector3(result.x, result.y, result.z);
 }
 var _SetRotation = luanet.TransformUtils.SetRotation;
 public void Transform.SetRotation(transform, vec3){
   _SetRotation(transform, vec3.x, vec3.y, vec3.z);
 }
 var _GetForwardVector = luanet.TransformUtils.GetForwardVector;
 public void Transform.GetForwardVector(transform){
   var result = {}
   _GetForwardVector(transform, result);
   return Vector3(result.x, result.y, result.z);
 }
 var _TransformPoint = luanet.TransformUtils.TransformPoint;
 public void Transform.TransformPoint(transform, vec3){
   var result = {}
   _TransformPoint(transform, vec3.x, vec3.y, vec3.z, result);
   var vec = Vector3(result.x, result.y, result.z);
   return vec;
 }
 Transform.GetMapper = luanet.TransformUtils.GetMapper;
 Transform.AddComponent = luanet.TransformUtils.AddComponent;
 Transform.SetRendererState = luanet.TransformUtils.SetRendererState;
 Transform.SetColliderState = luanet.TransformUtils.SetColliderState;
 Transform.SetObjectState = luanet.TransformUtils.SetObjectState;
 Transform.SetMaterialColor = luanet.TransformUtils.SetMaterialColor;
 Transform.Lerp = luanet.TransformUtils.Lerp;
 Transform.GetRigidbody = luanet.TransformUtils.GetRigidbody}}