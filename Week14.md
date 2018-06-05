**_题目要求_**
>> UI 效果制作（你仅需要实现以下效果之一）
>> - 进入 NGUI 官方网站，使用 UGUI 实现以下效果
>> - * Inventory 背包系统
 
 --------
 
 # 构建场景
 - 场景分层
 - * UI层
 - - - 详情
>> 该层包含一个`Panel`，该`Panel`挂载一张背景图，并且，其下包含一个 **_Package_** (`Panel`) 及一个 **_Equipment_** (`Panel`)。 `Package` 下包含9个 `Button` 对应9个装备栏。 `Equipment` 下包含3个 `Button` 对应3个穿戴装备（头盔、武器、足具）。<br>
>> 除此之外，该层还包括一个自己写的类 `MouseImage`，用以记录当前鼠标所选择的装备。
 - - - `Package`装备栏
>> 先添加9个 `Button`，分别修改 `Text` 为正确名称。<br>
>> 给 `Package` 添加 `Grid Layout Group`， 易于分层。设置长度，每个`Button` 所占空间、间隔，排列方式等属性，如下图：<br>

 - - - `Equipment`装备栏
>> 类似于 `Package`，设置参数如下：<br>

 - - - 效果图
 
 - - - `UICamera`
 >> 设置参数并将该相机挂在在 `Canvas` 上。
 

 - * 角色层
 - - - 详情
 >> 该层包含一个摄像机和一个玩家，玩家为下载好的模型。需要注意的是，该层的相机渲染应当放在主相机之前，将玩家挂载在该相机下。
 - - - 玩家相机
 >> 设置相机的参数（主要是深度），如下图：
 
 >> 最后得到效果图：
 
 - * 主相机层
 - - - 详情
 >> 该层包含主相机和一个粒子系统。
 - - - 主相机设置
 >> 主相机应当最后渲染，设置参数如下：
 
 
 - 场景效果
 
 
---------

 # 代码实现
 - 实现原理
 >> 在装备栏放置物品，由鼠标点击来进行交互，如下：
 
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
 
