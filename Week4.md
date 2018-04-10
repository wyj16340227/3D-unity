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
>> 包含以下方法，均只改变物体状态，不进行运动。<br>
```
        public void Start()
        {   
            //左岸与右岸及树木
            GameObject tempObj =  Instantiate(Resources.Load("Prefabs/Shore"), leftShore, Quaternion.identity) as GameObject;
            tempObj.transform.rotation = Quaternion.Euler(0, 180, 0);
            Instantiate(Resources.Load("Prefabs/Shore"), rightShore, Quaternion.identity);
            Instantiate(Resources.Load("Prefabs/Tree"), new Vector3(20f, -1.5f, -3), Quaternion.identity);
            Instantiate(Resources.Load("Prefabs/Tree"), new Vector3(-20f, -1.5f, -3), Quaternion.identity);


            //船
            boatObject = Instantiate(Resources.Load("Prefabs/Boat"), boatStartPosition, Quaternion.identity) as GameObject;
            boatObject.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
            //牧师与魔鬼，全部放在右岸
            for (int i = 0; i < 3; ++i)
            {
                rightShoreDemon.Push(Instantiate(Resources.Load("Prefabs/Demon")) as GameObject);
                rightShorePriest.Push(Instantiate(Resources.Load("Prefabs/Priest")) as GameObject);
            }
            //灯
            Instantiate(Resources.Load("Prefabs/Light"));
            
        }

        //初始化
        public void Reload()
        {
            //牧师与魔鬼全部放入右岸
            while (leftShoreDemon.Count != 0)
            {
                rightShoreDemon.Push(leftShoreDemon.Pop());
            }
            while (leftShorePriest.Count != 0)
            {
                rightShorePriest.Push(leftShorePriest.Pop());
            }
            //牧师与魔鬼下船
            while (boat.Count != 0)
            {
                if (boat.Peek().tag == "Demon")
                {
                    rightShoreDemon.Push(boat.Pop());
                }
                else
                {
                    rightShorePriest.Push(boat.Pop());
                }
            }
            while (moving.Count() != 0)
            {
                if (moving.Peek().tag == "Demon")
                {
                    rightShoreDemon.Push(moving.Pop());
                }
                else
                {
                    rightShorePriest.Push(moving.Pop());
                }
            }
            boatObject.transform.position = boatStartPosition;
            boatObject.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
            boatPosition = true;
            isMoving = false;
        }

        //左岸魔鬼离开
        public bool LeftDemonToBoat()
        {
            if (leftShoreDemon.Count == 0 || boat.Count >= 2 || boatPosition)
            {
                return false;
            }
            else
            {
                moving.Push(leftShoreDemon.Pop());
                return true;
            }
        }

        //左岸牧师离开
        public bool LeftPriestToBoat()
        {
            if (leftShorePriest.Count == 0 || boat.Count >= 2 || boatPosition)
            {
                return false;
            }
            else
            {
                moving.Push(leftShorePriest.Pop());
                return true;
            }
        }

        //右岸魔鬼离开
        public bool RightDemonToBoat()
        {
            if (rightShoreDemon.Count == 0 || boat.Count >= 2 || !boatPosition)
            {
                return false;
            }
            else
            {
                moving.Push(rightShoreDemon.Pop());
                return true;
            }
        }

        //右岸牧师离开
        public bool RightPriestToBoat()
        {
            if (rightShorePriest.Count == 0 || boat.Count >= 2 || !boatPosition)
            {
                return false;
            }
            else
            {
                moving.Push(rightShorePriest.Pop());
                return true;
            }
        }

        //将moving栈中的对象弹出，压入船
        public void MoveToBoat ()
        {
            boat.Push(moving.Pop());
        }

        //将船中的对象弹出，压入移动队列
        public void DownBoat()
        {
            moving.Push(boat.Pop());
        }

        //得到船
        public GameObject GetBoat ()
        {
            return boatObject;
        }

        //牧师与魔鬼上岸
        public void MoveToShore()
        {
            if (boatPosition)
            {
                if (moving.Peek().tag == "Demon")
                {
                    rightShoreDemon.Push(moving.Pop());
                }
                else
                {
                    rightShorePriest.Push(moving.Pop());
                }
            }
            else
            {
                if (moving.Peek().tag == "Demon")
                {
                    leftShoreDemon.Push(moving.Pop());
                }
                else
                {
                    leftShorePriest.Push(moving.Pop());
                }
            }
        }

        //得到移动队列中的对象
        public GameObject getMoving ()
        {
            return moving.Peek();
        }

        //得到当前岸边的牧师数量
        public int GetPriestCount ()
        {
            if (boatPosition)
            {
                return rightShorePriest.Count();
            }
            else
            {
                return leftShorePriest.Count();
            }
        }

        //得到当前岸边的恶魔数量
        public int GetDemonCount()
        {
            if (boatPosition)
            {
                return rightShoreDemon.Count();
            }
            else
            {
                return leftShoreDemon.Count();
            }
        }

        //恶魔上船，通过判断船停靠岸决定哪个岸边恶魔上船
        public void DemonOnBoat ()
        {
            if(!boatPosition)
            {
                LeftDemonToBoat();
            }
            else
            {
                RightDemonToBoat();
            }
        }

        //牧师上船，通过判断船停靠岸决定哪个岸边牧师上船
        public void PriestOnBoat()
        {
            if (!boatPosition)
            {
                LeftPriestToBoat();
            }
            else
            {
                RightPriestToBoat();
            }
        }

        //判断，返回GameStatus.state
        public GameState Charge()
        {
            if (boatPosition)
            {
                int demonCount = rightShoreDemon.Count;
                int priestCount = rightShorePriest.Count;
                GameObject[] boatArray = boat.ToArray();
                for (int i = 0; i < boat.Count; i++)
                {
                    if (boatArray[i].tag == "Demon")
                    {
                        demonCount++;
                    }
                    else
                    {
                        priestCount++;
                    }
                }
                if (demonCount > priestCount && priestCount != 0)
                {
                    return GameState.GameOver;
                }
                if (priestCount > demonCount && priestCount != 3)
                {
                    return GameState.GameOver;
                }
            }
            else
            {
                int demonCount = leftShoreDemon.Count;
                int priestCount = leftShorePriest.Count;
                GameObject[] boatArray = boat.ToArray();
                for (int i = 0; i < boat.Count; i++)
                {
                    if (boatArray[i].tag == "Demon")
                    {
                        demonCount++;
                    }
                    else
                    {
                        priestCount++;
                    }
                }
                if (demonCount > priestCount && priestCount != 0)
                {
                    return GameState.GameOver;
                }
                if (priestCount > demonCount && priestCount != 3)
                {
                    return GameState.GameOver;
                }
            }
            if (rightShorePriest.Count == 3 && leftShoreDemon.Count == 3)
            {
                return GameState.Win;
            }
            return GameState.Waiting;
        }

        //得到船停留岸边
        public bool getBoatPosition ()
        {
            return boatPosition;
        }

        //构造视图
        public void SetView()
        {
            GameObject[] leftDemonArray = leftShoreDemon.ToArray();
            for (int i = 0; i < leftShoreDemon.Count; ++i)
            {
                leftDemonArray[i].transform.position = leftDemonPosition +  new Vector3(-1.5f * i, 0, 0);
                leftDemonArray[i].transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            }

            GameObject[] leftPriestArray = leftShorePriest.ToArray();
            for (int i = 0; i < leftShorePriest.Count; ++i)
            {
                leftPriestArray[i].transform.position = leftPriestPosition + new Vector3(-1.5f * i, 0, 0);
                leftPriestArray[i].transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            }

            GameObject[] rightDemonArray = rightShoreDemon.ToArray();
            for (int i = 0; i < rightShoreDemon.Count; ++i)
            {
                rightDemonArray[i].transform.position = rightDemonPosition + new Vector3(1.5f * i, 0, 0);
                rightDemonArray[i].transform.rotation = Quaternion.Euler(0f, 270f, 0f);
            }

            GameObject[] rightPriestArray = rightShorePriest.ToArray();
            for (int i = 0; i < rightShorePriest.Count; ++i)
            {
                rightPriestArray[i].transform.position = rightPriestPosition + new Vector3(1.5f * i, 0, 0);
                rightPriestArray[i].transform.rotation = Quaternion.Euler(0f, 270f, 0f);
            }

            if (boat.Count != 0)
            {
                GameObject[] boatArray = boat.ToArray();
                boatArray[0].transform.position = boatObject.transform.position + new Vector3(1f, 0.5f, 0f);
                if (boat.Count == 2)
                {
                    boatArray[1].transform.position = boatObject.transform.position + new Vector3(-1f, 0.5f, 0f);
                }
            }
        }
        
        //船掉头
        public void SetPosition ()
        {
            boatPosition = !boatPosition;
            if (boatPosition)
            {
                boatObject.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
            } else
            {
                boatObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            }
        }

        //得到船上乘客数量
        public int BoatCount ()
        {
            return boat.Count;
        }
    }
```
>>
>> 3.`Action`<br>
>> 设置函数`act()`，调用该函数则传递参数，根据参数得到的对象及状态判断要执行的操作。设置`bool`变量`run`，用来标识当前是否在进行运动，当在进行运动的时候，拒绝`Controller`的函数调用（不改变参数），若没有运动，接受传递的参数，改变运动状态并开始运动。运动完成后再次将`run`置为`False`。<br>
```
        Vector3 boatStartPosition = new Vector3(4f, -3, -3);                        //以下为各个参考位置
        Vector3 boatEndPosition = new Vector3(-4f, -3, -3);
        Vector3 leftShore = new Vector3(-15, -2, -3);
        Vector3 rightShore = new Vector3(15, -2, -3);
        Vector3 boatCurrentPosition;
        Vector3 leftDemonPosition = new Vector3(-8f, -1.5f, -3);
        Vector3 leftPriestPosition = new Vector3(-12.5f, -1.5f, -3);
        Vector3 rightDemonPosition = new Vector3(8f, -1.5f, -3);
        Vector3 rightPriestPosition = new Vector3(12.5f, -1.5f, -3);

        GameObject obj;
        public float speed;         //速度
        bool boatPosition;          //船在哪边岸上
        int person;                 //船上的人数
        bool onBoat;                //人是否在船上
        bool run;                   //是否正在运动
        bool boatRorate;            //是否反转
        bool moveToShore;           //是否移动到岸边
```
>> 以下为调用函数。<br>
```
        //初始化
        public Action()
        {
            run = false;
            moveToShore = true;
            boatRorate = false;
            speed = 5;
        }

        //返回当前运动状态
        public bool Run()
        {
            return run;
        }
        
        //得到当前运动，如果正在运动，则拒绝，否则接受
        public void Act(GameObject _obj, bool _boatPosition, int _person, bool _onBoat)
        {
            if (run)
            {
                return;
            }
            run = true;
            obj = _obj;
            boatPosition = _boatPosition;
            person = _person;
            onBoat = _onBoat;
        }

        // Update is called once per frame
        void Update()
        {
            if (!run)
            {
                return;
            }
            //船移动
            if (obj.tag == "Boat") 
            {

                Vector3 target = boatStartPosition;
                if (boatPosition)
                {
                    target = boatEndPosition;
                }
                obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, speed * Time.deltaTime);
                if (obj.transform.position == target)
                {
                    run = false;
                }
            }
            //对象上船
            else if (obj.tag != "Boat" && !onBoat)
            {
                Vector3 target;
                if (boatPosition)
                {
                    target = rightDemonPosition;
                }
                else
                {
                    target = leftDemonPosition;
                }
                //对象到岸边
                if (moveToShore)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, speed * Time.deltaTime);
                    if (obj.transform.position == target)
                    {
                        moveToShore = false;
                    }
                }
                //对象上船
                if (!moveToShore)
                {
                    if (boatPosition)
                    {
                        target = boatStartPosition;
                    }
                    else
                    {
                        target = boatEndPosition;
                    }
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, speed * Time.deltaTime);
                    if (obj.transform.position == target)
                    {
                        run = false;
                        moveToShore = true;
                    }
                }
            }
            //对象上岸
            else
            {
                Vector3 target = leftDemonPosition;
                if (boatPosition)
                {
                    target = rightDemonPosition;
                }
                obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, speed * Time.deltaTime);
                if (obj.transform.position == target)
                {
                    run = false;
                }
            }
        }
```
>>* 4.
