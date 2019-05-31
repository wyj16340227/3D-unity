using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class load_calculator : MonoBehaviour {

    double interest;
    double loan;
    int month;
    string inputLoan;
    string inputInterest;
    string inputMonth;
    bool compute;
    double result;
    bool clear;
    bool wrongInput;

    void reload ()
    {
        interest = 0f;          //利息
        loan = 0f;              //贷款金额
        month = 0;              //还款期限
        inputInterest = "";     //用户输入利息
        inputLoan = "";         //用户输入贷款金额
        inputMonth = "";        //用户输入还款期限
        compute = false;        //是否计算
        result = 0f;            //每月应还款
    }

    private void Start()
    {
        reload();
    }

    

    private void OnGUI()
    {
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
    }
}
