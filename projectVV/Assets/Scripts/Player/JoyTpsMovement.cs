using System;
using UnityEngine;

namespace Player
{
    public class JoyTpsMovement:MonoBehaviour
    {
        
        [Header("Moving")] [SerializeField] private FixedJoystick variableJoystick;
        
        public Transform cam;
        [SerializeField] private CharacterController controller;

        [SerializeField] private float speed;
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashTime;
        [SerializeField] private float turnSmoothTime = 0.1f;
        [SerializeField] private int maxDash;
        public Animator animator;
        private int _dashLeft;
        private float _turnSmoothVelocity;
        private float _horizontal;
        private float _vertical;

        private void FixedUpdate()
        {
            JoyMove();
        }

        private void JoyMove()
        {
            var horizon = variableJoystick.Horizontal;
            var vert = variableJoystick.Vertical;
            var direct = Vector3.forward * vert + Vector3.right * horizon;

            if (direct.magnitude >= 0.1f)
            {
                var targetAngle = Mathf.Atan2(direct.x, direct.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                    turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                var moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                animator.SetBool($"Moving", true);
                controller.Move(moveDir * (speed * Time.deltaTime));
            }
        }

    }
}