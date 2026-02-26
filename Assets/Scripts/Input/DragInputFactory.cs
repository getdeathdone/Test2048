using UnityEngine;

namespace Test.Game2048.Input
{
    public class DragInputFactory : IDragInputFactory
    {
        public IDragInput Create(Transform target)
        {
            return new DragTouchInput(target);
        }
    }
}

