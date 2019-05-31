using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Disk.Action;

public enum GameStatues
{
    Pause,
    GameOver,
    Running,
    NextRound,
    WaitingForNextRound,
    Waiting,
    Counting,
    WaitingForGameOver
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
    public bool flyStyle;

    public int restDisk = 5;
	// Use this for initialization
	void Start () {
        scoreKeeper = gameObject.AddComponent<ScoreKeeper>() as ScoreKeeper;
        diskmanager = gameObject.AddComponent<DiskActionManager>() as DiskActionManager;
        diskmanager.GetScore(scoreKeeper);
        statues = GameStatues.Pause;

        fontstyle.fontSize = 50;
        fontstyle.normal.textColor = new Color(255, 255, 255);
        fontstyle.alignment = TextAnchor.MiddleCenter;
        GameObject player = Instantiate(Resources.Load("Prefabs/Player"), Vector3.up, Quaternion.identity) as GameObject;
        player.transform.rotation = Quaternion.Euler(0, 90, 0);
        player.transform.position = new Vector3(-5, -3, 0);
    }
	
	// Update is called once per frame
	void Update () {
		if (statues == GameStatues.Running)
        {
            if (restDisk > 0 && waitTime <= 0)
            {
                if (flyStyle)
                {
                    diskmanager.FlyDisk(scoreKeeper.GetRound());
                } else
                {
                    diskmanager.FlyDiskByForce(scoreKeeper.GetRound());
                }
                restDisk -= 1;
                waitTime = 4f;
            } else if (restDisk > 0 && waitTime > 0)
            {
                waitTime -= Time.deltaTime;
            } else if (restDisk <= 0)
            {
                if (scoreKeeper.GetRound() != MAX_ROUND)
                {
                    restDisk = 5;
                    statues = GameStatues.WaitingForNextRound;
                    waitTime = 2;

                } else
                {
                    statues = GameStatues.WaitingForGameOver;
                    waitTime = 2;
                }
            }
        } else if (statues == GameStatues.WaitingForNextRound)
        {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                statues = GameStatues.NextRound;
            }
        } else if (statues == GameStatues.WaitingForGameOver){
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                statues = GameStatues.GameOver;
            }
        }
	}

    public void OnGUI()
    {

        if (GUI.Button(new Rect(Screen.width - 300, 0, 100, 30), "Start", fontstyle))
        {
            time = 4f;
            statues = GameStatues.Counting;
            scoreKeeper.Reset();
        }
        if (GUI.Button(new Rect(Screen.width - 300, 50, 100, 30), "Change", fontstyle))
        {
            flyStyle = !flyStyle;
        }
        GUI.TextArea(new Rect(0, 0, 200, 50), "Round: " + scoreKeeper.GetRound().ToString(), fontstyle);
        GUI.TextArea(new Rect(0, 50, 200, 50), "Score: " + scoreKeeper.GetScore().ToString(), fontstyle);
        if (flyStyle)
        {
            GUI.TextArea(new Rect(0, 100, 200, 50), "Gymnastic", fontstyle);
        }
        else
        {
            GUI.TextArea(new Rect(0, 100, 200, 50), "Physics", fontstyle);
        }
        if (statues == GameStatues.Counting)
        {
            if (time >= 3)
            {
                GUI.TextArea(new Rect(Screen.width / 2 - 50, 100, 40, 40), "3", fontstyle);
            } else if (time >= 2)
            {
                GUI.TextArea(new Rect(Screen.width / 2 - 50, 100, 40, 40), "2", fontstyle);
            }
            else if (time >= 1)
            {
                GUI.TextArea(new Rect(Screen.width / 2 - 50, 100, 40, 40), "1", fontstyle);
            }
            else if (time >= 0)
            {
                GUI.TextArea(new Rect(Screen.width / 2 - 50, 100, 40, 40), "0", fontstyle);
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
            GUI.Button(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 100, 100, 30), "You Score Is: " + scoreKeeper.GetScore().ToString(), fontstyle);
        }
    }
}

public class ScoreKeeper : MonoBehaviour
{
    private int score;
    private int round;
    public static int MAX_ROUND = 3;

    public ScoreKeeper ()
    {
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
            round++;
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
