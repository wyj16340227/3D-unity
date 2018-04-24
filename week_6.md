# **_<center>改进飞碟（Hit UFO）游戏</center>_**
> - 游戏内容要求：<br>
> - - 按 adapter模式 设计图修改飞碟游戏<br>
> - - 使它同时支持物理运动与运动学（变换）运动<br>


------------
> - 
## **_<center>adaptor模式</center>_**<br>
> 
>> - 在本题中，实现了物理学飞行`FlyActionByForce`与运动学飞行`FlyAction`的适配。以`FlyActionByForce`适配`FlyAction`。<br>
>
>> 通过构造`FlyAction`的子类`TranslateFlyAction`中包含`FlyActionByForce`的实例，并重载`FlyAction`的方法，调用`FlyActionByForce`的方法来实现。<br>
>> 通过显示传参将`gameObject`传进构造函数之中，以实现用`FlyActionByForce`的方法来更改`Disk`的属性，并以`TranslateFlyAction`（`FlyAction`）的子类来实现动作。<br>
>> 构造适配器如下：<br>
```
    public class TranslateFlyAction : FlyAction
    {
        private FlyActionByForce forcefly;

        new
        public static FlyAction GetSSAction(Vector3 _dirction, GameObject disk)
        {
            TranslateFlyAction currentAction = ScriptableObject.CreateInstance<TranslateFlyAction>();
            currentAction.dirction = _dirction;
            currentAction.gameObject = disk;
            currentAction.forcefly = new FlyActionByForce();
            currentAction.forcefly.gameObject = disk;
            currentAction.forcefly.dirction = _dirction;
            currentAction.forcefly.transform = disk.transform;
            Debug.Log(currentAction.gameObject.name);
            Debug.Log(currentAction.forcefly.gameObject.name);
            return currentAction;
        }

        public override void Start()
        {
            forcefly.Start();
            forcefly.callback = callback;
        }

        public override void Update()
        {
            forcefly.Update();
        }
    }
```
>> 适配器将`FlyActionByForce`适配为`FlyAction`，这样就可以通过使用`TranslateFlyAction`来实现。调用语句如下：<br>
```
FlyAction fly = TranslateFlyAction.GetSSAction(new Vector3(Random.Range(5f, 20), Random.Range(2.5f, 10), Random.Range(0, 3f)), disk);
```

--------
## **_<center>两种实现</center>_**<br>
### **_<center>通过物理学来实现</center>_**<br>
> 
>> 通过两个变量来实现，一个是`Rigidbody`中自带的`use Gravity`来实现飞碟的下落，另一个是通过`Rigidbody.velocity`来为飞碟添加一个初速度。<br>

```
    public class FlyActionByForce : SSAction
    {
        public Vector3 dirction;
        // 飞行方向
        private float time;
        // 已经飞行时间
        // 刚体组建
        Rigidbody rig;

        public static FlyActionByForce GetSSAction(Vector3 _dirction, GameObject disk)
        {
            FlyActionByForce currentAction = ScriptableObject.CreateInstance<FlyActionByForce>();
            currentAction.gameObject = disk;
            currentAction.dirction = _dirction;
            currentAction.time = 0;
            return currentAction;
        }

        public override void Start()
        {
            rig = this.gameObject.GetComponent<Rigidbody>();
            rig.velocity = dirction;
            // 各个方向的匀速运动
            this.gameObject.GetComponent<Rigidbody>().useGravity = true;
            // 竖直方向的匀加速运动
        }

        public override void Update()
        {
            if (!this.gameObject.activeSelf) // 如果飞碟已经回收
            {
                this.destory = true;
                this.callback.SSEventAction(this, SSActionEventType.STARTED);
                return;
            }
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

### **_<center>通过运动学来实现</center>_**<br>
>
>> 实现方法与之前类似，唯一一点不同是之前的`Disk`不具有`Rigidbody`属性，移动完全通过运动学来实现，但现在为了物理学实现而添加了`Rigidbody` 并且使用了`use Gravity`，因此，需要在`Start()`函数中得到`gameObject.Rigidbody`并将`useGravity`变量设置为`false`。<br>
```
        public static FlyAction GetSSAction(Vector3 _dirction, GameObject disk)
        {
            FlyAction currentAction = ScriptableObject.CreateInstance<FlyAction>();
            currentAction.gameObject = disk;
            currentAction.dirction = _dirction;
            currentAction.time = 0;
            return currentAction;
        }

        public override void Start()
        {
            down = 5;
            this.gameObject.GetComponent<Rigidbody>().useGravity = false;
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

### **_<center>接口切换的实现</center>_**<br>
>> 通过实现另一个函数`FlyDiskByForce`，设置变量`flyStyle`记录当前飞行模式，从而调用不同函数。<br>
```
        public void FlyDisk(int num)
        {
            GameObject disk;
            for (int i = 0; i < num; i++)
            {
                disk = factory.GetDisk();
                FlyAction fly = FlyAction.GetSSAction(new Vector3(Random.Range(5f, 20), Random.Range(2.5f, 10), Random.Range(0, 3f)), disk);
                this.runAction(disk, fly, this);
            }
        }

        public void FlyDiskByForce(int num)
        {
            GameObject disk;
            for (int i = 0; i < num; i++)
            {
                disk = factory.GetDisk();
                FlyAction fly = TranslateFlyAction.GetSSAction(new Vector3(Random.Range(5f, 20), Random.Range(2.5f, 10), Random.Range(0, 3f)), disk);
                this.runAction(disk, fly, this);
            }
        }
```
[演示视频](https://pan.baidu.com/s/1zlTKH3u080ayWEBtQqIO_g)
