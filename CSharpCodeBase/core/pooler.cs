using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace MainGame
{
    public class pooler
    {

        public Dictionary<object, objectpool> _pools;
        public void init()
        {
            this._pools = new Dictionary<object, objectpool>();
        }
        public void CreatePool(object class_name)
        {
            if (this._pools[class_name] == null)
            {
                this._pools[class_name] = new objectpool();//class_name);/?
            }
        }
        public void Fetch(object class_name, params object param)
        {
            this._pools[class_name].Fetch(param);
        }
        public void Release(object obj)
        {
            //object.__pool.Release(obj);/?
        }
    }
}