using System;
using System.Collections;
using GamePlay;
using GamePlay.Enemies;
using Manager;
using UnityEngine;
using UnityEngine.VFX;

namespace Player
{
    public class ThirdPersonMovement : MonoBehaviour
    {
        public Transform cam;
        [SerializeField] private CharacterController controller;

        public Animator animator;

        //Check Transform
        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform bodyCheck;
        [SerializeField] private Transform comboCheck;

        //Layer
        [Header("Layer")] [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask deadLayer;
        [SerializeField] private LayerMask chestLayer;

        [Header("Moving")] [SerializeField] private float speed;
        [SerializeField] private float dashSpeed;
        [SerializeField] private float turnSmoothTime = 0.1f;
        private bool _canMoving;

        [Header("Dash")] [SerializeField] private float dashTime;
        [SerializeField] private int maxDash;
        [SerializeField] private float maxDashCountDown;
        private float _dashCountdown;
        private int _dashLeft;
        private float _turnSmoothVelocity;
        private float _horizontal;

        private float _vertical;
        private Vector3 _direction;

        //Jump
        [Header("Jump")] [SerializeField] private float gravity;
        [SerializeField] private float jumpSpeed;
        [SerializeField] private int maxJump;
        private int _jumpLeft;
        private Vector3 _velocity;
        private Vector3 _startPosition;
        private Coroutine _coroutine;

        [SerializeField] private AttackTrigger attackTrigger;
        [SerializeField] private VisualEffect attackVfx;
        [SerializeField] private GameObject circleAttack;

        private void Start()
        {
            _startPosition = transform.position;
            _jumpLeft = maxJump;
            _dashLeft = maxDash;
            _dashCountdown = maxDashCountDown;
            _canMoving = true;
        }

        private void Update()
        {
            Jump();
            Collect();
            Dash();
            Dead();
        }

        private void FixedUpdate()
        {
            Moving();
            FastMove();
        }


        private void Dead()
        {
            if (IsDead())
            {
                animator.SetBool($"Die", true);
            }
        }

        private void Moving()
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");
            NormalMove();
        }

        private void NormalMove()
        {
            if (_canMoving)
            {
                _direction = new Vector3(_horizontal, 0f, _vertical).normalized;
                if (_direction.magnitude >= 0.1f)
                {
                    animator.SetBool($"Walk", true);
                    var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                        turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    var moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    controller.Move(moveDir * (speed * Time.deltaTime));
                }

                else if (_direction.magnitude < 0.01f)
                {
                    animator.SetBool($"Walk", false);
                }

                _velocity.y += gravity * Time.deltaTime;
                controller.Move(_velocity * Time.deltaTime);
            }

            if (IsGround())
            {
                animator.SetBool($"Jump", false);
                _jumpLeft = maxJump;
                _dashLeft = maxDash;
            }

            if (IsOnDeadLayer())
            {
                ReSpawn();
            }
        }

        private void FastMove()
        {
            if (_canMoving)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    _direction = new Vector3(_horizontal, 0f, _vertical).normalized;
                    if (_direction.magnitude >= 0.1f)
                    {
                        animator.SetBool($"Run", true);
                        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                            turnSmoothTime);
                        transform.rotation = Quaternion.Euler(0f, angle, 0f);
                        var moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                        controller.Move(moveDir * (speed * 2 * Time.deltaTime));
                    }

                    _velocity.y += gravity * Time.deltaTime;
                    controller.Move(_velocity * Time.deltaTime);
                }

                else
                {
                    animator.SetBool($"Run", false);
                }
            }

            if (IsGround())
            {
                animator.SetBool($"Jump", false);
                _jumpLeft = maxJump;
                _dashLeft = maxDash;
            }

            if (IsOnDeadLayer())
            {
                ReSpawn();
            }
        }

        private void Dash()
        {
            if (_canMoving)
            {
                if (_dashCountdown <= 0)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        if (_dashLeft > 0)
                        {
                            _dashLeft--;
                            _dashCountdown = maxDashCountDown;
                            StartCoroutine(DashShow());
                        }
                    }
                }
            }

            _dashCountdown -= Time.deltaTime;
        }

        private IEnumerator DashShow()
        {
            float startTime = dashTime; // need to remember this to know how long to dash
            while (startTime >= 0)
            {
                var direction = new Vector3(_horizontal, 0f, _vertical).normalized;

                if (direction.magnitude >= 0.1f)
                {
                    var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    var moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    controller.Move(moveDir * (dashSpeed * Time.deltaTime));
                }

                yield return null; // this will make Unity stop here and continue next frame
                startTime -= Time.deltaTime;
            }
        }

        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_canMoving)
                {
                    if (_jumpLeft > 0)
                    {
                        animator.SetTrigger($"Jump");
                        _velocity.y = jumpSpeed;
                        _jumpLeft--;
                    }
                }
            }
        }

        private void Collect()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                var collects = Physics.OverlapSphere(bodyCheck.position, 0.7f, chestLayer);
                if (collects.Length > 0)
                {
                    foreach (var collect in collects)
                    {
                        var chest = collect.GetComponent<Chest>();
                        if (!chest.IsClaimed)
                        {
                            chest.Claimed();
                            PlayerManager.manager.coin++;
                            Debug.Log(PlayerManager.manager.coin);
                        }
                    }
                }
            }
        }

        private void ReSpawn()
        {
            transform.position = _startPosition;
        }

        private void StartAttack(int current)
        {
            PlayerManager.manager.attackCount = current;
            CallSound();
            attackTrigger.CanHit();
        }

        private void EndAttack()
        {
            attackTrigger.CantHit();
        }

        private void EndMove()
        {
            _canMoving = false;
        }

        private void StartMove()
        {
            _canMoving = true;
        }

        private void CheckDamage()
        {
            var objs = Physics.OverlapSphere(comboCheck.position, 1.7f);
            var counting = 0;
            foreach (var obj in objs)
            {
                var enemy = obj.GetComponent<Enemy>();
                if (enemy)
                {
                    counting++;
                    attackVfx.Play();
                    Debug.Log(obj.name);
                    enemy.EarnDamage(PlayerManager.manager.damage);
                }
            }

            if (counting > 0)
            {
                SoundManager.instance.audioSource.clip = SoundManager.instance.chem3Detect;
                SoundManager.instance.audioSource.Play();
            }
            else
            {
                SoundManager.instance.audioSource.clip = SoundManager.instance.chem3;
                SoundManager.instance.audioSource.Play();
            }
        }

        private void OnEffectAttack()
        {
            circleAttack.SetActive(true);
        }

        private void OffEffectAttack()
        {
            circleAttack.transform.localScale = new Vector3(0, 0, 0);
            circleAttack.SetActive(false);
        }

        private void CallSound()
        {
            switch (PlayerManager.manager.attackCount)
            {
                case 1:
                    SoundManager.instance.audioSource.clip = SoundManager.instance.chem1;
                    SoundManager.instance.audioSource.Play();
                    break;
                case 2:
                    SoundManager.instance.audioSource.clip = SoundManager.instance.chem2;
                    SoundManager.instance.audioSource.Play();
                    break;
            }
        }

        //Conditions
        private bool IsGround()
        {
            return Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
        }

        private bool IsOnDeadLayer()
        {
            return Physics.CheckSphere(groundCheck.position, 0.1f, deadLayer);
        }

        private bool IsDead()
        {
            return PlayerManager.manager.health <= 0;
        }
    }
}