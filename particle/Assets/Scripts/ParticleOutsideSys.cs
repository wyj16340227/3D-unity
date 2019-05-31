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
