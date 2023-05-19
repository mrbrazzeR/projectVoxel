using System;
using System.Collections;
using UnityEngine;

namespace GamePlay.Enemies
{
    public class RangeEnemy : EnemyAi
    {
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private Transform[] wayPoints;
        private int _currentStep;
        private Coroutine _attackCoroutine;
        private float _timeAttack;

        private void Update()
        {
            if (canSeePlayer)
            {
                Chasing();
            }
            else
            {
                Patrol();
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
            if (direction.magnitude <= range)
            {
                rb.velocity = Vector3.zero;
                Shooting();
            }
        }

        private void Patrol()
        {
            var direction = wayPoints[_currentStep].position - transform.position;
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            var smoothDampAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothDampAngle, 0f);
            var moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized * speed;
            rb.velocity = moveDir;
            if (Math.Abs(transform.position.x - wayPoints[_currentStep].position.x) < 0.01f &&
                Math.Abs(transform.position.z - wayPoints[_currentStep].position.z) < 0.01f)
            {
                ChangeWayPoint();
            }
        }

        private void ChangeWayPoint()
        {
            _currentStep = _currentStep == 0 ? 1 : 0;
        }

        private void Shooting()
        {
            _timeAttack -= Time.deltaTime;
            if (_timeAttack <= 0)
            {
                var bullet = Pool.poolInstance.Spawn("Bullet");
                bullet.transform.position = bulletSpawnPoint.position;
                var bulletBody = bullet.GetComponent<Bullet>();
                if (bulletBody)
                {
                    bulletBody.Moving(bulletSpawnPoint);
                }

                _timeAttack = 1f;
            }
        }
    }
}