using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using MainGame.core;


namespace MainGame {
    public class CharacterDamageReceiver : MonoBehaviour {
 public void OnApplyDamage(List<object> damageData){
          //if(this.entity.WakeUp  ){ 
          //  this.entity:WakeUp();
         // }
          var isDeath = GetComponent<Health>().Decrease((float)damageData[0]) == 0;
   
          //if(!isDeath){ 
            //if(this.entity.relationship != null  ){ 
            //  this.entity.relationship:AddInstance("enemy", damageData.source);
            //}
            /*if(damageData.effects.punch == "push"  ){ 
              print("Pushed!");
              if(this.entity.Pushed  ){ 
                this.entity:Pushed(damageData)//.source:GetPosition())
              }
            }else{
              if(this.entity.Hit  ){ 
                this.entity:Hit(damageData);
              }
            }
          }else{
            // death
            if(this.entity.Death  ){ 
              this.entity:Death();
            }
            //print("Enemy is death!")*/
          }
        }
    }
}