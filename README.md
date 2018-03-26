# 简答题
>解释 游戏对象（GameObjects） 和 资源（Assets）的区别与联系。
>>1.游戏对象是游戏场景中的实例，是可以创建的物体，类似于C语言中的类创建的实例，是一个容器。游戏中的人物、场景等都是游戏对象。<br>
>>2.资源是游戏对象的组件，是某种特性，可以是行为、效果等等可以赋给游戏对象的特性。如赋给游戏对象的效果，或者是写一个位移的C#文件，都是资源。<br>
>>3.游戏对象是资源的集合，是可以将资源体现出来的实例。
>
>下载几个游戏案例，分别总结资源、对象组织的结构（指资源的目录组织结构与游戏对象树的层次结构）。
>>根据某站[教学视频](https://www.bilibili.com/video/av19327181?from=search&seid=16408126898703037567)中的教学案例,可以发现，资源组织目录一般会在asset文件夹下建立子文件夹将各种类别的资源区分开来，一般会分成：scripts、sceans、materials，而游戏对象则是以虚拟树，例如相机、敌人等父节点，下面再以各个游戏对象的分类将其作为子节点归属在相应父节点下。
>
>编写一个代码，使用 debug 语句来验证 MonoBehaviour 基本行为或事件触发的条件。
>*   基本行为包括 Awake() Start() Update() FixedUpdate() LateUpdate()
>>编写代码如下：
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        print("Start!");
	}
	
	// Update is called once per frame
	void Update () {
        print("Update!");
	}

    private void Awake()
    {
        print("Awake!");
    }

    private void FixedUpdate()
    {
        print("FixedUpdate!");
    }

    private void LateUpdate()
    {
        print("LateUpdate!");
    }
}
```
>>得到的结果如下：
![验证](https://github.com/wyj16340227/3D-unity/blob/master/%E9%AA%8C%E8%AF%81.png "验证")
>>发现，Awake与Start只出现了一次，但三个Update出现了几百次且不断上升。
>*  常用事件包括 OnGUI() OnDisable() OnEnable()
>>编写代码如下：
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    private void OnDisable()
    {
        print("OnDisable!");
    }

    private void OnGUI()
    {
        print("OnGUI!");
    }

    private void OnEnable()
    {
        print("OnEnable!");
    }
}
```
>>得到结果如下：
![验证2](https://github.com/wyj16340227/3D-unity/blob/master/%E9%AA%8C%E8%AF%812.png "验证2")
>>由结果可以发现：最先调用`OnEnable()`函数一次，之后不停调用`OnGUI()`函数，当结束时，调用`OnDisable()`函数一次。
>
>查找脚本手册，了解 GameObject，Transform，Component 对象
>* 分别翻译官方对三个对象的描述（Description）
>>1.Transform:场景中的每个对象都有一个Transform。 它用于存储和操纵对象的位置，旋转和缩放。 每个Transform都可以有一个父元素，它允许您分层应用位置，旋转和缩放。 这是在“层次结构”窗格中显示的层次结构。 他们还支持枚举类，因此您可以使用以下方式循环访问子代。<br>
>>2.GameObject:Unity场景中所有实体的基类。<br>
>>注意：GameObject类中的许多变量已被删除。 例如，要访问csharp中的GameObject.renderer，请改用GetComponent <Renderer>（）。 在JS脚本中使用GetComponent。<Renderer>（）。<br>
>>3.Component:所有附加到GameObjects的基类。<br>
>>请注意，您的代码永远不会直接创建组件。 而是编写脚本代码，并将脚本附加到GameObject。<br>
>
>* 描述下图中 table 对象（实体）的属性、table 的 Transform 的属性、 table 的部件
>* * 本题目要求是把可视化图形编程界面与 Unity API 对应起来，当你在 Inspector 面板上每一个内容，应该知道对应 API。
>* * 例如：table 的对象是 GameObject，第一个选择框是 activeSelf 属性。
>>1.table的属性：Untagged。<br>
>>2.table的transform的属性：位置(0, 0, 0); 旋转(0, 0, 0); 大小(1, 1, 1)。<br>
>>3.table的部件：Transform, Cube, Box Collider, Mesh Renderer。<br>
>
>* 用 UML 图描述 三者的关系（请使用 UMLet 14.1.1 stand-alone版本出图）
![作业2-1](https://github.com/wyj16340227/3D-unity/blob/master/%E4%BD%9C%E4%B8%9A2-1.png "作业2-1")
>>构图如下：
![作业2-1](https://github.com/wyj16340227/3D-unity/blob/master/%E4%BD%9C%E4%B8%9A2-1.png "作业2-1")
>
>整理相关学习资料，编写简单代码验证以下技术的实现：
>* 查找对象
>>编写代码如下：
```

```
>* 添加子对象
>>编写代码如下：

>* 遍历对象树
>>编写代码如下：

>* 清除所有子对象
>>编写代码如下：
