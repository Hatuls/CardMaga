using CardMaga.UI;
using System;
using System.Collections.Generic;
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
        [SerializeField] List<Jet> _jets;
        Jet _currentJet;
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
        private bool TryGetJet(JetDirectionSO jetDirection, out Jet jet)
        {
            jet = null;
            foreach (var j in _jets)
            {
                if (jetDirection == j.JetDirectionSO)
                {
                    jet = j;
                    return true;
                }
            }
            return false;
        }
        public void CheckValidation()
        {
            _canMove = false;
            if (_jets == null)
                throw new System.Exception("Spaceship Has no Jets");
            if (Path == null)
                throw new System.Exception("Spaceship Has no Path");
            if (_spaceshipSpriteRenderer == null)
                throw new System.Exception("Spaceship Has no Sprite Renderer");
            if (_spaceshipHolder == null)
                throw new System.Exception("Spacship Has no Holder");
        }
        public void Init(Path path, Sprite spaceshipSprite, Sprite jetSprite, JetDirectionSO jetDirection, float speed, float acceleration)
        {
            Path = path;
            transform.position = Path.FirstWaypoint.position;
            Path.TryGetNextWaypoint(0, out NextWaypoint);
            CurrentDirection = Path.CalculateSpaceshipDirection(NextWaypoint);
            CheckIfLastPoint();
            if (TryGetJet(jetDirection, out _currentJet))
            {
                _currentJet.Init(jetSprite);
            }
            else
            {
                throw new System.Exception("Spapcship has no Jet");
            }
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
        private void Update()
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
                    _currentJet.ActivateJet(true);
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
            foreach (var j in _jets)
            {
                j.ActivateJet(false);
            }
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