using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using MainGame.core;
//using UnityEngine.EventSystems;
 
namespace MainGame
{
    public class Player : UnityEntity
    {   
        public Vector3 homePosition;

        new public void Awake()
        {
            isStatic = false;
            // FiX ME            
            gameObject.AddComponent<PlayerController>();
            //gameObject.AddComponent<GuiControl>();
        }
        
        public void StoreHomePosition(Vector3 position)
        {
            homePosition = position;
        }         
    }
}