using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class d01 {
 return public void (ds){
   ds:Say("Hey, ){ you have a message for(me?")
   ds:Answer("Yes");
   ds:Answer("No");
   var result = coroutine.yield();
   if(result == 0  ){ 
     ds:Say("Ok, got it to me!");
   }else{
     ds:Say("Good luck buddy!");
     ds:Answer("Ok");
     coroutine.yield();
     return;
   }
   ds:Answer("Ok, take it!");
   coroutine.yield();
 }}}