using PlazmaGames.Attribute;
using System;
using UnityEngine;

namespace Recursive
{
    public class ParadoxHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask _ghostLayer;
        [SerializeField] private float _blockageCheckRadius = 0.2f;
        [SerializeField] private float _groundedCheckRadius = 0.1f;
        [SerializeField] private Transform _feet;

        [SerializeField, ReadOnly] private bool _isFlying = false;
        [SerializeField, ReadOnly] private float _flyingStartTime;
        [SerializeField, ReadOnly] private Vector3 _lastPos;
        [SerializeField, ReadOnly] private float _velY;

        private void Paradox()
        {
            Destroy(gameObject);
            Debug.Log("Paradox Triggered!");
        }

        private bool CheckForBlockage()
        {
            return Physics.CheckSphere(transform.position, _blockageCheckRadius, ~_ghostLayer);
        }

        private bool IsFlying()
        {
            return !Physics.CheckSphere(_feet.position, _groundedCheckRadius, ~_ghostLayer);
        }

        private void FixedUpdate()
        {
            if (RecursiveGameManager.IsPaused) return;

            _velY = (transform.position.y - _lastPos.y) / Time.deltaTime;

            if (Mathf.Abs(_velY) <= 0.02f && IsFlying())
            {
                if (!_isFlying) _flyingStartTime = Time.time;
                _isFlying = true;

                if (Time.time - _flyingStartTime > 0.5f)
                {
                    Debug.Log("Paradox From Flying!");
                    Paradox();
                }
            }
            else
            {
                _isFlying = false;
            }

            if (CheckForBlockage())
            {
                Debug.Log("Paradox From Blockage!");
                Paradox();
            }

            _lastPos = transform.position;
        }
    }
}
