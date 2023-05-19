using System;
using System.Collections;
using UnityEngine;

namespace GamePlay.Enemies
{
    public class EnemyFov : MonoBehaviour
    {
        public float radius;
        [Range(0, 360)] public float angle;

        public GameObject playerRef;

        public LayerMask targetMask;
        public LayerMask obstructionMask;

        public bool canSeePlayer;

        [SerializeField] private float turnSmoothTime = 0.1f;
        private float _turnSmoothVelocity;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private float speed;
        [SerializeField] private float range;
        [SerializeField] private Transform[] wayPoints;
        private int _currentStep;
        private Coroutine _attackCoroutine;
        private float _timeAttack;
        [SerializeField] private float maxTimeAttack;

        private void Start()
        {
            StartCoroutine(FovRoutine());
        }

        private void Update()
        {
            if (canSeePlayer)
            {
                EnemyMoving();
            }
            else
            {
                Patrol();
            }
        }

        private IEnumerator FovRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(0.2f);

            while (true)
            {
                yield return wait;
                FieldOfViewCheck();
            }
        }

        private void FieldOfViewCheck()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    var trigger = !Physics.Raycast(transform.position, directionToTarget, distanceToTarget,
                        obstructionMask);
                    canSeePlayer = !(Mathf.Abs(target.transform.position.y - transform.position.y) > 1f) && trigger;
                }
                else
                    canSeePlayer = false;
            }
            else if (canSeePlayer)
                canSeePlayer = false;
        }

        private void EnemyMoving()
        {
            var direction = playerRef.transform.position - transform.position;
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            var smoothDampAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                ref _turnSmoothVelocity,
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
            var smoothDampAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
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
                var bullet = Pool.PoolInstance.Spawn("Bullet");
                bullet.transform.position = bulletSpawnPoint.position;
                var bulletBody = bullet.GetComponent<Bullet>();
                if (bulletBody)
                {
                    bulletBody.Moving(bulletSpawnPoint);
                }

                _timeAttack = maxTimeAttack;
            }
        }
    }
}