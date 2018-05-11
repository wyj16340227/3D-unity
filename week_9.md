![](http://imglf5.nosdn.127.net/img/S3F1ejdrdGNrNFhnbVYzZWJjVFljOHFsNkZyWmlXUXMzYzM1bE1YemtwS09COXMrbkJXWDhBPT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0)
 - 结构`UML`<br>
 ![](http://imglf4.nosdn.127.net/img/S3F1ejdrdGNrNFhnbVYzZWJjVFljODMyR2t0K2NacWs1TC9uakN1Q0xCbWVLNWZ6a25PaTVnPT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0)
 
 - 游戏介绍<br>
 ![](http://imglf4.nosdn.127.net/img/S3F1ejdrdGNrNFhnbVYzZWJjVFljMXFsSThySjd0WUIrS0J2M2FCRDhkb2VXRFhjQXFNNWtRPT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0)
 ![](http://imglf6.nosdn.127.net/img/S3F1ejdrdGNrNFhnbVYzZWJjVFljeHhzVWViN2M5SGdjcGNkVStka2pqYXI5aDBYNkVHRG9RPT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0)

 >  1.该游戏存在6个区域（1~6），每个区域都有一个巡逻兵，各区域用围墙分开，相邻区域的围墙有间隙可以通过，角色不可穿墙。游戏角色初始化在时在区域5。
 >  2.当玩家进入某一区域时，该区域内的巡逻兵会自动追击玩家，直至玩家离开该区域。若玩家被巡逻兵抓到，游戏结束。当巡逻兵所在区域没有其他角色的时候巡逻兵沿着四边形运动。
 >  3.各区域巡逻兵不会离开自己所在区域，当玩家躲过一个巡逻兵的追击时，即安全的通过一个区域时，分数会+1。
 >  4.每得5分，游戏等级提升一次，升级巡逻兵的速度。<br>
 > 
 - 关键 **模式**
 > - 工厂模式<br>
 > 
 >> 在游戏中，使用巡逻兵工厂来产生巡逻兵，当兵工厂中有巡逻兵时，重设参数并将该巡逻兵交给需求方，当兵工厂中没有巡逻兵的时候，实例化一个新的巡逻兵并初始化，返回给需求方。<br>
 > 
```
        public GameObject GetSoldier()
        {
            if (free.Count != 0)
            {
                used.Add(free[0]);
                free.RemoveAt(0);
                used[used.Count - 1].SetActive(true);
                used[used.Count - 1].GetComponent<MeshRenderer>().material.color = Color.black;
            }
            else
            {
                GameObject tempDisk = Instantiate(Resources.Load("Prefabs/Soldier"), Vector3.up, Quaternion.identity) as GameObject;
                used.Add(tempDisk);
                used[used.Count - 1].GetComponent<MeshRenderer>().material.color = Color.black;
                used[used.Count - 1].SetActive(true);
            }
            return used[used.Count - 1];
        }
```
<br>

 > - 发布-订阅模式
 > <br>
 >> 使用玩家作为消息发布方，将自己的相关信息（如：生存状态，位置，所在区域）发送给订阅者，订阅者根据自己的需求，从所有信息中得到自己想要的部分（如：场景控制器判断游戏是否结束，巡逻兵判断是否在自己所在区域，是否追击），并根据信息做下一步动作。<br>
 > 该游戏中，发布对象为玩家，发布信息有：存活状态，位置，所在区域。订阅者有巡逻兵：根据玩家所在区域判断是否追击、根据玩家状态判断是否追击、根据玩家位置判断追击方向；场景控制器：根据玩家状态判断游戏是否结束；记分员：根据玩家所在区域判断是否得分。<br>
```
//Observer
    public abstract class Observer : MonoBehaviour
    {
        public abstract void Reaction(bool aLive, Vector3 playPosition, int playLocation);

//Subject
    public abstract class Subject : MonoBehaviour
    {
        List<Observer> m_Observers = new List<Observer>();

        public abstract void Attach(Observer listener);

        public abstract void Detach(Observer theObserver);

        public abstract void Notify(bool aLive, Vector3 playPosition, int playLocation);
    }
    }
    
//Player<Publish>
        protected List<Observer> obs = new List<Observer>();   //所有观察者
        public override void Attach(Observer o)
        {
            obs.Add(o);
        }

        public override void Detach(Observer o)
        {
            obs.Remove(o);
        }

        public override void Notify(bool aLive, Vector3 playPosition, int playLocation)
        {
            foreach (Observer o in obs)
            {
                o.Reaction(aLive, playPosition, playLocation);
            }
        }

//SceneController<Subscribe>
        public override void Reaction(bool aLive, Vector3 playPosition, int playLocation)
        {
            isOver = !aLive;
        }

//ScoreManager<Subscribe>
        public override void Reaction(bool aLive, Vector3 playPosition, int playLocation)
        {
            playerIsLive = aLive;
            currentLoc = playLocation;
            if (currentLoc != lastLoc)
            {
                score++;
                lastLoc = currentLoc;
            }
        }

//Soldier<Subscribe>
        public override void Reaction (bool _aLive, Vector3 _playPosition, int _playLocation)
        {
            playerLocation = _playLocation;
            playerPosition = _playPosition;
            playAlive = _aLive;
        }
```
 - 实现及关键代码<br>
 > 
 > - 角色`Player`
 >  
 >> `Player`的运动通过从键盘上读取输入来实现。在角色的编写中，会遇到一个问题：当碰撞到墙壁时，由于输入由玩家控制，所以可能导致碰撞挤压导致旋转，旋转后，再运动可能会“飞上天”，因此，每次运动的时候，通过显式修改`transform.rotation`的位置，使得`x` `z`的值始终为`0`。同时，角色的消息发布是在`Update()`中进行的，但是由于发生碰撞事件之后，该角色的变为不活跃，不再执行该函数，所以在发生碰撞后同样要执行一次`Notify()`。<br>

```
            //move
            transform.Translate(Vector3.forward * speed * Input.GetAxisRaw("Horizontal") * Time.deltaTime);
            transform.Translate(Vector3.left * speed * Input.GetAxisRaw("Vertical") * Time.deltaTime);
            transform.Rotate(Vector3.up, Input.GetAxisRaw("Horizontal") * angle_speed * Time.deltaTime);
            //set rotation to make player at floor
            float y = transform.rotation.y;
            transform.rotation = Quaternion.Euler(0, y, 0);

        private void OnCollisionEnter(Collision c)
        {
            if (c.gameObject.CompareTag("Soldier"))
            {
                isLive = false;
                Notify(isLive, transform.position, loc);
            }
        }

```

 > - 角色`Soldier`与角色工厂`SoldierFactory`
 >  
 >> 角色运动的第一个问题是判断路径，在得到玩家发布的信息后，会先判断玩家的所在区域，如果是自己的所在区域，就向玩家方向追击。当巡逻时，会先想着当前前进方向前进，运动到一定时间的时候，会转向90度（因此正常运行会走四边形路线），当发生碰撞的时候，如果和墙发生了碰撞，就转向。<br>
```
        void catchPlayer()
        {
            catching = true;
            transform.LookAt(playerPosition);
            transform.position = Vector3.Lerp(transform.position, playerPosition, catchSpeed * Time.deltaTime);
        }

        void move ()
        {
            transform.Translate(0, 0, walkSpeed * Time.deltaTime);
        }
        update () {
        .......
            if (index == playerLocation)
            {
                ani.SetBool("catching", true);
                catchPlayer();
            }
            else
            {
                ani.SetBool("catching", false);
                move();
                time += Time.deltaTime;
            }
            if (time >= 3f)
            {
                turn();
            }
        }
```

 >> 第二个问题是巡逻兵只能够在自己所在方向运动，那么碰撞到墙可以转向，如果遇到空隙了呢？此时不会发生碰撞，因此，编写了函数`checkTurn()`，此函数通过调用`getNowLocation()`来判断巡逻兵此时的位置，当此时的位置与自己的编号不同的时候，就需要转向。`checkTurn()`在`Update()`中每一帧调用。
 >> 在这里，使用了动画状态机，游戏角色有两个动作，分别为`walk`和`run`，对应巡逻与追赶。<br>
 > 
```
        void checkTurn ()
        {
            if (index != getNowLocation())
            {
                turn();
            }
        }

        int getNowLocation()
        {
            if (gameObject.transform.position.z > 5 && gameObject.transform.position.z < 15)
            {
                if (gameObject.transform.position.x < 0)
                {
                    return 3;
                }
                else
                {
                    return 6;
                }
            }
            else if (gameObject.transform.position.z > -5 && gameObject.transform.position.z < 5)
            {
                if (gameObject.transform.position.x < 0)
                {
                    return 2;
                }
                else
                {
                    return 5;
                }
            }
            else
            {
                if (gameObject.transform.position.x < 0)
                {
                    return 1;
                }
                else
                {
                    return 4;
                }
            }
        }
```
 
 >> `SoldierFactory`实现使用了`List<Soldier> used`与`List<Soldier> free`，实现方式已在设计模式中给出，不再赘述。
 >
 > - `Data`数据源
 >  
 >> 此模块用来生成场景所需要的所有对象。有两个关键问题，第一个问题是，玩家的声明在这里，但是需要订阅的`ScoreManager`及`SceneController`在他的上层，在这里用了一个不规范的方法，`getObserver`，通过将对象传进来，然后再创建。第二个问题是，如何获得巡逻兵的编号及初始位置。在这里，使用字段`Soldier` + 当前士兵数目作为`name`，赋给巡逻兵，并且创建了位置数组，将6个初始位置通过在地图上的位置得到。<br>
 >
```
    public class Data : MonoBehaviour
    {
        private Subject sub;
        private GameObject player;
        private Observer obs;
        private SoldierFactory soldierFac;
        private int soldierNum;
        private List<GameObject> soldierSet = new List<GameObject>();
        private Vector3[] soldierPos = new Vector3[] {new Vector3(-7.5f, -0.5f, -10), new Vector3(-7.5f, -0.5f, 0), new Vector3(-7.5f, -0.5f, 10),
            new Vector3(7.5f, -0.5f, -10), new Vector3(7.5f, -0.5f, 0), new Vector3(7.5f, -0.5f, 10)};

        void Start()
        {
            soldierNum = 0;
            soldierFac = gameObject.AddComponent<SoldierFactory>() as SoldierFactory;
            Instantiate(Resources.Load("Prefabs/World"), new Vector3 (0, 0f, 0), Quaternion.identity);
            player = Instantiate(Resources.Load("Prefabs/Player"), new Vector3(1, -0.5f, 1), Quaternion.identity) as GameObject;
            sub = player.GetComponent<Player>();

            for (int i = 0; i < 6; i++)
            {
                GameObject soldier = soldierFac.GetSoldier();
                soldier.name = "Soldier" + soldierNum;
                soldierNum++;
                obs = soldier.GetComponent<Soldier>();
                sub.Attach(obs);
                soldier.transform.position = soldierPos[i];
            }
        }

        public void getObserver (SceneController obs)
        {
            Subject tempSub = player.GetComponent<Player>();
            tempSub.Attach(obs);
        }

        public void getObserver(ScoreManager obs)
        {
            Subject tempSub = player.GetComponent<Player>();
            tempSub.Attach(obs);
        }
    } 
```
 > - `ScoreManager`记分员<br>
 >  
 >> 比较简单，主要就是如何计分。由于订阅了玩家，因此，根据玩家的所在区域来计分，使用一个变量`lastLoc`来记录玩家上一次所在的区域，当得到的新的信息中，玩家的位置发生了改变，那么说明玩家已经安全的通过了上一区域，此时更新`lastLoc`并得分。<br>
```
        public override void Reaction(bool aLive, Vector3 playPosition, int playLocation)
        {
            playerIsLive = aLive;
            currentLoc = playLocation;
            if (currentLoc != lastLoc)
            {
                score++;
                lastLoc = currentLoc;
            }
        }
```

 > - `SceneController`场景控制器与`UIController`UI控制器<br>
 >  
 >> 此场景控制器包括了一个`UIController`（显示分数），一个数据源`Data`。较为简单。`UIController`包括一个`ScoreManager`，可以直接看代码。<br>

 - 完整代码<br>
 >
 > - `Observer`
 >  
```
    public abstract class Observer : MonoBehaviour
    {
        public abstract void Reaction(bool aLive, Vector3 playPosition, int playLocation);

    }
```

 > - `Subject`
 >  
```
    public abstract class Subject : MonoBehaviour
    {
        List<Observer> m_Observers = new List<Observer>();

        public abstract void Attach(Observer listener);

        public abstract void Detach(Observer theObserver);

        public abstract void Notify(bool aLive, Vector3 playPosition, int playLocation);
    }
```

 > - `Player`
 >  
```
    public class Player : Subject
    {
        int loc;
        private float speed;
        private float angle_speed;
        private bool isLive;
        private Vector3 position;

        // Use this for initialization
        void Start()
        {
            speed = 5f;
            angle_speed = 3f;
            isLive = true;
        }
        protected List<Observer> obs = new List<Observer>();   //所有观察者

        int getNowLocation()
        {
            if (gameObject.transform.position.z > 5 && gameObject.transform.position.z < 15)
            {
                if (gameObject.transform.position.x < 0)
                {
                    return 3;
                }
                else
                {
                    return 6;
                }
            }
            else if (gameObject.transform.position.z > -5 && gameObject.transform.position.z < 5)
            {
                if (gameObject.transform.position.x < 0)
                {
                    return 2;
                }
                else
                {
                    return 5;
                }
            }
            else
            {
                if (gameObject.transform.position.x < 0)
                {
                    return 1;
                }
                else
                {
                    return 4;
                }
            }
        }

        public override void Attach(Observer o)
        {
            obs.Add(o);
        }

        public override void Detach(Observer o)
        {
            obs.Remove(o);
        }

        public override void Notify(bool aLive, Vector3 playPosition, int playLocation)
        {
            foreach (Observer o in obs)
            {
                o.Reaction(isLive, playPosition, playLocation);
            }
        }
        private void Update()
        {
            //move
            transform.Translate(Vector3.forward * speed * Input.GetAxisRaw("Vertical") * Time.deltaTime);
            transform.Translate(Vector3.right * speed * Input.GetAxisRaw("Horizontal") * Time.deltaTime);
            transform.Rotate(Vector3.up, Input.GetAxisRaw("Vertical") * angle_speed * Time.deltaTime);
            //set rotation to make player at floor
            float y = transform.rotation.y;
            transform.rotation = Quaternion.Euler(0, y, 0);
            float x = transform.position.x;
            float z = transform.position.z;
            transform.position = new Vector3(x, -0.5f, z);
            loc = getNowLocation();
            Notify(isLive, transform.position, loc);
        }

        private void OnCollisionEnter(Collision c)
        {
            if (c.gameObject.CompareTag("Soldier"))
            {
                isLive = false;
                Notify(isLive, transform.position, loc);
            }
        }
    }
```

 > - `Soldier`
 >  
```
    public class Soldier : Observer
    {
        private Animator ani;
        private int index;
        private bool catching;
        private int playerLocation;
        private Vector3 playerPosition;
        private float walkSpeed;
        private float catchSpeed;
        private bool playAlive;
        private float time;


        // Use this for initialization
        void Start()
        {
            ani = GetComponent<Animator>();
            index = getOwnIndex();
            walkSpeed = 0.25f;
            catchSpeed = 0.27f;
        }

        private int getOwnIndex()
        {
            string name = this.gameObject.name;
            char tempIndex = name[name.Length - 1];
            int result = tempIndex - '0';
            return result + 1;
        }


        public override void Reaction (bool _aLive, Vector3 _playPosition, int _playLocation)
        {
            playerLocation = _playLocation;
            playerPosition = _playPosition;
            playAlive = _aLive;
        }

        void catchPlayer()
        {
            catching = true;
            transform.LookAt(playerPosition);
            transform.position = Vector3.Lerp(transform.position, playerPosition, catchSpeed * Time.deltaTime);
        }

        void move ()
        {
            transform.Translate(0, 0, walkSpeed * Time.deltaTime);
        }

        void checkTurn ()
        {
            if (index != getNowLocation())
            {
                turn();
            }
        }

        void turn ()
        {
            transform.Rotate(0, 90, 0);
            time = 0;
        }

        // Update is called once per frame
        void Update()
        {
            checkTurn();
            if (index == playerLocation)
            {
                ani.SetBool("catching", true);
                catchPlayer();
            }
            else
            {
                ani.SetBool("catching", false);
                move();
                time += Time.deltaTime;
            }
            if (time >= 3f)
            {
                turn();
            }
        }

        void OnCollisionEnter(Collision e)
        {
            //撞击围栏，选择下一个点移动  
            if (e.gameObject.CompareTag("Wall"))
            {
                turn();
            }

            //撞击hero，游戏结束  
            if (e.gameObject.CompareTag("Player"))
            {
                e.gameObject.SetActive(false);
                catching = false;
            }
        }

        public void speedUp ()
        {
            catchSpeed += 0.1f;
        }

        int getNowLocation()
        {
            if (gameObject.transform.position.z > 5 && gameObject.transform.position.z < 15)
            {
                if (gameObject.transform.position.x < 0)
                {
                    return 3;
                }
                else
                {
                    return 6;
                }
            }
            else if (gameObject.transform.position.z > -5 && gameObject.transform.position.z < 5)
            {
                if (gameObject.transform.position.x < 0)
                {
                    return 2;
                }
                else
                {
                    return 5;
                }
            }
            else
            {
                if (gameObject.transform.position.x < 0)
                {
                    return 1;
                }
                else
                {
                    return 4;
                }
            }
        }
    }
```

 > - `SoldierFactory`
 >  
```
    public class SoldierFactory : MonoBehaviour
    {
        public GameObject soldierPrefab;
        private static List<GameObject> used = new List<GameObject>();
        private static List<GameObject> free = new List<GameObject>();

        void Start()
        {
        }

        public GameObject GetSoldier()
        {
            if (free.Count != 0)
            {
                used.Add(free[0]);
                free.RemoveAt(0);
                used[used.Count - 1].SetActive(true);
            }
            else
            {
                GameObject tempDisk = Instantiate(Resources.Load("Prefabs/Soldier"), Vector3.up, Quaternion.identity) as GameObject;
                used.Add(tempDisk);
                used[used.Count - 1].SetActive(true);
            }
            return used[used.Count - 1];
        }

        public void FreeSoldier(GameObject soldier)
        {
            soldier.SetActive(false);
            used.Remove(soldier);
            free.Add(soldier);
        }
    }
```

 > - `Data`
 >  
```
    public class Data : MonoBehaviour
    {
        private Subject sub;
        private GameObject player;
        private Observer obs;
        private SoldierFactory soldierFac;
        private int soldierNum;
        private List<GameObject> soldierSet = new List<GameObject>();
        private Vector3[] soldierPos = new Vector3[] {new Vector3(-7.5f, -0.5f, -10), new Vector3(-7.5f, -0.5f, 0), new Vector3(-7.5f, -0.5f, 10),
            new Vector3(7.5f, -0.5f, -10), new Vector3(7.5f, -0.5f, 0), new Vector3(7.5f, -0.5f, 10)};

        void Start()
        {
            soldierNum = 0;
            soldierFac = gameObject.AddComponent<SoldierFactory>() as SoldierFactory;
            Instantiate(Resources.Load("Prefabs/World"), new Vector3 (0, 0f, 0), Quaternion.identity);
            player = Instantiate(Resources.Load("Prefabs/Player"), new Vector3(1, -0.5f, 1), Quaternion.identity) as GameObject;
            sub = player.GetComponent<Player>();

            for (int i = 0; i < 6; i++)
            {
                GameObject soldier = soldierFac.GetSoldier();
                soldier.name = "Soldier" + soldierNum;
                soldierNum++;
                obs = soldier.GetComponent<Soldier>();
                sub.Attach(obs);
                soldier.transform.position = soldierPos[i];
            }
        }

        void levelUp()
        {
            foreach (GameObject temp in soldierSet)
            {
                Soldier tempSoldier = temp.GetComponent<Soldier>();
                tempSoldier.speedUp();
            }
        }

        private void Update()
        {
        }

        public void getObserver (SceneController obs)
        {
            Subject tempSub = player.GetComponent<Player>();
            tempSub.Attach(obs);
        }

        public void getObserver(ScoreManager obs)
        {
            Subject tempSub = player.GetComponent<Player>();
            tempSub.Attach(obs);
        }

        public GameObject getPlayer ()
        {
            return player;
        }
    } 
```

 > - `ScoreManager`
 >  
```
    public class ScoreManager : Observer
    {
        private int lastLoc;
        private int currentLoc;
        private int score;
        private bool playerIsLive;

        public int getScore()
        {
            return score;
        }

        public void addScore(int s)
        {
            if (playerIsLive)
            {
                score += s;
            }
        }

        public void resetScore()
        {
            score = 0;
        }

        void Awake()
        {
            playerIsLive = true;
            score = -1;
            lastLoc = -1;
            currentLoc = -1;
        }

        public override void Reaction(bool aLive, Vector3 playPosition, int playLocation)
        {
            playerIsLive = aLive;
            currentLoc = playLocation;
            if (currentLoc != lastLoc)
            {
                score++;
                lastLoc = currentLoc;
            }
        }
    }
```

 > - `UIController`
 >  
```
    public class UIController : MonoBehaviour
    {
        int score;
        int level;
        bool isOver;

        GUIStyle fontstyle = new GUIStyle();
        private void Start()
        {
            isOver = false;
            level = 1;
            fontstyle.fontSize = 50;
            fontstyle.normal.textColor = new Color(255, 255, 255);
            fontstyle.alignment = TextAnchor.MiddleCenter;
        }
        private void OnGUI()
        {
            if (isOver)
            {
                GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 100, 50), "Game Over", fontstyle);
            }
            if (score == -1)
            {
                GUI.Button(new Rect(100, 0, 100, 50), "Score: 0", fontstyle);

            }
            else
            {
                GUI.Button(new Rect(100, 0, 100, 50), "Score: " + score, fontstyle);
            }
            GUI.Button(new Rect(100, 50, 100, 50), "Level: " + level, fontstyle);
        }
        public void gameOver ()
        {
            isOver = true;
        }
        public void setScore(int _score)
        {
            score = _score;
        }
        public void setLevel(int _level)
        {
            level = _level;
        }
    }
```

 > - `SceneController`
 >  
```
    public class SceneController : Observer
    {
        int level;
        Data myData;
        private static SceneController instance;
        UIController UI;
        private Subject sub;
        public static SceneController getInstance()
        {
            if (instance == null)
                instance = new SceneController();
            return instance;
        }
        private bool isOver;
        ScoreManager scoreManager;
        public override void Reaction(bool aLive, Vector3 playPosition, int playLocation)
        {
            isOver = !aLive;
        }

        bool get = false;

        // Use this for initialization
        void Start()
        {
            int level = 1;
            isOver = false;
            scoreManager = gameObject.AddComponent<ScoreManager>() as ScoreManager;
            UI = gameObject.AddComponent<UIController>() as UIController;
            myData = gameObject.AddComponent<Data>() as Data;
        }

        // Update is called once per frame
        void Update()
        {
            if (!get)
            {
                myData.getObserver(this);
                myData.getObserver(scoreManager);
                get = !get;
            }
            int score = scoreManager.getScore();
            UI.setScore(score);
            if (score >= level * 5)
            {
                level++;
                UI.setLevel(level);
            }
            if (isOver)
            {
                UI.gameOver();
            }
        }
    }
```

 - 演示视频<br>
 
 ![enter image description here](http://imglf4.nosdn.127.net/img/S3F1ejdrdGNrNFZibThmNE5ibWpZR3JPeHVkcjk4L3pQYUlXSXowM1JIKzN6VjFlNlhHV3p3PT0.gif)

