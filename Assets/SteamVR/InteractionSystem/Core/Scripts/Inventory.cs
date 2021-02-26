using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class Inventory : MonoBehaviour
    {
        public int size = 64;
        public List<GameObject> items;
        public SteamVR_Action_Boolean inventory;
        public float invGuiDist = 2;
        public float InvRotationSpeed = 0.5f;
        public Transform cam;
        public GameObject InventorySlot;
        public GameObject inventoryGuiCanvas;
        public float lookdown;

        private Vector3 invpos = Vector3.zero;

        private void Start()
        {
            items = new List<GameObject>();
            for(int x = 0; x < Mathf.Sqrt(size); x++)
            {
                for (int y = 0; y < Mathf.Sqrt(size); y++)
                {
                    GameObject slot = Instantiate(InventorySlot, new Vector3(0,0,0), Quaternion.identity) as GameObject;
                    slot.transform.SetParent(inventoryGuiCanvas.transform);
                    slot.transform.localPosition = new Vector3(0.875f - (0.25f * x), 0.875f - (0.25f * y), 0);
                }
            }
        }

        private void Update()
        {
            if(inventory.GetStateDown(SteamVR_Input_Sources.Any))
            {
                invpos = transform.position;
                invpos += cam.forward * invGuiDist;
                invpos.y = transform.position.y + 0.8f;
                inventoryGuiCanvas.transform.position = invpos;
            }
            Vector3 lookdir = cam.position;
            lookdir.y -= lookdown;
            Quaternion _lookRotation = Quaternion.LookRotation((lookdir - inventoryGuiCanvas.transform.position).normalized);
            inventoryGuiCanvas.transform.rotation = Quaternion.Slerp(inventoryGuiCanvas.transform.rotation, _lookRotation, Time.deltaTime * InvRotationSpeed);
        }

        public void addItem(Item item)
        {
            items.Add(item.getObject());
        }

        public List<GameObject> getItems()
        {
            return items;
        }
    }
}