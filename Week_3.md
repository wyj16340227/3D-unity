# 简答并用程序验证
>* 游戏对象运动的本质是什么？
>> 游戏对象运动的本质是游戏对象方向、位置的不断变换。
>* 请用三种方法以上方法，实现物体的抛物线运动。（如，修改变换属性，使用向量的Vector3的方法...）
>> 1.由(0, 0, 0)出发，设置抛物线为`y = x^2`代码如下：
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour {

    private float speed;
    private float position;

	// Use this for initialization
	void Start () {
        speed = 1f;
        position = 0;
        transform.position = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        position += speed * Time.deltaTime;
        transform.position = new Vector3(position, position * position, 0);
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}
}
```
>> 2.
>> 3.
>* 写一个程序，实现一个完整的太阳系，其他星球围绕太阳的转速必须不一样，且不在一个法平面上。
>> 
