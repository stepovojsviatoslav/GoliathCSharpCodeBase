using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class Input {
 KeyCode = UnityEngine.KeyCode;
 Input = {}
 Input.enabled = true;
 var _RaycastMouseOnTerrain = luanet.InputUtils.RaycastMouseOnTerrain;
 public void Input.RaycastMouseOnTerrain(){
   var result = {}
   _RaycastMouseOnTerrain(result);
   return Vector3(result.x, result.y, result.z);
 }
 var _RaycastTerrain = luanet.InputUtils.RaycastTerrain;
 public void Input.RaycastTerrain(position){
   var result = {}
   _RaycastTerrain(result, position.x, position.y, position.z);
   return Vector3(result.x, result.y, result.z);
 }
 var _RaycastMouseOnTerrainWithHeight = luanet.InputUtils.RaycastMouseOnTerrainWithHeight;
 public void Input.RaycastMouseOnTerrainWithHeight(height){
   var result = {}
   _RaycastMouseOnTerrainWithHeight(result, height);
   return Vector3(result.x, result.y, result.z);
 }
 var _RaycastTargetEntity = luanet.InputUtils.RaycastTargetEntity;
 public void Input.RaycastTargetEntity(){
   return _RaycastTargetEntity();
 }
 var _RaycastTargetEntityWithRadius = luanet.InputUtils.RaycastTargetEntityWithRadius;
 Input.RaycastTargetEntityWithRadius = _RaycastTargetEntityWithRadius;
 var _GetAxis = UnityEngine.Input.GetAxis;
 public void Input.GetAxis(name){
   return _GetAxis(name);
 }
 var _GetKey = UnityEngine.Input.GetKey;
 public void Input.GetKey(keycode){
   .enabled  &&  _GetKey(keycode);
 }
 public void Input.GetKeyDown(keycode){
   .enabled  &&  UnityEngine.Input.GetKeyDown(keycode);
 }
 var _GetKeyDown = UnityEngine.Input.GetKeyDown;
 public void Input.GetKeyDownUnlocked(keycode){
   return _GetKeyDown(keycode);
 }
 var _IsPointerOverUI = luanet.InputUtils.IsPointerOverUI;
 var _GetMouseButtonDown = UnityEngine.Input.GetMouseButtonDown;
 public void Input.GetMouseButtonDown(keycode){
   return _GetMouseButtonDown(keycode)  &&  not _IsPointerOverUI() ;
 }
 public void Input.IsPointerOverUI(){
   return _IsPointerOverUI();
 }
 var _GetMouseButton = UnityEngine.Input.GetMouseButton;
 public void Input.GetMouseButton(keycode){
   return _GetMouseButton(keycode)  &&  not _IsPointerOverUI();
 }
 public void Input.GetMouseScrollValue(){
   var value = Input.GetAxis("Mouse ScrollWheel");
   if(value != 0  &&  _IsPointerOverUI()  ){ 
     return 0;
   }
   return value;
 }
 var _GetTargetEntityForAction = luanet.InputUtils.GetTargetEntityForAction;
 public void Input.GetTargetEntityForAction(vec3, radius){
   return _GetTargetEntityForAction(vec3.x, vec3.y, vec3.z, radius);
 }}}