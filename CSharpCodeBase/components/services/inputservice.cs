using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class inputservice {
 var default = "Default Layer";
 var mouseAndKeyboard = "Keyboard/Mouse";
 public void init(self);
     this.inputLayersStack = Stack();
     this.inputLayersStack:Push(default);
     this.inputDevice = luanet.InControl.InputManager.ActiveDevice;
 }})
 public void IsGamepad(){
   if(this.inputDevice.Name == mouseAndKeyboard  ){ 
     return false;
   }
   return true;
 }
 public void CheckLayer(layer){
   var mLayer = layer  ||  default;
   if(mLayer  &&  mLayer == this.inputLayersStack:Top()  ){ 
     return true;
   }
   return false;
 }
 public void PushFrame(name){
   this.inputLayersStack:Push(name);
 }
 public void PopFrame(){
   if(this.inputLayersStack:Top() != default  ){ 
     this.inputLayersStack:Pop();
   }
 }
 public void GetKey(keycode, layer){
   if(self:CheckLayer(layer)  ){ 
     return Input.GetKey(keycode);
   }
   return false;
 }
 public void GetKeyDown(keycode, layer){
   if(self:CheckLayer(layer)  ){ 
     return Input.GetKeyDown(keycode);
   }
   return false;
 }
 public void GetKeyDownUnlocked(keycode, layer){
   if(self:CheckLayer(layer)  ){ 
     return Input.GetKeyDownUnlocked(keycode);
   }
   return false;
 }
 public void GetMouseButtonDown(keycode, layer){
   if(self:CheckLayer(layer)  ){ 
     return Input.GetMouseButtonDown(keycode);
   }
   return false;
 }
 public void GetMouseButton(keycode, layer){
   if(self:CheckLayer(layer)  ){ 
     return Input.GetMouseButton(keycode);
   }
   return false;
 }
 public void BottomButtonWasPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.Action1.WasPressed;
   }
   return false;
 }
 public void LeftButtonWasPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.Action3.WasPressed;
   }
   return false;
 }
 public void TopButtonWasPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.Action4.WasPressed;
   }
   return false;
 }
 public void RightButtonWasPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.Action2.WasPressed;
   }
   return false;
 }
 public void LeftStickValues(layer){
   var value = Vector3();
   if(self:CheckLayer(layer)  ){ 
     value.x = this.inputDevice.LeftStickX.Value;
     value.z = this.inputDevice.LeftStickY.Value;
   }
   return value;
 }
 // Левый стик был нажат по вертикале
 public void LeftStickYIsPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.LeftStickY.IsPressed;
   }
   return false;
 }
 public void LeftStickXIsPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.LeftStickX.IsPressed;
   }
   return false;
 }
 public void RightTriggerWasPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.RightTrigger.WasPressed;
   }
   return false;
 }
 public void RightTriggerIsPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.RightTrigger.IsPressed;
   }
   return false;
 }
 public void LeftTriggerWasPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.LeftTrigger.WasPressed;
   }
   return false;
 }
 public void LeftTriggerIsPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.LeftTrigger.IsPressed;
   }
   return false;
 }
 public void GetLookValue(layer){
   var value = Vector3();
   if(self:CheckLayer(layer)  ){ 
     value.x = this.inputDevice.RightStickX.Value;
     value.z = this.inputDevice.RightStickY.Value;
   }
   return value;
 }
 public void LookXWasPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.RightStickX.WasPressed;
   }
   return false;
 }
 public void LookYWasPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.RightStickY.WasPressed;
   }
   return false;
 }
 public void RightStickYWasPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.RightStickY.WasPressed;
   }
   return false;
 }
 public void RightStickXWasPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.RightStickX.WasPressed;
   }
   return false;
 }
 public void RightStickButtonWasPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.RightStickButton.WasPressed;
   }
   return false;
 }
 public void LeftStickButtonWasPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.LeftStickButton.WasPressed;
   }
   return false;
 }
 public void LeftBumperWasPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.LeftBumper.WasPressed;
   }
   return false;
 }
 public void LeftBumperIsPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.LeftBumper.IsPressed;
   }
   return false;
 }
 public void RightBumperWasPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.RightBumper.WasPressed;
   }
   return false;
 }
 public void RightBumperIsPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.RightBumper.IsPressed;
   }
   return false;
 }
 public void DPadLeftIsPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.DPadLeft.IsPressed;
   }
   return false;
 }
 public void DPadRightIsPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.DPadRight.IsPressed;
   }
   return false;
 }
 public void DPadUpIsPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.DPadUp.IsPressed;
   }
   return false;
 }
 public void DPadDawnIsPressed(layer){
   if(self:CheckLayer(layer)  ){ 
     return this.inputDevice.DPadDown.IsPressed;
   }
   return false;
 }
 }}