using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace backpack
{
    public class EquipmentManager : MonoBehaviour
    {
        public MouseImage mouseImage;       //鼠标
        private int hat = 0;                //头盔穿戴
        private int hand = 0;               //武器
        private int shoes = 0;              //足具

        void Start()
        {
        }

        public MouseImage getMouse()
        {
            return mouseImage;
        }

        public void reset()
        {
            hat = 0;
            hand = 0;
            shoes = 0;
        }

        public int getHat()
        {
            return hat;
        }



        public int getHand()
        {
            return hand;
        }

        public int getShoes()
        {
            return shoes;
        }

        public void setHat(int _hat)
        {
            hat = _hat;
        }


        public void setHand(int _hand)
        {
            hand = _hand;
        }


        public void setShoes(int _shoes)
        {
            shoes = _shoes;
        }
    }
}