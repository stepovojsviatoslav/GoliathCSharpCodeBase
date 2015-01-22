using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;

namespace MainGame
{
    public class objectpool
    {
        public object _class;

        public void objectpool(object class_name)
        {
            this._class = class_name;
            this._poolLength = 1;
            this._curPoint = 1;
            this._pool = new object[_poolLength];
            this.Init();
        }
        public void Init()
        {
            for (int i = 0; i < this._poolLength; i++)
            {
                if (this._pool[i] == null)
                {
                    //this._pool[i] = this._class();
                    //this._pool[i].__pool = self;
                }
            }
        }

        public void Fetch(params object param)
        {
            if (this._curPoint > this._poolLength)
            {
                this._poolLength = this._poolLength + 1;
                this.Init();
            }
            else
            {
                var object_s = this._pool[this._curPoint];
                this._curPoint = this._curPoint + 1;
                //if(object_s.OnFetch  ){  object_s.OnFetch(); }
                //return object;
            }
        }

        public int _poolLength { get; set; }
        public int _curPoint { get; set; }
        public object[] _pool;

        public void Release(object objects)
        {
            this._curPoint = this._curPoint - 1;
            this._pool[this._curPoint] = objects;
            //if(object.OnRelease  ){  object:OnRelease() }
        }
    }
}