using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SoldierSpace.s
{
    public class Director : System.Object
    {
        //singleton instance
        private static Director instance;

        public SceneController currentScene;
        public bool running
        {
            get;
            set;
        }

        public static Director getInstance()
        {
            if (instance == null)
            {
                instance = new Director();
            }
            return instance;
        }
    }
}
