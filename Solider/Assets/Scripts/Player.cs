using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldierSpace.s {
    public class Player : Subject
    {
        int loc;
        private float speed;
        private float angle_speed;
        private bool isLive;
        private Vector3 position;

        // Use this for initialization
        void Start()
        {
            speed = 5f;
            angle_speed = 10f;
            isLive = true;
        }
        protected List<Observer> obs = new List<Observer>();   //所有观察者

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

        public override void Attach(Observer o)
        {
            obs.Add(o);
        }

        public override void Detach(Observer o)
        {
            obs.Remove(o);
        }

        public override void Notify(bool aLive, Vector3 playPosition, int playLocation)
        {
            foreach (Observer o in obs)
            {
                o.Reaction(isLive, playPosition, playLocation);
            }
        }
        private void Update()
        {
            //move
            transform.Translate(Vector3.forward * speed * Input.GetAxisRaw("Vertical") * Time.deltaTime);
            transform.Translate(Vector3.right * speed * Input.GetAxisRaw("Horizontal") * Time.deltaTime);
            transform.Rotate(Vector3.up, Input.GetAxisRaw("Vertical") * angle_speed * Time.deltaTime);
            //set rotation to make player at floor
            float y = transform.rotation.y;
            transform.rotation = Quaternion.Euler(0, y, 0);
            float x = transform.position.x;
            float z = transform.position.z;
            transform.position = new Vector3(x, -0.5f, z);
            loc = getNowLocation();
            Notify(isLive, transform.position, loc);
        }

        private void OnCollisionEnter(Collision c)
        {
            if (c.gameObject.CompareTag("Soldier"))
            {
                isLive = false;
                Notify(isLive, transform.position, loc);
            }
        }
    }
}
