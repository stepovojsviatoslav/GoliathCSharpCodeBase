using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using MainGame.core;
//using UnityEngine.EventSystems;
 
namespace MainGame{
    public class Player : UnityEntity
    {
        Player(GameObject gameObject)
        {
            base(gameObject);
            bool static = false;
        }
        //public void init (self, gameObject);
        //      UnityExistsEntity.init(self, gameObject);
        //      self:AddComponent(PlayerController);
        //      self:AddComponent(GuiControl);                  
        //}})
        public void StoreHomePosition(Vector3 position)
        {
            homePosition = position;
        }         
    }
}