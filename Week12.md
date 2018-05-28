# **_题目要求_**
--------

## 本次作业基本要求是 _三选一_

### 1、简单粒子制作

-   按参考资源要求，制作一个粒子系统，[参考资源](http://www.cnblogs.com/CaomaoUnity3d/p/5983730.html)
-   使用 3.3 节介绍，用代码控制使之在不同场景下效果不一样

### 2、完善官方的“汽车尾气”模拟

-   使用官方资源资源 Vehicle 的 car， 使用 Smoke 粒子系统模拟启动发动、运行、故障等场景效果

### 3、参考 http://i-remember.fr/en 这类网站，使用粒子流编程控制制作一些效果， 如“粒子光环”

-   可参考以前作业
<br>

## 选做第三题
> **_创造一个行星带_**
>> 中央为金星，外面环绕两个行星带，内行星带逆时针旋转，外行星带顺时针旋转，每个行星带有1000颗行星。<br>
------
> **_创作过程_**
> - 设置背景<br>
>> 因为是行星，所以是在夜空中，因此我们的背景要设置成黑色。创造一个纯黑色的天空盒，天空盒的贴图使用6张纯黑色的图片。<br>
<br>

![](http://imglf4.nosdn.127.net/img/S3F1ejdrdGNrNFhHakZvcy8xYjlpZEtvTVhseVhIalgxRjBBbFg4QmhjcEI2ZmlVa093Z1hnPT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0)

<br>

> - 创建中心天体<br>
>> 在网上搜索贴图，创建新的行星，贴上贴图，编写C#文件，控制行星的自转。将C#脚本挂载上对象，并将对象设置为预制。
<br>

![](http://imglf5.nosdn.127.net/img/S3F1ejdrdGNrNFhHakZvcy8xYjlpWkw1dUFEK095ZWQzYTd0UnJCMmFqcndsdHEvNGRIdXdRPT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0)

<br>

> - 设置粒子<br>
>> 在网上搜索行星的贴图，贴在粒子上，因为是行星带，因此形状要设置成圆形，先将`Randerer` 设置为`Mesh`，再设置`Sphere`。<br>
<br>

![](http://imglf5.nosdn.127.net/img/S3F1ejdrdGNrNFhHakZvcy8xYjlpYktUYUl6Yll0STlyVG5MUll0Y1NCaGJWSFZkdjdka0lBPT0.png?imageView&thumbnail=500x0&quality=96&stripmeta=0)

<br>

> - 粒子信息类`ParticleMes`<br>
>> 该类存储粒子应有的信息，主要包括粒子的半径、角度（代码见文末）。<br>
<br>
```
    private float radius = 0f;      // 半径
    private float angle = 0f;       // 角度 
```
<br>

> - 粒子系统`ParticleSys`<br>
>> 该类控制整个粒子系统，包含三个重要的变量：粒子系统、粒子数组、粒子信息数组。<br>
<br>
```
    private ParticleSystem particleSys;             //粒子系统  
    private ParticleSystem.Particle[] particle;     //粒子数组  
    private ParticleMes[] particleMes;              //粒子信息

    public int particleNum;         //粒子数量 
    public float min;               //内径  
    public float max;               //外径  
    public float speed;             //速度
```
<br>

> - 粒子初始化<br>
>> 随机生成粒子的构造数据，希望粒子能够在均匀分布在内外径之间，因此使用了随机生成的方法生成粒子的半径；与此同时，随机生成了粒子的角度。<br>
<br>
```
            //粒子位置在内外径之内  
            float midRadius = (min + max) / 2;
            float minRate = Random.Range(1.0f, midRadius / min);
            float maxRate = Random.Range(midRadius / max, 1.0f);
            float radius = Random.Range(min * minRate, max * maxRate);

            //角度  
            float angle = Random.Range(0.0f, 360.0f);
            float theta = angle / 180 * Mathf.PI;
```
<br>

> - 粒子的运动<br>
>> 更改粒子的角度实现粒子的运动（内行星带顺时针，外行星带逆时针），参照网上的方法，增加了层次变量，将行星带中的行星分为5层，为所有的行星均匀的设置了速度，使外围的运动更慢，内围的运动的更快。<br>
<br>
```
            particleMes[i].angle += (i % tier + 1) * (speed / particleMes[i].radius / tier);
            particleMes[i].angle = (360.0f + particleMes[i].angle) % 360.0f;
```
<br>

> - 中心天体的运动<br>
>> 实现很简单，绕着法平面转就可以了。<br>
<br>
```
		this.gameObject.transform.Rotate(new Vector3(1, 2, 0), rotateSpeed * Time.deltaTime);
```
<br>

------
> 实现效果<br>
小图：<br>
<br>
![enter image description here](http://imglf3.nosdn.127.net/img/S3F1ejdrdGNrNFhHakZvcy8xYjlpYVAzcWZpNDlmVGtnelZxUmRKYlB6emp6YUNLenhvOWlBPT0.gif)
<br>
大图：<br>
<br>
![enter image description here](http://imglf3.nosdn.127.net/img/S3F1ejdrdGNrNFhHakZvcy8xYjlpUkE1RmtRYVcrS2h0QUVqZnBXVTg3MWJpM00vOUN2UW5nPT0.gif)
<br>
-------
> **_源代码_**
> - `StarMove`<br>
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMove : MonoBehaviour {

    private float revolutionSpeed;
    private float rotateSpeed;

	// Use this for initialization
	void Start () {
        rotateSpeed = 10f;
    }
	
	// Update is called once per frame
	void Update () {
		this.gameObject.transform.Rotate(new Vector3(1, 2, 0), rotateSpeed * Time.deltaTime);
    }
}

```
> - `ParticleMes`<br>
```
public class ParticleMes
{
    public float radius = 0f;      //半径
    public float angle = 0f;       //角度
    public ParticleMes (float _radius, float _angle)
    {
        radius = _radius;  
        angle = _angle;
    }
}
```
> - `ParticleInsideSys`<br>
```
public class ParticleInsideSys : MonoBehaviour {

    private ParticleSystem particleSys;             //粒子系统  
    private ParticleSystem.Particle[] particle;     //粒子数组  
    private ParticleMes[] particleMes;              //粒子信息

    public int particleNum;         //粒子数量 
    public float min;               //内径  
    public float max;               //外径  
    public float speed;             //速度

    // Use this for initialization
    void Start () {
        particleNum = 1000;
        min = 6f;
        max = 9f;
        speed = 5f;

        // 初始化粒子数组  
        particle = new ParticleSystem.Particle[particleNum];
        particleMes = new ParticleMes[particleNum];

        // 初始化粒子系统  
        particleSys = this.GetComponent<ParticleSystem>();
        particleSys.startSpeed = 0;                 //粒子位置由程序控制
        particleSys.startSize = 0.03f;              //设置粒子大小
        particleSys.loop = true;                    //不断循环
        particleSys.maxParticles = particleNum;     //设置粒子量
        particleSys.Emit(particleNum);              //发射粒子
        particleSys.GetParticles(particle);

        for (int i = 0; i < particleNum; ++i)
        {
            //粒子位置在内外径之内   
            float midRadius = (min + max) / 2;
            float minRate = Random.Range(1.0f, midRadius / min);
            float maxRate = Random.Range(midRadius / max, 1.0f);
            float radius = Random.Range(min * minRate, max * maxRate);

            //角度  
            float angle = Random.Range(0.0f, 360.0f);
            float theta = angle / 180 * Mathf.PI;

            particleMes[i] = new ParticleMes(radius, angle);

            particle[i].position = new Vector3(particleMes[i].radius * Mathf.Cos(theta), 0f, particleMes[i].radius * Mathf.Sin(theta));
        }

        particleSys.SetParticles(particle, particleNum);
    }
	
	// Update is called once per frame
	void Update () {
        int tier = 5;
        for (int i = 0; i < particleNum; i++)
        {
            particleMes[i].angle -= (i % tier + 1) * (speed / particleMes[i].radius / tier);
            particleMes[i].angle = (360.0f + particleMes[i].angle) % 360.0f;
            float theta = particleMes[i].angle / 180 * Mathf.PI;

            particle[i].position = new Vector3(particleMes[i].radius * Mathf.Cos(theta), 0f, particleMes[i].radius * Mathf.Sin(theta));
        }
        particleSys.SetParticles(particle, particleNum);
    }
}
```
> - `ParticleOutsideSys`<br>
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOutsideSys : MonoBehaviour
{

    private ParticleSystem particleSys;             //粒子系统  
    private ParticleSystem.Particle[] particle;     //粒子数组  
    private ParticleMes[] particleMes;              //粒子信息

    public int particleNum;         //粒子数量 
    public float min;               //内径
    public float max;               //外径  
    public float speed;             //速度 

    // Use this for initialization
    void Start()
    {
        particleNum = 1000;
        min = 5f;
        max = 7f;
        speed = 5f;

        // 初始化粒子数组  
        particle = new ParticleSystem.Particle[particleNum];
        particleMes = new ParticleMes[particleNum];

        // 初始化粒子系统  
        particleSys = this.GetComponent<ParticleSystem>();
        particleSys.startSpeed = 0;                 //粒子位置由程序控制
        particleSys.startSize = 0.03f;              //设置粒子大小
        particleSys.loop = true;                    //不停循环
        particleSys.maxParticles = particleNum;     //设置粒子量
        particleSys.Emit(particleNum);              //发射粒子
        particleSys.GetParticles(particle);

        for (int i = 0; i < particleNum; ++i)
        {
            //粒子位置在内外径之内   
            float midRadius = (min + max) / 2;
            float minRate = Random.Range(1.0f, midRadius / min);
            float maxRate = Random.Range(midRadius / max, 1.0f);
            float radius = Random.Range(min * minRate, max * maxRate);

            //角度  
            float angle = Random.Range(0.0f, 360.0f);
            float theta = angle / 180 * Mathf.PI;

            particleMes[i] = new ParticleMes(radius, angle);

            particle[i].position = new Vector3(particleMes[i].radius * Mathf.Cos(theta), 0f, particleMes[i].radius * Mathf.Sin(theta));
        }

        particleSys.SetParticles(particle, particleNum);
    }

    // Update is called once per frame
    void Update()
    {
        int tier = 5;
        for (int i = 0; i < particleNum; i++)
        {
            particleMes[i].angle += (i % tier + 1) * (speed / particleMes[i].radius / tier);
            particleMes[i].angle = (360.0f + particleMes[i].angle) % 360.0f;
            float theta = particleMes[i].angle / 180 * Mathf.PI;

            particle[i].position = new Vector3(particleMes[i].radius * Mathf.Cos(theta), 0f, particleMes[i].radius * Mathf.Sin(theta));
        }
        particleSys.SetParticles(particle, particleNum);
    }
}

```
> - `SceneController`<br>
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Instantiate(Resources.Load("Prefabs/Sun"), new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(Resources.Load("Prefabs/HaloIn"), new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(Resources.Load("Prefabs/HaloOut"), new Vector3(0, 0, 0), Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
```
