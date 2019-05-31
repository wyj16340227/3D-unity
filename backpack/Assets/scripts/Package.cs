using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace backpack
{
    public class Package : MonoBehaviour
    {

        private EquipmentManager equipmentManager;
        private Image packageImage;
        public int mouse_type = 0; // 0->没有装备，1...有不同装备
        public Sprite hair;
        public Sprite weapon;
        public Sprite foot;
        public Sprite UISprite;

        void Awake()
        {
            equipmentManager = (EquipmentManager)FindObjectOfType(typeof(EquipmentManager));
            packageImage = GetComponent<Image>();
        }

        public void On_equip_Button()
        {
            int MouseType = equipmentManager.getMouse().getMouseType(); // 得到鼠标目前的mousetype
            if (packageImage.sprite != UISprite && MouseType == 0) // 若鼠标没有图片在上面，并且bag的image不为空有装备，则取走bag_image的装备
            {
                Debug.Log(mouse_type);
                packageImage.sprite = UISprite;
                equipmentManager.getMouse().setMouseType(mouse_type); // 将当前装备的type给鼠标
                mouse_type = 0; // 此背包的mousetype变为0，则当前背包啥都没有
            }
            else
            {   // 若鼠标上有装备，设置装备
                if (MouseType == 1) packageImage.sprite = hair;
                else if (MouseType == 2) packageImage.sprite = weapon;
                else if (MouseType == 3) packageImage.sprite = foot;
                mouse_type = MouseType; // mousetype变为鼠标的mousetype
                equipmentManager.getMouse().setMouseType(0); // 鼠标装备消失
            }
        }
    }
}
