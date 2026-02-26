using System;
using Test.Game2048.Config;
using Test.Game2048.Cubes.Abstractions;
using Test.Game2048.Input;
using UnityEngine;
using Zenject;

namespace Test.Game2048.Cubes
{
    public class CubeSling : ICubeSling, ITickable
    {
        public event Action<Cube> Detached;

        private readonly CubeSlingData _data;
        private readonly IDragInputFactory _dragInputFactory;

        private Cube _cube;
        private Transform _cubeTransform;
        private IDragInput _touchInput;

        public CubeSling(CubeSlingData data, IDragInputFactory dragInputFactory)
        {
            _data = data;
            _dragInputFactory = dragInputFactory;
        }

        public void Tick()
        {
            _touchInput?.Tick();
        }

        public void Attach(Cube cube)
        {
            Reset();

            _cube = cube;
            _cubeTransform = cube.transform;
            _cube.EnableKinematic();

            _touchInput = _dragInputFactory.Create(_cubeTransform);
            _touchInput.Drag += OnDragCube;
            _touchInput.EndDrag += OnEndDragCube;
        }

        private void Detach()
        {
            ResetInput();

            var cube = _cube;
            _cube = null;
            _cubeTransform = null;

            cube.DisableKinematic();
            cube.Push(Vector3.forward, _data.PushForce);

            Detached?.Invoke(cube);
        }

        private void OnDragCube(Vector3 position)
        {
            var oldPosition = _cubeTransform.position;
            var clampedX = Mathf.Clamp(position.x, _data.MinX, _data.MaxX);
            var newPosition = new Vector3(clampedX, oldPosition.y, oldPosition.z);
            _cubeTransform.position = newPosition;
        }

        private void OnEndDragCube()
        {
            Detach();
        }

        public void Reset()
        {
            ResetInput();
            _cube = null;
            _cubeTransform = null;
        }

        private void ResetInput()
        {
            if (_touchInput == null)
                return;

            _touchInput.EndDrag -= OnEndDragCube;
            _touchInput.Drag -= OnDragCube;
            _touchInput = null;
        }
    }
}
