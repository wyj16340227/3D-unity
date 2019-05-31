using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace backpack
{
    public class Equipment : MonoBehaviour
    {

        // Use this for initialization
        private EquipmentManager equipmentManager;
        private Image equipImage;
        public int mouse_type;
        public Sprite weapon; // 为此背包中相应的装备
        public Sprite UISprite;

        void Awake()
        {
            equipmentManager = (EquipmentManager)FindObjectOfType(typeof(EquipmentManager));
            equipImage = GetComponent<Image>();
        }

        public void On_equip_Button()
        {
            int MouseType = equipmentManager.getMouse().getMouseType(); // 得到鼠标上的mousetype
            if (equipImage.sprite == weapon && MouseType == 0) // 取走装备区装备，当装备区含有装备并且mousetype=0鼠标上没有装备
            {
                equipImage.sprite = UISprite;
                equipmentManager.getMouse().setMouseType(mouse_type);
            }
            else
            {
                if (mouse_type == MouseType && equipImage.sprite != weapon)
                {
                    // 将装备佩戴到装备区中
                    equipImage.sprite = weapon;
                    mouse_type = MouseType;
                    equipmentManager.getMouse().setMouseType(0);
                }
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // 防止重复装备
            if (mouse_type == 1 && equipmentManager.getHat() == 1)
            {
                equipmentManager.setHat(0); //已装备
                equipImage.sprite = weapon;
            }
            else if (mouse_type == 2 && equipmentManager.getHand() == 1)
            {
                equipmentManager.setHand(0);
                equipImage.sprite = weapon;
            }
            else if (mouse_type == 3 && equipmentManager.getShoes() == 1)
            {
                equipmentManager.setShoes(0);
                equipImage.sprite = weapon;
            }
        }
    }
}
