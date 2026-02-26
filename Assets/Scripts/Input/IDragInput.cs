using System;
using UnityEngine;

namespace Test.Game2048.Input
{
    public interface IDragInput
    {
        event Action<Vector3> Drag;
        event Action EndDrag;

        void Tick();
    }
}

