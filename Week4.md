# 操作与总结
>* 参考 Fantasy Skybox FREE 构建自己的游戏场景
>> 我所涉及的游戏场面主要有两个部分，第一个为天空盒`sky box`，另一个为游戏对象渲染。
>>* 天空盒
>>
>> 天空盒使用了师兄在某社交群上上传的`sky box`文件包，选择了天空盒`Think Cloud Water`，主要考虑的因素为这次试验做的是`牧师与魔鬼`过河，所以选择了有海水的场景。将文件夹导入`unity`。<br>
>>
>> 天空盒中有6张贴图。对应6个面。首先将`main camera`的`Clear flag`属性设置为`sky box`，并添加'Component'->`Randering`->'skybox'。<br>
>>
>> 创建一个新的`material`，选择'sky box'->`six sides`，按照提示顺序将6张图片拖入到`material`中。创建完成后将`material`拖入到`main camera`的`sky box`属性中。<br>
>>
![skyBox](http://imglf6.nosdn.127.net/img/S3F1ejdrdGNrNFVwaVBPZEJwSHVnakZsUWwrYldidFQ2SXpXMGlkOHJvS3ZiSk12UmtBYmNnPT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0 "skyBox")<br>
![mainCamera](http://imglf5.nosdn.127.net/img/S3F1ejdrdGNrNFVwaVBPZEJwSHVnbU92TlFoWkh4dC95K0JnTk9rcWFZeGpEUkVnZUZTVFN3PT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0 "mainCamera")<br>
>>* 游戏对象<br>
>>
>> 在[unity asset store](https://assetstore.unity.com/)上下载石头，恶魔，牧师，船只，树木等游戏对象，替换之前的游戏对象。<br>
![替换对象](http://imglf4.nosdn.127.net/img/S3F1ejdrdGNrNFVwaVBPZEJwSHVnalVDVThQVDBUdFZoNDUvT1B5SjViZkRxcVlQc05MMEx3PT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0 "替换对象")<br>
>* 写一个简单的总结，总结游戏对象的使用
>>* 1.游戏对象
>>
>> 游戏对象是在游戏中创建的一个物体，从场景中的角色，环境，甚至背景音乐，照相机，都是游戏对象（当然，还有特殊的空对象）。<br>
>>
>> 游戏对象被添加到游戏中时本身并不包含任何特性（特征，属性..）。当然，它们包含有已经被实现的控制组件。例如，一个灯光（Light）就是一个添加到游戏对象的组件。我们可以给游戏对象添加属性来控制游戏对象的运动。游戏对象是一个组建容器。<br>
>>
>>* 2.游戏对象所具有的普遍属性
>>
>> `Active` 不活跃则不会执行`update()`以及`rendering`，控制着游戏对象的状态。<br>
>>
>> `Name` 对象的名字，可以通过名字来查找对象。<br>
>>
>> `Tag` 字串，对象的标签，可以通过`Tag`来标记、识别对象。<br>
>>
>> `Layer` 分组对象，常用于摄像机渲染。<br>
>>
>>* 3.常见对象与渲染<br>
>>
>> `Camera` `SkyBoxes` `3D物体` `地形构造系统` `音频` `Asset` `Empty` `Light` `UI` `Partical System` `Materials`<br>
>>
# 编程实践
>* 牧师与魔鬼 动作分离版
>> 1.构造结构<br>
>> 使用三个主要类，分别为`Data`(数据)`Action`(动作)`Controller`(用户交互及控制管理动作)<br>
>>
>> 2.`Data`<br>
>> 在原先的基础上，将所有关于`GameObject`的`Transforme`改变抽离出来，交由`Controller`控制`Action`完成。在`Data`类中，只实现各个对象状态的改变，而不进行运动。<br>
>> 在原有基础上，增加了一个`moving`变量，用来存储正在移动的物体。<br>
>>
```
private GameObject boatObject;                                              //船
        private Stack<GameObject> leftShoreDemon = new Stack<GameObject>();         //左岸魔鬼
        private Stack<GameObject> leftShorePriest = new Stack<GameObject>();        //左岸牧师
        private Stack<GameObject> rightShoreDemon = new Stack<GameObject>();        //右岸魔鬼
        private Stack<GameObject> rightShorePriest = new Stack<GameObject>();       //右岸牧师
        private Stack<GameObject> boat = new Stack<GameObject>();                   //船上数量
        private Stack<GameObject> moving = new Stack<GameObject>();                 //移动队列
        private bool boatPosition = true;                                           //船在右边
        private bool isMoving = false;                                              //船在移动
        Vector3 boatStartPosition = new Vector3(4f, -3, -3);                        //以下为各个参考位置
        Vector3 boatEndPosition = new Vector3(-4f, -3, -3);
        Vector3 leftShore = new Vector3(-15, -2, -3);
        Vector3 rightShore = new Vector3(15, -2, -3);
        Vector3 boatCurrentPosition;
        Vector3 leftDemonPosition = new Vector3(-8f, -1.5f, -3);
        Vector3 leftPriestPosition = new Vector3(-12.5f, -1.5f, -3);
        Vector3 rightDemonPosition = new Vector3(8f, -1.5f, -3);
        Vector3 rightPriestPosition = new Vector3(12.5f, -1.5f, -3);
```
