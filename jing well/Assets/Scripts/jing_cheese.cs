using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jing_cheese : MonoBehaviour {
    
    public GUIStyle TipsStyle;
    private string[] play = {"O", "X", " "};        //存储玩家的表示方式，" "表示无玩家
    private int currentPlayer;                      //当前玩家，0为O玩家；1为X玩家
    private int[ , ] cheese = new int[3, 3];        //存储当前棋盘状态
    bool wrong_input;                               //若棋子下重复，则为正
    int current_step;                               //当前棋盘棋子数目
    int over;                                       //游戏是否有结果，1为一方胜出，2为平局，0为无结果

    private void Start()
    {
        reload();
        TipsStyle.contentOffset = new Vector2(96, 45);
        TipsStyle.alignment = TextAnchor.MiddleCenter;
        TipsStyle.fontSize = 15;

    }

    private void reload ()
    {
        currentPlayer = 0;
        current_step = 0;
        wrong_input = false;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                cheese[i, j] = 2;
            }
        }
        over = 0;
    }

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
}
