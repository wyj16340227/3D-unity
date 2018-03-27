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
>>构图如下：<br>
![作业2-2](https://github.com/wyj16340227/3D-unity/blob/master/%E4%BD%9C%E4%B8%9A2-2.png "作业2-2")
>
>整理相关学习资料，编写简单代码验证以下技术的实现：
>* 查找对象
>>编写代码如下：
```
    //通过名字查找： 
    public static GameObject MyFind(string name)
    {
        return GameObject.Find(name);
    }
    //通过标签查找： 
    //单个对象：
    public static GameObject MyFindWithTag(string tag)
    {
        return GameObject.FindWithTag(tag);
    }
    //多个对象：
    public static GameObject[] MyFindGameObjectsWithTag(string tag)
    {
        GameObject.FindGameObjectsWithTag(tag);
    }
```
>* 添加子对象
>>编写代码如下：
```
    public static GameObect MyCreatePrimitive(PrimitiveType type)
    {
        return GameObject.CreatePrimitive(type);
    }
```
>* 遍历对象树
>>编写代码如下：
```
    void MyConvenience (Transform child in transform) { 
        Debug.Log(child.gameObject.name); 
    }
```
>* 清除所有子对象
>>编写代码如下：
```
    void MyDestroy (Transformchild in transform) {
        Destroy(child.gameObject);
    }
```
>
>资源预设（Prefabs）与 对象克隆 (clone)
>* 预设（Prefabs）有什么好处？<br>
>>预设相当于创建一个模板，方便构造多个相同的对象。例如，当我要构造一片森林，可以先组建一棵树木并将它作为预设，就可以通过预设创建多个相同的对象，省去了多次重复的繁琐操作性。
>* 预设与对象克隆 (clone or copy or Instantiate of Unity Object) 关系？<br>
>>预设不同于对象克隆，对象克隆后，母体的改变不会改变子体的属性，但预设改变后，所有由预设创建的对象的属性都会发生改变。简单的例子，比如设计了一颗树苗作为预设，并通过该预设创建了一片树林，有一天突然发现小树林不美观，就将预设的小树苗设计成了一棵大树，那么所有的小树苗都会变成大树，小树林就变成了森林。而对于克隆，假如通过树苗A克隆得到树苗B，将A设计为大树后，B并不会发生改变。
>* 制作 table 预制，写一段代码将 table 预制资源实例化成游戏对象。<br>
>>
>
>尝试解释组合模式（Composite Pattern / 一种设计模式）。使用 BroadcastMessage() 方法
>* 向子对象发送消息
>>1.组合模式，将对象组合成树形结构以表示“部分-整体”的层次结构，组合模式使得用户对单个对象和组合对象的使用具有一致性。掌握组合模式的重点是要理解清楚 “部分/整体” 还有 ”单个对象“ 与 "组合对象" 的含义。<br>
>>组合模式可以让客户端像修改配置文件一样简单的完成本来需要流程控制语句来完成的功能。<br>
>>2.编写代码如下：
```
    //父类：
    void Start()
    {
        this.BroadcastMessage("MyHello");
    }
    //子类：
    void MyHello()
    {
        print("Hello!");
    }
```
# 编程实践，小游戏
>游戏内容： 井字棋 或 贷款计算器 或 简单计算器 等等<br>
>技术限制： 仅允许使用 IMGUI 构建 UI<br>
>作业目的：<br>
>* 提升 debug 能力
>* 提升阅读 API 文档能力
>>1.井字棋：<br>
>>实现效果如下：<br>
![作业2-3](https://github.com/wyj16340227/3D-unity/blob/master/%E4%BD%9C%E4%B8%9A2-3.png "作业2-3")<br>
>>关键代码如下：
```
    public GUIStyle TipsStyle;
    private string[] play = {"O", "X", " "};        //存储玩家的表示方式，" "表示无玩家
    private int currentPlayer;                      //当前玩家，0为O玩家；1为X玩家
    private int[ , ] cheese = new int[3, 3];        //存储当前棋盘状态
    bool wrong_input;                               //若棋子下重复，则为正
    int current_step;                               //当前棋盘棋子数目
    int over;                                       //游戏是否有结果，1为一方胜出，2为平局，0为无结果
```
```
    //判断是否胜利
    private bool GameOver ()
    {
        //横向胜利
        if (cheese[0, 0] == cheese[0, 1] && cheese[0, 1] == cheese[0, 2] && cheese[0, 0] != 2)
        {
            print("h");
            return true;
        }
        if (cheese[1, 0] == cheese[1, 1] && cheese[1, 1] == cheese[1, 2] && cheese[1, 0] != 2)
        {
            print("h");
            return true;
        }
        if (cheese[2, 0] == cheese[2, 1] && cheese[2, 1] == cheese[2, 2] && cheese[2, 0] != 2)
        {
            print("h");
            return true;
        }

        //竖向胜利
        if (cheese[0, 0] == cheese[1, 0] && cheese[1, 0] == cheese[2, 0] && cheese[0, 0] != 2)
        {
            print("h");
            return true;
        }
        if (cheese[0, 1] == cheese[1, 1] && cheese[1, 1] == cheese[2, 1] && cheese[0, 1] != 2)
        {
            print("h");
            return true;
        }
        if (cheese[0, 2] == cheese[1, 2] && cheese[1, 2] == cheese[2, 2] && cheese[0, 2] != 2)
        {
            print("h");
            return true;
        }

        //斜向胜利
        if (cheese[0, 0] == cheese[1, 1] && cheese[1, 1] == cheese[2, 2] && cheese[0, 0] != 2)
        {
            print("h");
            return true;
        }
        if (cheese[0, 2] == cheese[1, 1] && cheese[1, 1] == cheese[2, 0] && cheese[0, 2] != 2)
        {
            print("h");
            return true;
        }

        return false;
    }

    private void OnGUI()
    {
        //重新加载
        if (GUI.Button(new Rect(200, 200, 100, 20), "开始!"))
        {
            reload();
        }
        //一方胜利
        if (over == 1)
        {
            GUI.Label(new Rect(0, 0, 120, 30), "玩家：" + play[currentPlayer] + "取得胜利！", TipsStyle);
        }
        //平局
        else if (over == 2)
        {
            GUI.Label(new Rect(0, 0, 120, 30), "平局！", TipsStyle);
        }
        else
        {
            GUI.Label(new Rect(0, 0, 100, 20), "井字棋", TipsStyle);
            GUI.Label(new Rect(0, 20, 100, 20), "当前玩家: " + play[currentPlayer], TipsStyle);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (GUI.Button(new Rect(100 + 30 * i, 100 + 30 * j, 30, 30), play[cheese[i, j]]))
                    {
                        //该位置已有棋子
                        if (cheese[i, j] != 2)
                        {
                            wrong_input = true;
                        }
                        //该位置无棋子
                        else
                        {
                            //下棋
                            cheese[i, j] = currentPlayer;
                            currentPlayer = (currentPlayer + 1) % 2;
                            current_step++;
                            //判断是否一方胜利
                            if (GameOver())
                            {
                                over = 1;
                                currentPlayer = (currentPlayer + 1) % 2;
                            } else
                            {
                                //是否棋盘走满，平局
                                if (current_step == 9)
                                {
                                    over = 2;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
```
[演示视频](https://github.com/wyj16340227/3D-unity/blob/master/16340227%20%E4%BA%95%E5%AD%97%E6%A3%8B.mp4)
>>2.贷款计算器：
>>实现效果如下：<br>
![作业2-4](https://github.com/wyj16340227/3D-unity/blob/master/%E4%BD%9C%E4%B8%9A2-4.png "作业2-4")<br>
>>关键代码如下：
```
        interest = 0f;          //利息
        loan = 0f;              //贷款金额
        month = 0;              //还款期限
        inputInterest = "";     //用户输入利息
        inputLoan = "";         //用户输入贷款金额
        inputMonth = "";        //用户输入还款期限
        compute = false;        //是否计算
        result = 0f;            //每月应还款
```
```
        //如果需要计算
        if (compute)
        {
            interest = double.Parse(inputInterest);
            loan = double.Parse(inputLoan);
            month = int.Parse(inputMonth);
            result = (loan + loan * month * interest / 1200) / month;
            compute = false;
        }
        GUI.Label(new Rect(0, 0, 100, 20), "贷款金额(元): ");
        GUI.Label(new Rect(0, 20, 100, 20), "年利率(%): ");
        GUI.Label(new Rect(0, 40, 100, 20), "还款期限(月): ");
        GUI.Label(new Rect(0, 60, 200, 20), "每月应还(元): " + result.ToString());
        inputLoan = GUI.TextField(new Rect(100, 0, 50, 20), inputLoan);
        inputInterest = GUI.TextField(new Rect(100, 20, 50, 20), inputInterest);
        inputMonth = GUI.TextField(new Rect(100, 40, 50, 20), inputMonth);
        //当点击计算，将compute置为true，在下一次调用计算
        if (GUI.Button(new Rect(50, 100, 50, 20), "计算"))
        {
            compute = true;
        }
        //当点击清零，重新加载所有数据
        if (GUI.Button(new Rect(100, 100, 50, 20), "清除"))
        {
            reload();
        }
```
>>[演示视频](https://github.com/wyj16340227/3D-unity/blob/master/16340227%20%E8%B4%B7%E6%AC%BE%E8%AE%A1%E7%AE%97%E5%99%A8.mp4)
<iframe width="560" height="315" src="http://v.youku.com/v_show/id_XMzQ5MzkwMzg0OA==.html" frameborder="0" allowfullscreen></iframe>
