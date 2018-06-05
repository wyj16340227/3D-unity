# **_题目要求_**
## UI 效果制作（你仅需要实现以下效果之一）
 - 进入 NGUI 官方网站，使用 UGUI 实现以下效果
 - * Inventory 背包系统
 
 --------
 
 # 题目实现
 ## 构建场景
 - 场景分层
 - * UI层
 - - - 详情
>> 该层包含一个`Panel`，该`Panel`下包含一个 **_Package_** (`Panel`) 及一个 **_Equipment_** (`Panel`)。 `Package` 下包含9个 `Button` 对应9个装备栏。 `Equipment` 下包含3个 `Button` 对应3个穿戴装备（头盔、武器、足具）。
>> 除此之外，该层还包括一个自己写的类 `MouseImage`，用以记录当前鼠标所选择的装备。
 - - - `Package`装备栏
>> 先添加9个 `Button`，分别修改 `Text` 为正确名称。
>> 
 - * 角色层
 - * 主相机层
