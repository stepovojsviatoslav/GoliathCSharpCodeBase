using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class math {
 public void math.clamp(low, n, high) .min(math.max(n, low), high) }{
 public void math.lerp(a, b, k) {
   var result = a * (1-k) + b * k ;
   if(a < b  &&  result > b  ){ 
     result = b;
   }else{if a > b  &&  result > a  ){ 
     result = a;
   }
   return result;
 }
 public void math.chance(percent) {
   .random() < (percent/100);
 }
 public void math.sectomin(sec){
   return sec * 60;
 }
 public void math.sign(x){
    if(x<0  ){ 
      return -1;
    }else{if x>0  ){ 
      return 1;
    }else{
      return 0;
    }
 }}}