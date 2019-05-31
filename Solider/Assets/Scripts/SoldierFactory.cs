using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldierSpace.s
{
    public class SoldierFactory : MonoBehaviour
    {
        public GameObject soldierPrefab;
        private static List<GameObject> used = new List<GameObject>();
        private static List<GameObject> free = new List<GameObject>();

        void Start()
        {
        }

        public GameObject GetSoldier()
        {
            if (free.Count != 0)
            {
                used.Add(free[0]);
                free.RemoveAt(0);
                used[used.Count - 1].SetActive(true);
            }
            else
            {
                GameObject tempDisk = Instantiate(Resources.Load("Prefabs/Soldier"), Vector3.up, Quaternion.identity) as GameObject;
                used.Add(tempDisk);
                used[used.Count - 1].SetActive(true);
            }
            return used[used.Count - 1];
        }

        public void FreeSoldier(GameObject soldier)
        {
            soldier.SetActive(false);
            used.Remove(soldier);
            free.Add(soldier);
        }
    }
}
