using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class config {
 public void init(self, table, section);
       this.table = table;
       this.section = section;
 }})
 public void Get(path){
   return GameController.database:Get(this.table, this.section .. "/" .. path);
 }
 }}