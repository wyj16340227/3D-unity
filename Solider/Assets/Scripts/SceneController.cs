using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldierSpace.s
{
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
}

