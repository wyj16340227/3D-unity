# 简答并用程序验证
>* 游戏对象运动的本质是什么？
>> 游戏对象运动的本质是游戏对象方向、位置的不断变换。
>* 请用三种方法以上方法，实现物体的抛物线运动。（如，修改变换属性，使用向量的Vector3的方法...）
>> 1.由`(0, 0, 0)`出发，设置抛物线为`Y = X^2`，通过改变`position`实现，代码如下：
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour {

    private float speed;	//速度
    private float position;	//位置

	// Use this for initialization
	void Start () {
            speed = 1f;
            position = 0;
            transform.position = new Vector3(0, 0, 0);
        }
	
	// Update is called once per frame
	void Update () {
            position += speed * Time.deltaTime;
            transform.position = new Vector3(position, position * position, 0);
	}
}
```
>> 2.通过`transform.Translate()`函数实现，其他相同，计算出增量为`2X`，代码如下：
```
	void Update () {
            position += speed * 2 * Time.deltaTime;
            transform.Translate(Vector3.up * position * Time.deltaTime);
            transform.Translate(Vector3.right * Time.deltaTime);
	}
```
>> 3.通过增加`tranform.position`实现，代码如下：
```
        void Update () {
            position += speed * 2 * Time.deltaTime;
            transform.position += new Vector3(Time.deltaTime, position * Time.deltaTime, 0);
	}
