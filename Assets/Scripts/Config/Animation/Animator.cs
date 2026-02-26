using UnityEngine;

namespace Test.Game2048.Config.Animation
{
    public abstract class Animator<T> : ScriptableObject
    {
        public abstract void Animate(T target);
    }
}
