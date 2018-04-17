# 作业与练习
> - 编写一个简单的鼠标打飞碟（Hit UFO）游戏<br>
> 
>  - - 游戏内容要求：<br>
>
>> 1.  游戏有 n 个 round，每个 round 都包括10 次 trial；<br>
>> 2.  每个 trial 的飞碟的色彩、大小、发射位置、速度、角度、同时出现的个数都可能不同。它们由该 round 的 ruler 控制；<br>
>> 3.  每个 trial 的飞碟有随机性，总体难度随 round 上升；<br>
>> 4.  鼠标点中得分，得分规则按色彩、大小、速度不同计算，规则可自由设定。<br>
>
> - - 游戏的要求：<br>
>
>> 使用带缓存的工厂模式管理不同飞碟的生产与回收，该工厂必须是场景单实例的！具体实现见参考资源 Singleton 模板类<br>
>> 近可能使用前面 MVC 结构实现人机交互与游戏模型分离<br>

- 如下：<br>
> 
> 使用了MVC与游戏工厂模型<br>
> - 1.类图<br>
> <br>
![](http://imglf3.nosdn.127.net/img/S3F1ejdrdGNrNFVSMUdMZDJnMUJ6QzNZNklUQ2lnVUEzY2VyZTRYODFHTDRVU051bVlaT09nPT0.jpg?imageView&rotate=90&thumbnail=500x0&quality=96&stripmeta=0&type=jpg "")<br>
> - 2.工厂实现<br>
> <br>
```
public class DiskFactory : MonoBehaviour
    {
        public GameObject diskPrefab;
        private static List<GameObject> used = new List<GameObject>();
        private static List<GameObject> free = new List<GameObject>();
        public static Vector3 leftBounder = new Vector3(-7, 0, 0);


        void Start()
        {
        }

        public GameObject GetDisk()
        {
            if (free.Count != 0)
            {
                used.Add(free[0]);
                free.RemoveAt(0);
                used[used.Count - 1].SetActive(true);
                int colorSet = Random.Range(0, 5);
                if (colorSet > 4)
                {
                    used[used.Count - 1].GetComponent<MeshRenderer>().material.color = Color.black;
                }
                else if (colorSet > 3)
                {
                    used[used.Count - 1].GetComponent<MeshRenderer>().material.color = Color.blue;
                }
                else if (colorSet > 2)
                {
                    used[used.Count - 1].GetComponent<MeshRenderer>().material.color = Color.white;
                }
                else if (colorSet > 1)
                {
                    used[used.Count - 1].GetComponent<MeshRenderer>().material.color = Color.green;
                }
                else if (colorSet > 0)
                {
                    used[used.Count - 1].GetComponent<MeshRenderer>().material.color = Color.gray;
                }
            }
            else
            {
                GameObject tempDisk = Instantiate(Resources.Load("Prefabs/Disk"), Vector3.up, Quaternion.identity) as GameObject;
                int colorSet = Random.Range(0, 5);
                if (colorSet > 4)
                {
                    tempDisk.GetComponent<MeshRenderer>().material.color = Color.black;
                } else if (colorSet > 3)
                {
                    tempDisk.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
                else if (colorSet > 2)
                {
                    tempDisk.GetComponent<MeshRenderer>().material.color = Color.white;
                }
                else if (colorSet > 1)
                {
                    tempDisk.GetComponent<MeshRenderer>().material.color = Color.green;
                }
                else if (colorSet > 0)
                {
                    tempDisk.GetComponent<MeshRenderer>().material.color = Color.gray;
                }
                used.Add(tempDisk);
                used[used.Count - 1].SetActive(true);
            }
            used[used.Count - 1].transform.position = leftBounder;
            return used[used.Count - 1];
        }

        public void StartPosition(GameObject disk)
        {
            disk.transform.position = leftBounder;
        }

        public void FreeDisk(GameObject disk)
        {
            disk.SetActive(false);
            used.Remove(disk);
            free.Add(disk);
        }
    }
```
> - 3.`Action`<br>
> <br>
> 类套用了网上的模板，在这里只是实现特有的动作`FlyAction`及动作管理器`DiskActionManager`<br>
```
public class FlyAction : SSAction
    {
        public Vector3 dirction;
        // 飞行方向
        private float time;
        // 已经飞行时间
        float down;

        public static FlyAction GetSSAction(Vector3 _dirction)
        {
            FlyAction currentAction = ScriptableObject.CreateInstance<FlyAction>();
            currentAction.dirction = _dirction;
            currentAction.time = 0;
            return currentAction;
        }

        public override void Start()
        {
            down = 5;
        }

        public override void Update()
        {
            if (!this.gameObject.activeSelf) // 如果飞碟已经回收
            {
                this.destory = true;
                this.callback.SSEventAction(this, SSActionEventType.STARTED);
                return;
            }
            time += Time.deltaTime;
            transform.position += Time.deltaTime * dirction;
            // 各个方向的匀速运动
            transform.position += Vector3.down * down * time * Time.deltaTime;
            // 竖直方向的匀加速运动
            if (this.transform.position.y < -3)
            {
                this.destory = true;
                // 回收飞碟
                this.gameObject.SetActive(false);
                this.callback.SSEventAction(this, SSActionEventType.COMPLETED, 0, null, this.gameObject);
            }
        }
    }
```
```
public class DiskActionManager : SSActionManager, ISSActionCallback
    {
        public ScoreKeeper score { get; set; }

        DiskFactory factory;
        Vector3 leftBounder = new Vector3(-15, 10, -5);

        public void GetScore (ScoreKeeper scoreKeeper)
        {
            score = scoreKeeper;
        }

        new void Start()
        {
            factory = gameObject.AddComponent<DiskFactory>() as DiskFactory;
        }

        // Update is called once per frame
        new void Update()
        {
            base.Update();
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，在scene视图中能看到由摄像机发射出的射线
                    GameObject gameObj = hitInfo.collider.gameObject;
                    if (gameObj.name.StartsWith("Disk") == true)//当射线碰撞目标的name包含Cube，执行拾取操作
                    {
                        score.Hit();
                        gameObj.transform.position = new Vector3(-1, -4, -1);
                        factory.FreeDisk(gameObj);
                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，在scene视图中能看到由摄像机发射出的射线
                    GameObject gameObj = hitInfo.collider.gameObject;
                    if (gameObj.name.StartsWith("Disk") == true)//当射线碰撞目标的name包含Cube，执行拾取操作
                    {
                        Debug.Log(gameObj.name);
                    }
                }
            }
        }

        public void FlyDisk(int num)
        {
            GameObject disk;
            for (int i = 0; i < num; i += 2)
            {
                disk = factory.GetDisk();
                FlyAction fly = FlyAction.GetSSAction(new Vector3(Random.Range(5f, 20), Random.Range(2.5f, 10), Random.Range(0, 3f)));
                this.runAction(disk, fly, this);
            }
        }

        // 回调函数
        public void SSEventAction(SSAction source, SSActionEventType events = SSActionEventType.COMPLETED,
            int intParam = 0, string strParam = null, GameObject objParam = null)
        {

            if (events == SSActionEventType.COMPLETED)
            // 落到y轴以下
            {
                if (objParam != null)
                {
                    factory.FreeDisk(objParam);
                }
            }
        }
    }
```
> 在这里，`Disk`回收有两种方式可以实现，一种是鼠标点击，获得`Disk`，第二种是`Disk`掉在地上，第一种很好实现，第二种是通过将`Disk`的`Active`属性置为`false`，在下一轮的时候进行检测，回收。<br>
> - 4.场记`ScoreKeeper`<br>
> <br>
```
public class ScoreKeeper : MonoBehaviour
{
    private int score;
    private int round;
    public static int MAX_ROUND = 10;

    public ScoreKeeper ()
    {
        Debug.Log("Create scoreKeeper");
        score = 0;
        round = 1;
    }

    public void Reset ()
    {
        score = 0;
        round = 1;
    }

    public bool NextRound ()
    {
        if (round == MAX_ROUND)
        {
            return false;
        } else
        {
            return true;
        }
    }

    public void Hit ()
    {
        score++;
    }

    public int GetRound ()
    {
        return round;
    }

    public int GetScore ()
    {
        return score;
    }
}
```
> 在场记中，因为需要管理的数据较少，所以将计分与记回合都交给了记分员。<br>
> - 5.场景控制器`SceneController`<br>
> <br>
```
public enum GameStatues
{
    Pause,
    GameOver,
    Running,
    NextRound,
    Waiting,
    Counting
}

public class SceneController : MonoBehaviour {
    public GameStatues statues = GameStatues.Running;
    public static int MAX_SCORE = 10;
    public static int MAX_ROUND = 3;
    public float time = 4f;
    public float waitTime = 0f;
    public static ScoreKeeper scoreKeeper;
    public static DiskActionManager diskmanager;
    public static GameObject player;
    GUIStyle fontstyle = new GUIStyle();

    public int restDisk = 10;
	// Use this for initialization
	void Start () {
        scoreKeeper = gameObject.AddComponent<ScoreKeeper>() as ScoreKeeper;
        diskmanager = gameObject.AddComponent<DiskActionManager>() as DiskActionManager;
        diskmanager.GetScore(scoreKeeper);
        statues = GameStatues.Pause;

        fontstyle.fontSize = 50;
        fontstyle.normal.textColor = new Color(255, 255, 255);
        fontstyle.alignment = TextAnchor.MiddleCenter;
    }
	
	// Update is called once per frame
	void Update () {
		if (statues == GameStatues.Running)
        {
            if (restDisk > 0 && waitTime <= 0)
            {
                diskmanager.FlyDisk(scoreKeeper.GetRound());
                restDisk -= scoreKeeper.GetRound();
                waitTime = 4f;
            } else if (restDisk > 0 && waitTime > 0)
            {
                waitTime -= Time.deltaTime;
            } else if (restDisk <= 0)
            {
                if (scoreKeeper.GetRound() != MAX_ROUND)
                {
                    restDisk = 10;
                    scoreKeeper.NextRound();
                    statues = GameStatues.NextRound;
                } else
                {
                    statues = GameStatues.GameOver;
                }
            }
        }
	}

    public void OnGUI()
    {

        if (GUI.Button(new Rect(Screen.width - 300, 0, 100, 30), "Start", fontstyle))
        {
            time = 4f;
            statues = GameStatues.Counting;
        }
        GUI.TextArea(new Rect(0, 0, 200, 50), "Round: " + scoreKeeper.GetRound().ToString(), fontstyle);
        GUI.TextArea(new Rect(0, 50, 200, 50), "Score: " + scoreKeeper.GetScore().ToString(), fontstyle);
        if (statues == GameStatues.Counting)
        {
            if (time >= 3)
            {
                GUI.TextArea(new Rect(Screen.width / 2 - 50, 100, 40, 40), "3");
            } else if (time >= 2)
            {
                GUI.TextArea(new Rect(Screen.width / 2 - 50, 100, 40, 40), "2");
            }
            else if (time >= 1)
            {
                GUI.TextArea(new Rect(Screen.width / 2 - 50, 100, 40, 40), "1");
            }
            else if (time >= 0)
            {
                GUI.TextArea(new Rect(Screen.width / 2 - 50, 100, 40, 40), "0");
            }
            time -= Time.deltaTime;

            if (time < 0)
            {
                statues = GameStatues.Running;
            }
        }
        else if (statues == GameStatues.NextRound)
        {
            if (GUI.Button (new Rect (Screen.width - 300, Screen.height  - 200, 100, 30), "NEXT ROUND!", fontstyle))
            {
                time = 4f;
                statues = GameStatues.Counting;
                if (!scoreKeeper.NextRound())
                {
                    statues = GameStatues.GameOver;
                }
            }
        }
        else if (statues == GameStatues.GameOver)
        {
            GUI.Button(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 200, 100, 30), "Game Over", fontstyle);
        }
    }
}
```
> 场景控制器里实例化了`ScoreKeeper`与`DiskActionManager`，通过`OnGUI()`与用户完成交互。<br>
