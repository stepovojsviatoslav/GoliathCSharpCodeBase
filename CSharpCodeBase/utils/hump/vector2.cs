using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class vector2 {
 public void init(self, x, y);
     this.x, this.y = x  ||  0, y  ||  0;
 }})
 public void __add( rhs ){
     ( this.x + rhs.x, this.y + rhs.y );
 }
 public void __sub( rhs ){
     ( this.x - rhs.x, this.y - rhs.y );
 }
 public void __mul( rhs ){
     ( this.x * rhs, this.y * rhs );
 }
 public void __div( rhs ){
     ( this.x / rhs, this.y / rhs );
 }
 public void Dot( rhs ){
     return this.x * rhs.x + this.y * rhs.y;
 }
 public void Cross( rhs ){
     ( this.x * rhs.y, -this.y * rhs.x );
 }
 public void __tostring(){
     return string.format("(%2.2f, %2.2f)", this.x, this.y);
 }
 public void __eq( rhs ){
     return this.x == rhs.x  &&  this.y == rhs.y;
 }
 public void DistSq(other){
     return (this.x - other.x)*(this.x - other.x) + (this.y - other.y)*(this.y - other.y);
 }
 public void Dist(other){
     return math.sqrt(self:DistSq(other));
 }
 public void LengthSq(){
     return this.x*this.x + this.y*this.y;
 }
 public void Length(){
     return math.sqrt(self:LengthSq());
 }
 public void Normalize(){
     var len = self:Length();
     if(len > 0  ){ 
         this.x = this.x / len;
         this.y = this.y / len;
     }
     return self;
 }
 public void GetNormalized(){
     return self / self:Length();
 }
 public void Get(){
     return this.x, this.y;
 }
 public void Vector2.Distance(v1, v2){
   return v1:Dist(v2);
 }
 }}