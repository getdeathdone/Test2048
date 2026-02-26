using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Test.Game2048.Cubes
{
    [RequireComponent(typeof(Renderer))]
    [RequireComponent(typeof(Rigidbody))]
    public class Cube : MonoBehaviour
    {
        private static readonly HashSet<Cube> ActiveCubesSet = new HashSet<Cube>();
        public static IReadOnlyCollection<Cube> ActiveCubes => ActiveCubesSet;

        public event Action<Cube, Cube, float> Collide;

        public bool IsKinematic { get; private set; }

        public int Number
        {
            get => _number;
            set => UpdateVisualNumber(_number = value);
        }
        private int _number;

        public Color Color
        {
            get => _color;
            set => UpdateVisualColor(_color = value);
        }
        private Color _color;

        [Header("References")]
        [SerializeField] private TextMeshPro[] _textNumbers;

        private Renderer _renderer;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            ActiveCubesSet.Add(this);
        }

        private void OnDisable()
        {
            ActiveCubesSet.Remove(this);
        }

        public void Initialize(int number, Color color)
        {
            Number = number;
            Color = color;
        }

        public void Push(Vector3 direction, float force)
        {
            _rigidbody.AddForce(direction * force, ForceMode.Impulse);
        }
        public void Rotate(Vector3 torque)
        {
            _rigidbody.AddTorque(torque, ForceMode.Impulse);
        }

        public void EnableKinematic()
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            IsKinematic = true;
        }
        public void DisableKinematic()
        {
            _rigidbody.constraints = RigidbodyConstraints.None;
            IsKinematic = false;
        }

        private void UpdateVisualColor(Color color)
        {
            _renderer.material.color = color;
        }
        private void UpdateVisualNumber(int number)
        {
            foreach (var textField in _textNumbers)
                textField.text = number.ToString();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Cube cube))
                OnCollideWithCube(cube, collision);
        }
        private void OnCollideWithCube(Cube cube, Collision collision)
        {
            if (cube.IsKinematic) return;

            if (cube.Number == Number)
                OnCollideWithSameNumberCube(cube, collision);
        }
        private void OnCollideWithSameNumberCube(Cube cube, Collision collision)
        {
            var directionToCube = (cube.transform.position - transform.position).normalized;
            var directedSpeed = Mathf.Abs(Vector3.Dot(collision.relativeVelocity, directionToCube));
            Collide?.Invoke(this, cube, directedSpeed);
        }
    }
}
