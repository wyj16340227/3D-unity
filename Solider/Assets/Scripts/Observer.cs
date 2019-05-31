using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldierSpace.s
{
    public abstract class Observer : MonoBehaviour
    {
        public abstract void Reaction(bool aLive, Vector3 playPosition, int playLocation);

    }
}
