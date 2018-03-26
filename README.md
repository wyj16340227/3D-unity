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
>*  常用事件包括 OnGUI() OnDisable() OnEnable()
