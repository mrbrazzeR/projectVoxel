using System;
using UnityEngine;

namespace GamePlay.Enemies
{
    public class TreeMan : EnemyAi
    {
        private bool _canMove;

        private Vector3 _startPosition;
        private Quaternion _startRotation;

        protected override void Start()
        {
            base.Start();
            _startPosition = transform.position;
            _startRotation = transform.rotation;
            _canMove = true;
        }

        private void Update()
        {
            if (_canMove)
            {
                if (canSeePlayer)
                {
                    Chasing();
                }
                else
                {
                    BackToPosition();
                }
            }
        }

        private void Chasing()
        {
            var direction = playerRef.transform.position - transform.position;
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            var smoothDampAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothDampAngle, 0f);
            var moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized * speed;
            rb.velocity = moveDir;
            animator.SetBool($"Moving", true);
            if (direction.magnitude <= range)
            {
                rb.velocity = Vector3.zero;
                animator.SetBool($"Moving", false);
                Attack();
            }
        }

        private void BackToPosition()
        {
            var direction = _startPosition - transform.position;
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            var smoothDampAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                ref turnSmoothVelocity,
                turnSmoothTime);
            if (direction.magnitude <= 1f)
            {
                transform.rotation = _startRotation;
                animator.SetBool($"Moving", false);
                return;
            }
            transform.rotation = Quaternion.Euler(0f, smoothDampAngle, 0f);
            var moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized * speed;
            rb.velocity = moveDir;
            animator.SetBool($"Moving", true);
        }

        private void Attack()
        {
            animator.SetTrigger($"Attack");
        }

        private void OnMove()
        {
            _canMove = true;
        }

        private void OffMove()
        {
            _canMove = false;
        }
    }
}