```
>* 写一个程序，实现一个完整的太阳系，其他星球围绕太阳的转速必须不一样，且不在一个法平面上。
>> 1.在网上查阅资料可知，8大行星和太阳之间的距离比例为（以地球为1.0，距离太阳1496000000千米）；<br>
水星Mercury 0.387<br>
金星Venus 0.723<br>
地球Earth 1.000<br>
火星Mars 1.524<br>
木星Jupiter 5.205<br>
土星Saturn 9.576<br>
天王星Uranus 19.18<br>
海王星Neptune 30.13<br>
>> 大小比例为（以地球为1，直径1390000千米）；<br>
木星 ：土星 ：天王星 ：海王星 ：地球 ：金星 ：火星 ：水星 ：太阳<br>
1317 ：745 ： 65 ： 57 ： 1 ：0.86 ： 0.15 ：0.056 ： 1300000<br>
>> 2.构造太阳与九大行星，使用`3D贴图`构造`Materials`，之后适当调整距离及大小<br>
>> 得到效果图如下：<br>
![效果图1](https://github.com/wyj16340227/3D-unity/blob/master/%E6%95%88%E6%9E%9C%E5%9B%BE1.png "效果图1")<br>
>> 3.编写函数，代码如下：
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    public Transform Sun;
    public Transform Mercury;
    public Transform Venus;
    public Transform Earth;
    public Transform Mars;
    public Transform Jupiter;
    public Transform Satune;
    public Transform Uranus;
    public Transform Neptune;
    public float EarthRevolutionSpeed;
    public float EarthRotateSpeed;

	// Use this for initialization
	void Start () {
        EarthRevolutionSpeed = 1f;
        EarthRotateSpeed = 1f;
	}
	
	// Update is called once per frame
	void Update () {
        Mercury.RotateAround(Sun.position, new Vector3(0.1f, 1, 0), EarthRevolutionSpeed * 3 * Time.deltaTime);
        Mercury.Rotate(new Vector3(1,2, 0), EarthRotateSpeed * 2 * Time.deltaTime);
        Venus.RotateAround(Sun.position, new Vector3(0.2f, 1, 0), EarthRevolutionSpeed * 2.5f * Time.deltaTime);
        Venus.Rotate(new Vector3(1, 2.5f, 0), EarthRotateSpeed * 2.5f* Time.deltaTime);
        Earth.RotateAround(Sun.position, Vector3.up, EarthRevolutionSpeed);
        Earth.Rotate(new Vector3(1, 1.732f, 0), EarthRotateSpeed);
        Mars.RotateAround(Sun.position, new Vector3(0.12f, 1, 0), EarthRevolutionSpeed * 0.9f * Time.deltaTime);
        Mars.Rotate(new Vector3(1, 1.3f, 0), EarthRotateSpeed * 2.7f * Time.deltaTime);
        Jupiter.RotateAround(Sun.position, new Vector3(0.15f, 1, 0), EarthRevolutionSpeed * 0.87f * Time.deltaTime);
        Jupiter.Rotate(new Vector3(1, 2.7f, 0), EarthRotateSpeed * 2.9f * Time.deltaTime);
        Satune.RotateAround(Sun.position, new Vector3(0.19f, 1, 0), EarthRevolutionSpeed * 0.6f * Time.deltaTime);
        Satune.Rotate(new Vector3(1, 1.7f, 0), EarthRevolutionSpeed * 27 * Time.deltaTime);
        Uranus.RotateAround(Sun.position, new Vector3(-0.1f, 1, 0), EarthRevolutionSpeed * 0.55f * Time.deltaTime);
        Uranus.Rotate(new Vector3(1, 3.5f, 0), EarthRotateSpeed * 4 * Time.deltaTime);
        Neptune.RotateAround(Sun.position, new Vector3(-0.22f, 1, 0), EarthRevolutionSpeed * 0.1f * Time.deltaTime);
        Neptune.Rotate(new Vector3(1, 1.3f, 0), EarthRevolutionSpeed * 7.9f * Time.deltaTime);
    }
}
```
# 编程实践
>* 阅读以下游戏脚本
>> 祭司和魔鬼<br>
>>祭司和魔鬼是一款益智游戏，你将在这段时间内帮助神父和魔鬼过河。在河的一边有三个牧师和三个魔鬼。他们都想到达这条河的另一边，但只有一条船，这条船每次只能载两人。而且必须有一个人将船从一侧转向另一侧。在Flash游戏中，您可以点击它们来移动它们，然后单击go按钮将游艇移动到另一个方向。如果祭司在河边的恶魔中被编号，他们会被杀死，游戏结束。你可以用很多方法来尝试。让所有的牧师活着！祝你好运！<br>
>程序需要满足的要求：<br>
>* * 玩游戏（http://www.flash-game.net/game/2535/priests-and-devils.html）
>* * 列出游戏中提及的事物（对象）
>* * 用表格列出玩家动作表（规则表），注意，动作越少越好
>* * 请将游戏中对象做成预制
>* * 在GenGameObjects中创建长方形，正方形，球及其色彩代表游戏中的对象。
>* * 使用C＃集合类型有效组织对象
>* * 整个游戏仅主摄像机和一个空对象，其他对象必须代码动态生成!!!。整个游戏不许出现查找游戏对象，SendMessage这类突破程序结构的通讯耦合语句。违背本条准则，不给分
>* * 请使用课件架构图编程，不接受非MVC结构程序
>* * 注意细节，例如：船未靠岸，牧师与魔鬼上下船运动中，均不能接受用户事件！
>> 1.首先，罗列出游戏中出现的事物：<br>
`牧师`, `魔鬼`, `船只`, `海岸`, `灯光`<br>
对象|活动|规则
-|-|-
船只|启程|船上有人且船在岸边
魔鬼|上船|船上有空位且船只停靠岸边有魔鬼
牧师|上船|船上有空位且船只停靠岸边由牧师
牧师/魔鬼|下船|船只到达对岸
系统|胜利|魔鬼与牧师全部到达左岸
系统|失败|一边魔鬼数量超过牧师
系统|重置|无<br>
>> 3.以`Sphere`作为`魔鬼`，以`Cube`作为`牧师`，以`Cube`作为`河岸`以及`船只`，以系统预设灯光直接作为`Light`，调整他们的大小，将他们放入`Recourse`资源文件夹下的`Prefabs`文件夹下作为预设对象，并为他们分别添加标签。<br>
![预设文件](https://github.com/wyj16340227/3D-unity/blob/master/%E9%A2%84%E8%AE%BE%E6%96%87%E4%BB%B6.png "预设文件")<br>
>> 4.关系图<br>
![关系图1](http://imglf4.nosdn.127.net/img/S3F1ejdrdGNrNFhvbUJCZHNtczRDN25tVTc5WUQvaTk3ckROOVNhZC9yRFF5K0k0R2hkWVVnPT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0 "关系图1")<br>
