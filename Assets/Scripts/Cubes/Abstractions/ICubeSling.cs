using System;

namespace Test.Game2048.Cubes.Abstractions
{
    public interface ICubeSling
    {
        event Action<Cube> Detached;
        void Attach(Cube cube);
        void Reset();
    }
}

