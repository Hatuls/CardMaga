using CardMaga.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.LiveObjects.Spaceships
{
    public class Spaceship : MonoBehaviour, ICheckValidation
    {
        [Sirenix.OdinInspector.ReadOnly] [SerializeField] float _speed;
        [Sirenix.OdinInspector.ReadOnly] [SerializeField] float _acceleration;
        [Sirenix.OdinInspector.ReadOnly] [SerializeField] bool _canMove;
        [SerializeField] GameObject _spaceshipHolder;
        [SerializeField] SpriteRenderer _spaceshipSpriteRenderer;
        [SerializeField] Jet _jet;
        [Sirenix.OdinInspector.ReadOnly] [SerializeField] public Path Path;
        public event Action<Spaceship> OnReachedDestination;
        public Vector3 CurrentDirection;
        public bool IsLastPartOfPath = false;
        public int NextWaypoint;

        private void Start()
        {
            CheckValidation();
            _spaceshipHolder.SetActive(false);
        }
        public void CheckValidation()
        {
            _canMove = false;
            if (_jet == null)
                throw new System.Exception("Spaceship Has no Jet");
            if (Path == null)
                throw new System.Exception("Spaceship Has no Path");
            if (_spaceshipSpriteRenderer == null)
                throw new System.Exception("Spaceship Has no Sprite Renderer");
            if (_spaceshipHolder == null)
                throw new System.Exception("Spacship Has no Holder");
        }
        public void Init(Path path, Sprite spaceshipSprite, Sprite jetSprite, float speed, float acceleration)
        {
            Path = path;
            transform.position = Path.FirstWaypoint.position;
            Path.TryGetNextWaypoint(0, out NextWaypoint);
            CurrentDirection = Path.CalculateSpaceshipDirection(NextWaypoint);
            CheckIfLastPoint();

            _jet.Init(jetSprite);

            _spaceshipSpriteRenderer.AssignSprite(spaceshipSprite);
            _speed = speed;
            _acceleration = acceleration;
            _canMove = true;
            _spaceshipHolder.SetActive(true);
        }
        void CheckIfLastPoint()
        {
            if (NextWaypoint == Path.FinalWaypoint)
            {
                IsLastPartOfPath = true;
            }
            else
            {
                IsLastPartOfPath = false;
            }
        }
        private void FixedUpdate()
        {
            if (_canMove)
            {
                if (Path.CheckIFReachedNextWaypoint(transform.position, NextWaypoint))
                {
                    if (NextWaypoint == Path.FinalWaypoint)
                    {
                        StopSpaceship();
                        return;
                    }
                    else
                    {
                        Path.TryGetNextWaypoint(NextWaypoint, out NextWaypoint);
                        CurrentDirection = Path.CalculateSpaceshipDirection(NextWaypoint);
                    }
                }
                if (NextWaypoint == Path.FinalWaypoint)
                {
                    _jet.ActivateJet(true);
                }
                Move();
            }
        }

        void Move()
        {
            CheckIfLastPoint();
            if (IsLastPartOfPath)
                _speed += _acceleration * Time.fixedDeltaTime;

            var finalMovementVector = _speed * Time.fixedDeltaTime * CurrentDirection;
            transform.Translate(finalMovementVector);
        }
        void StopSpaceship()
        {
            _canMove = false;
            _jet.ActivateJet(false);
            _spaceshipHolder.SetActive(false);
            OnReachedDestination.Invoke(this);
        }
        private void OnDisable()
        {
            _canMove = false;
            Path.IsOccupied = false;
        }
    }
}