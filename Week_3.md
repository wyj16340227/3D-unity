# 简答并用程序验证
>* 游戏对象运动的本质是什么？
>> 游戏对象运动的本质是游戏对象方向、位置的不断变换。
>* 请用三种方法以上方法，实现物体的抛物线运动。（如，修改变换属性，使用向量的Vector3的方法...）
>> 1.由`(0, 0, 0)`出发，设置抛物线为`Y = X^2`，通过改变`position`实现，代码如下：
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour {

    private float speed;	//速度
    private float position;	//位置

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
	}
}
```
>> 2.通过`transform.Translate()`函数实现，其他相同，计算出增量为`2X`，代码如下：
```
	void Update () {
            position += speed * 2 * Time.deltaTime;
            transform.Translate(Vector3.up * position * Time.deltaTime);
            transform.Translate(Vector3.right * Time.deltaTime);
	}
```
>> 3.通过增加`tranform.position`实现，代码如下：
```
        void Update () {
            position += speed * 2 * Time.deltaTime;
            transform.position += new Vector3(Time.deltaTime, position * Time.deltaTime, 0);
	}
```
>* 写一个程序，实现一个完整的太阳系，其他星球围绕太阳的转速必须不一样，且不在一个法平面上。
>> 在网上查阅资料可知，8大行星和太阳之间的距离比例为（以地球为1.0）；<br>
水星 0.387<br>
金星0.723<br>
地球1.000<br>
火星1.524<br>
木星5.205<br>
土星9.576<br>
天王星19.18<br>
海王星30.13<br>

