using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldierSpace.s
{
    public class Soldier : Observer
    {
        private Animator ani;
        private int index;
        private bool catching;
        private int playerLocation;
        private Vector3 playerPosition;
        private float walkSpeed;
        private float catchSpeed;
        private bool playAlive;
        private float time;


        // Use this for initialization
        void Start()
        {
            ani = GetComponent<Animator>();
            index = getOwnIndex();
            walkSpeed = 0.25f;
            catchSpeed = 0.27f;
        }

        private int getOwnIndex()
        {
            string name = this.gameObject.name;
            char tempIndex = name[name.Length - 1];
            int result = tempIndex - '0';
            return result + 1;
        }


        public override void Reaction (bool _aLive, Vector3 _playPosition, int _playLocation)
        {
            playerLocation = _playLocation;
            playerPosition = _playPosition;
            playAlive = _aLive;
        }

        void catchPlayer()
        {
            catching = true;
            transform.LookAt(playerPosition);
            transform.position = Vector3.Lerp(transform.position, playerPosition, catchSpeed * 3 * Time.deltaTime);
        }

        void move ()
        {
            transform.Translate(0, 0, walkSpeed * Time.deltaTime);
        }

        void checkTurn ()
        {
            if (index != getNowLocation())
            {
                turn();
            }
        }

        void turn ()
        {
            transform.Rotate(0, 90, 0);
            time = 0;
        }

        // Update is called once per frame
        void Update()
        {
            checkTurn();
            if (index == playerLocation)
            {
                ani.SetBool("catching", true);
                catchPlayer();
            }
            else
            {
                ani.SetBool("catching", false);
                move();
                time += Time.deltaTime;
            }
            if (time >= 3f)
            {
                turn();
            }
        }

        void OnCollisionEnter(Collision e)
        {
            //撞击围栏，选择下一个点移动  
            if (e.gameObject.CompareTag("Wall"))
            {
                turn();
            }

            //撞击hero，游戏结束  
            if (e.gameObject.CompareTag("Player"))
            {
                e.gameObject.SetActive(false);
                catching = false;
            }
        }

        public void speedUp ()
        {
            catchSpeed += 0.1f;
        }

        int getNowLocation()
        {
            if (gameObject.transform.position.z > 5 && gameObject.transform.position.z < 15)
            {
                if (gameObject.transform.position.x < 0)
                {
                    return 3;
                }
                else
                {
                    return 6;
                }
            }
            else if (gameObject.transform.position.z > -5 && gameObject.transform.position.z < 5)
            {
                if (gameObject.transform.position.x < 0)
                {
                    return 2;
                }
                else
                {
                    return 5;
                }
            }
            else
            {
                if (gameObject.transform.position.x < 0)
                {
                    return 1;
                }
                else
                {
                    return 4;
                }
            }
        }
    }
}
