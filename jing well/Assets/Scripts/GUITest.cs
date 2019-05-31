using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUITest : MonoBehaviour {

    string str;
    private void Start()
    {
        str = "";
    }
    void OnGUI()
    {
        /*str = GUILayout.TextField(str, GUILayout.Width(100));
        print(str);*/

        if (GUILayout.Button("hello", GUILayout.Width(100)))
        {
            print("123");
        }
    }
}
