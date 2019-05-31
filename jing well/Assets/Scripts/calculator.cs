using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calculator : MonoBehaviour {

    bool plus;
    bool subtract;
    bool multiply;
    bool divide;
    double result;
    double operator1;
    double operator2;
    string inputOperator1;
    string inputOperator2;
    bool compute;
    string[,] calculate = { { "C", "7", "4", "1", "0" }, { "-", "8", "5", "2", "."}, {"%", "9", "6", "3", "D" }, { "/", "*", "-", "+", "="}};
    string input;
    void reload ()
    {
        plus = false;
        subtract = false;
        multiply = false;
        divide = false;
        result = 0;
        operator1 = 0;
        operator2 = 0;
        inputOperator1 = "";
        inputOperator2 = "";
        compute = false;
        input = "";
    }

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                print(calculate[i, j]);
            }
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 20), "计算器");
        GUI.Label(new Rect(100, 80, 50, 20), input);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (GUI.Button(new Rect(100 + 30 * i, 100 + 30 * j, 30, 30), calculate[i, j]))
                {
                    if (i == 0 && j == 4)
                    {
                        input = input + "0";
                    } else if (i == 0 && j == 3)
                    {
                        input = input + "1";
                    } else if (i == 1 && j == 3)
                    {
                        input = input + "2";
                    } else if (i == 2 && j == 3)
                    {
                        input = input + "3";
                    } else if (i == 0 && j == 2)
                    {
                        input = input + "4";
                    } else if (i == 1 && j == 2)
                    {
                        input = input + "5";
                    } else if (i == 2 && j == 2)
                    {
                        input = input + "6";
                    } else if (i == 0 && j == 1)
                    {
                        input = input + "7";
                    } else if (i == 1 && j == 1)
                    {
                        input = input + "8";
                    } else if (i == 2 && j == 1)
                    {
                        input = input + "9";
                    } else if (i == 0 && j == 0)
                    {
                        reload();
                    } else if (i == 1 && j == 0)
                    {
                        if (string.Equals(input, ""))
                        {
                            input = input + "-";
                        } else 
                        {
                            if (operator1 == 0)
                            {
                                if (operator2 == 0)
                                {
                                    operator1 = double.Parse(input);
                                    input = "";
                                } else
                                {

                                }
                            } else
                            {
                                operator2 = double.Parse(input);
                                input = "";
                            }
                            subtract = true;
                        }
                    }
                }
            }
        }
    }
}
