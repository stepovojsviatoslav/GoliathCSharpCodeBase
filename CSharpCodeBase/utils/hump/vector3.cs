using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class vector3 {
 public void init(self, x, y, z);
     this.x, this.y, this.z = x  ||  0, y  ||  0, z  ||  0;
 }})
 public void __add( rhs ){
     if(self == null  ){ 
       print(debug.traceback());
     }
     ( this.x + rhs.x, this.y + rhs.y, this.z + rhs.z);
 }
 public void __sub( rhs ){
     ( this.x - rhs.x, this.y - rhs.y, this.z - rhs.z);
 }
 public void __mul( rhs ){
     ( this.x * rhs, this.y * rhs, this.z * rhs);
 }
 public void __div( rhs ){
     ( this.x / rhs, this.y / rhs, this.z / rhs);
 }
 public void Dot( rhs ){
     return this.x * rhs.x + this.y * rhs.y + this.z * rhs.z;
 }
 public void Cross( rhs ){
     ( this.y * rhs.z - this.z * rhs.y,;
                     this.z * rhs.x - this.x * rhs.z,;
                     this.x * rhs.y - this.y * rhs.x);
 }
 public void __tostring(){
     return string.format("(%2.2f, %2.2f, %2.2f)", this.x, this.y, this.z) ;
 }
 public void __eq( rhs ){
     return this.x == rhs.x  &&  this.y == rhs.y  &&  this.z == rhs.z;
 }
 public void DistSq(other){
     return (this.x - other.x)*(this.x - other.x) + (this.y - other.y)*(this.y - other.y) + (this.z - other.z)*(this.z - other.z);
 }
 public void Dist(other){
     return math.sqrt(self:DistSq(other));
 }
 public void Vector3.Distance(v1, v2){
   return v1:Dist(v2);
 }
 public void LengthSq(){
     return this.x*this.x + this.y*this.y + this.z*this.z;
 }
 public void Length(){
     return math.sqrt(self:LengthSq());
 }
 public void Normalize(){
     var len = self:Length();
     if(len > 0  ){ 
         this.x = this.x / len;
         this.y = this.y / len;
         this.z = this.z / len;
     }
     return self;
 }
 public void RotateAroundY(a){
   var tmpx = this.x;
   var tmpz = this.z;
   this.x = math.cos(a) * tmpx - math.sin(a) * tmpz;
   this.z = math.sin(a) * tmpx + math.cos(a) * tmpz;
 }
 public void GetNormalized(){
     return self / self:Length();
 }
 public void Get(){
     return this.x, this.y, this.z;
 }
 public void Zero(){
   this.x = 0;
   this.y = 0;
   this.z = 0;
 }
 public void Vector3.Lerp(start, final, percent){
   dv = final - start;
   dv = dv * percent;
   return start + dv;
 }
 public void Clone(){
   (this.x, this.y, this.z);
 }
 public void Randomize(x, y, z){
   this.x = this.x + math.random() * x - x/2;
   this.y = this.y + math.random() * y - y/2;
   this.z = this.z + math.random() * z - z/2;
 }
 public void Vector3.GetRandom(ax, ay, az){
   (math.random() * ax - ax/2,;
     math.random() * ay - ay/2,;
     math.random() * az - az/2);
 }
 }}