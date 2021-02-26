using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class Item : MonoBehaviour
    {
        public int count = 1;

        public void destory()
        {
            this.gameObject.SetActive(false);
        }

        public GameObject getObject()
        {
            return this.gameObject;
        }
    }
}