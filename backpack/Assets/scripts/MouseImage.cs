using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace backpack
{
    public class MouseImage : MonoBehaviour
    {

        private EquipmentManager equipmentManager;
        private Image mouseImage;
        private int mouseType = 0;

        public Sprite none;
        public Sprite hat;
        public Sprite hand;
        public Sprite shoes;
        public Color None;
        public Color NotNone;
        public Camera cam;

        void Awake()
        {
            equipmentManager = (EquipmentManager)FindObjectOfType(typeof(EquipmentManager));
            //equipmentManeger.setMouse(this);
            mouseImage = GetComponent<Image>();
        }

        public int getMouseType()
        {
            return mouseType;
        }


        public void setMouseType(int _mouseType)
        {
            mouseType = _mouseType;
        }

        void Update()
        {
            Debug.Log(mouseType);
            if (mouseType == 0)
            {
                mouseImage.sprite = none;
                mouseImage.color = None;
            }
            else if (mouseType == 1)
            {
                mouseImage.color = new Color(1, 1, 1, 1);
                mouseImage.sprite = hat;
            }
            else if (mouseType == 2)
            {
                mouseImage.color = new Color(1, 1, 1, 1);
                mouseImage.sprite = hand;
            }
            else if (mouseType == 3)
            {
                mouseImage.color = new Color(1, 1, 1, 1);
                mouseImage.sprite = shoes;
            }
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        }
    }
}
