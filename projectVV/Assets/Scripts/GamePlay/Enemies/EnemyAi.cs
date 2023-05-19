using System;
using System.Collections;
using UnityEngine;

namespace GamePlay.Enemies
{
    public class EnemyAi:MonoBehaviour
    {
        //Check FOV
        [Header("FOV")]
        public float radius;
        [Range(0, 360)] public float angle;
        public GameObject playerRef;
        public LayerMask targetMask;
        public LayerMask obstructionMask;
        public bool canSeePlayer;
        [SerializeField] public float heightCanSee;
        
        //Size
        [Header("Size")]
        [SerializeField] public float turnSmoothTime = 0.1f;
        protected float turnSmoothVelocity;
        [SerializeField] protected float speed;
        [SerializeField] protected float range;
        [SerializeField] protected Rigidbody rb;
        
        [SerializeField] protected Animator animator;

        protected virtual void Start()
        {
            StartCoroutine(FovRoutine());
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

                    canSeePlayer = !Physics.Raycast(transform.position, directionToTarget, distanceToTarget,
                        obstructionMask);
                    if (Mathf.Abs(target.transform.position.y - transform.position.y) > heightCanSee)
                    {
                        canSeePlayer = false;
                    }
                }
                else
                    canSeePlayer = false;
            }
            else if (canSeePlayer)
                canSeePlayer = false;
        }

    }
}