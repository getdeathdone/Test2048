using UnityEngine;

namespace Test.Game2048.Input
{
    public interface IDragInputFactory
    {
        IDragInput Create(Transform target);
    }
}

