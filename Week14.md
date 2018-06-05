**_题目要求_**
>> UI 效果制作（你仅需要实现以下效果之一）
>> - 进入 NGUI 官方网站，使用 UGUI 实现以下效果
>> - * Inventory 背包系统
 
 --------
 
 # 构建场景
 - 场景分层
 
 <br>
 
 ![](http://imglf3.nosdn.127.net/img/S3F1ejdrdGNrNFh1blE5WmdVQmRGVkU0Qys4VStxY1pCUFl2ZnlVNmxWa01kNlRydzBFT2F3PT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0)
 <br>
 
 - * UI层
 - - - 详情
>> 该层包含一个`Panel`，该`Panel`挂载一张背景图，并且，其下包含一个 **_Package_** (`Panel`) 及一个 **_Equipment_** (`Panel`)。 `Package` 下包含9个 `Button` 对应9个装备栏。 `Equipment` 下包含3个 `Button` 对应3个穿戴装备（头盔、武器、足具）。<br>
>> 除此之外，该层还包括一个自己写的类 `MouseImage`，用以记录当前鼠标所选择的装备。
 - - - `Package`装备栏
>> 先添加9个 `Button`，分别修改 `Text` 为正确名称。<br>
>> 给 `Package` 添加 `Grid Layout Group`， 易于分层。设置长度，每个`Button` 所占空间、间隔，排列方式等属性，如下图：<br>

<br>

![](http://imglf6.nosdn.127.net/img/S3F1ejdrdGNrNFh1blE5WmdVQmRGUURvOTBLbUsyUGtUZUxicVlRMFYxYVNTZXpwaGFZQXBRPT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0)
<br>

 - - - `Equipment`装备栏
>> 类似于 `Package`，设置参数如下：<br>

<br>

![](http://imglf4.nosdn.127.net/img/S3F1ejdrdGNrNFh1blE5WmdVQmRGYXhRcDdBRGwyMDEwTUh6dGNlZkhodHMxc0loaFQrRjRRPT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0)
<br>

 - - - 效果图
 
 <br>
 
 ![](http://imglf4.nosdn.127.net/img/S3F1ejdrdGNrNFZiRE9yTzhnZXFYTzFFbTMxcExiVWM4RVpZZ1FsUEx6ZGJOQTUvWE5wNjVBPT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0)
 <br>
 
 - - - `UICamera`
 >> 设置参数并将该相机挂在在 `Canvas` 上。
 

 - * 角色层
 - - - 详情
 >> 该层包含一个摄像机和一个玩家，玩家为下载好的模型。需要注意的是，该层的相机渲染应当放在主相机之前，将玩家挂载在该相机下。
 - - - 玩家相机
 >> 设置相机的参数（主要是深度），如下图：
 
 >> 最后得到效果图：
 
 <br>
 
 ![](http://imglf3.nosdn.127.net/img/S3F1ejdrdGNrNFh1blE5WmdVQmRGUTlLeWV3ZHdoTWRjT09qazZ3bG90aWJoV3UxaDNna0x3PT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0)
 <br>
 
 - * 主相机层
 - - - 详情
 >> 该层包含主相机和一个粒子系统。
 - - - 主相机设置
 >> 主相机应当最后渲染，设置参数如下：
 
 <br>
 
 ![](http://imglf5.nosdn.127.net/img/S3F1ejdrdGNrNFh1blE5WmdVQmRGUUhlZFMzSHNYUnp6eTI3MHhJY1RsRHZlY1FYUzNoRlVBPT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0)
 <br>
 
 - 场景效果
 
 <br>
 
 ![](http://imglf4.nosdn.127.net/img/S3F1ejdrdGNrNFZiRE9yTzhnZXFYTzFFbTMxcExiVWM4RVpZZ1FsUEx6ZGJOQTUvWE5wNjVBPT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0)
 <br>
 
---------

 # 代码实现
 - 实现原理
 >> 装备的信息全部存储在每一个 `Button` 里，每次传递数字，根据数字信息来判断该位置的装备。<br>
 >> 在装备栏放置物品，由鼠标点击来进行交互，如下：
  
 <br>
 
 ![](http://imglf3.nosdn.127.net/img/S3F1ejdrdGNrNFZiRE9yTzhnZXFYSG9hakE2ekJlUmM3SFhrMXVNcTBIbktjYVQwSklncW1BPT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0)
 <br>
 
 >> 代码如下：
 
 ```
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
 ```
 
 >> 在装备栏（玩家穿戴装备栏），由鼠标点击来进行交互，如下：

<br>

![](http://imglf5.nosdn.127.net/img/S3F1ejdrdGNrNFZiRE9yTzhnZXFYTGFaNXJRLzN3cnZ5M3dITndFTUVMVlBIaTVzVmIwbUx3PT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0)
<br>
 
 >> 代码如下：
 
 ```
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
 ```
 
 - `EquipmentManager`管理器
 >> 该类为控制器，控制整个场景的元素，调节玩家装备及鼠标信息。<br>
 >> 如下：
 
 ```
        public MouseImage mouseImage;       //鼠标
        private int hat = 0;                //头盔穿戴
        private int hand = 0;               //武器
        private int shoes = 0;              //足具
 ```
 
 - `MouseImage`鼠标信息
 >> 该类包含鼠标信息，里面有当前鼠标选择的装备图片及装备类型。<br>
 >> 如下：
 
 ```
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
 ```
 
 --------
 
 # 源代码挂载及参数设置
 - **_MouseImage_**
 
 ```
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
 ```
 
 >> 挂在UI层的mouseImage上。
 
 - **_EquipmentManager_**
 
 ```
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace backpack
{
    public class EquipmentManager : MonoBehaviour
    {
        public MouseImage mouseImage;       //鼠标
        private int hat = 0;                //头盔穿戴
        private int hand = 0;               //武器
        private int shoes = 0;              //足具

        void Start()
        {
        }

        public MouseImage getMouse()
        {
            return mouseImage;
        }

        public void reset()
        {
            hat = 0;
            hand = 0;
            shoes = 0;
        }

        public int getHat()
        {
            return hat;
        }



        public int getHand()
        {
            return hand;
        }

        public int getShoes()
        {
            return shoes;
        }

        public void setHat(int _hat)
        {
            hat = _hat;
        }


        public void setHand(int _hand)
        {
            hand = _hand;
        }


        public void setShoes(int _shoes)
        {
            shoes = _shoes;
        }
    }
}
 ```
 
 >> 挂载在空对象上，配置成员变量 `mouseImage` <br>
 >> 如图: 
 
 <br>
 
 ![](http://imglf6.nosdn.127.net/img/S3F1ejdrdGNrNFZiRE9yTzhnZXFYSjJwT0hhdDJSS21ueXRMZVJvalhlMmg5SVhnOWhPYkx3PT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0)
 <br>
 
 - **_Package_**
 
 ```
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
 ```
 
 >> 注意，该脚本需要挂载在每一个装备栏下，并且需要配置参数，配置事件，将装备图片配置好，并配置其他参数。将前三个装备栏放上装备。<br>
 >> 如下：
 
 
 <br>
 
 ![](http://imglf4.nosdn.127.net/img/S3F1ejdrdGNrNFh1blE5WmdVQmRGUkNlN05KZGZ0OVdYZkxsZkxZbmVBQ0ZFY205OXFtc3VnPT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0)
 <br>
 
 - **_Equipment_**
 
 ```
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
 ```
 
 >> 同理，同样需要配置，如下：
 
 <br>
 
 ![](http://imglf3.nosdn.127.net/img/S3F1ejdrdGNrNFh1blE5WmdVQmRGVjVxb2xwdlE5eVFkQzJlZERkWUdpN1JhWnl6djVoTmFRPT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0)
 <br>

--------

# 演示视频

[演示视频](http://v.youku.com/v_show/id_XMzY0NzUzNzAyMA==.html)
