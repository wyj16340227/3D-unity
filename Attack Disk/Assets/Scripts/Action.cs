
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Disk.Action
{
    public enum SSActionEventType : int { STARTED, COMPLETED }

    public interface ISSActionCallback
    {
        void SSEventAction(SSAction source, SSActionEventType events = SSActionEventType.COMPLETED,
            int intParam = 0, string strParam = null, GameObject objParam = null);
    }

    public class SSAction : ScriptableObject // 动作的基类
    {
        public bool enable = true;
        public bool destory = false;

        public GameObject gameObject { get; set; }
        public Transform transform { get; set; }
        public ISSActionCallback callback { get; set; }

        public virtual void Start()
        {
            throw new System.NotImplementedException("Action Start Error!");
        }

        public virtual void Update()
        {
            throw new System.NotImplementedException("Action Update Error!");
        }
    }

    public class FlyAction : SSAction
    {
        public Vector3 dirction;
        // 飞行方向
        private float time;
        // 已经飞行时间
        float down;

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

    public class BiuActionByForce : SSAction
    {
        public Vector3 dirction;
        // 已经飞行时间
        public float speed;
        // 刚体组建
        Rigidbody rig;

        public static BiuActionByForce GetSSAction(Vector3 _dirction, GameObject bullet)
        {
            BiuActionByForce currentAction = ScriptableObject.CreateInstance<BiuActionByForce>();
            currentAction.gameObject = bullet;
            currentAction.dirction = _dirction;
            return currentAction;
        }

        public override void Start()
        {
            speed = 100;
            rig = this.gameObject.GetComponent<Rigidbody>();
            rig.velocity = dirction * speed;
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

            if (Input.GetMouseButton(0))
            {

            }
        }

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

    public class SSActionManager : MonoBehaviour
    {
        private Dictionary<int, SSAction> dictionary = new Dictionary<int, SSAction>();
        private List<SSAction> watingAddAction = new List<SSAction>();
        private List<int> watingDelete = new List<int>();

        protected void Start()
        {

        }

        public void Update()
        {
            foreach (SSAction ac in watingAddAction) dictionary[ac.GetInstanceID()] = ac;
            watingAddAction.Clear();
            // 将待加入动作加入dictionary执行

            foreach (KeyValuePair<int, SSAction> dic in dictionary)
            {
                SSAction ac = dic.Value;
                if (ac.destory) watingDelete.Add(ac.GetInstanceID());
                else if (ac.enable) ac.Update();
            }
            // 如果要删除，加入要删除的list，否则更新

            foreach (int id in watingDelete)
            {
                SSAction ac = dictionary[id];
                dictionary.Remove(id);
                DestroyObject(ac);
            }
            watingDelete.Clear();
            // 将deletelist中的动作删除
        }

        public void runAction(GameObject gameObject, SSAction action, ISSActionCallback callback)
        {
            action.gameObject = gameObject;
            action.transform = gameObject.transform;
            action.callback = callback;
            watingAddAction.Add(action);
            action.Start();
        }
    }

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

    public class BulletFactory : MonoBehaviour
    {
        public GameObject bulletPrefab;
        private static List<GameObject> used = new List<GameObject>();
        private static List<GameObject> free = new List<GameObject>();

        void Start()
        {
        }

        public GameObject GetBullet()
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
                GameObject tempBullet = Instantiate(Resources.Load("Prefabs/Bullet"), Vector3.up, Quaternion.identity) as GameObject;
                int colorSet = Random.Range(0, 5);
                if (colorSet > 4)
                {
                    tempBullet.GetComponent<MeshRenderer>().material.color = Color.black;
                }
                else if (colorSet > 3)
                {
                    tempBullet.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
                else if (colorSet > 2)
                {
                    tempBullet.GetComponent<MeshRenderer>().material.color = Color.white;
                }
                else if (colorSet > 1)
                {
                    tempBullet.GetComponent<MeshRenderer>().material.color = Color.green;
                }
                else if (colorSet > 0)
                {
                    tempBullet.GetComponent<MeshRenderer>().material.color = Color.gray;
                }
                used.Add(tempBullet);
                used[used.Count - 1].SetActive(true);
            }
            return used[used.Count - 1];
        }

        public void StartPosition(GameObject bullet)
        {
        }

        public void FreeDisk(GameObject bullet)
        {
            bullet.SetActive(false);
            used.Remove(bullet);
            free.Add(bullet);
        }
    }
}