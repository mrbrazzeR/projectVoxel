using GamePlay.Enemies;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace GamePlay.Players
{
    public class ThirdPersonMovement : MonoBehaviour
    {
        public Transform cam;
        [SerializeField] private CharacterController controller;

        [Header("Moving")] [SerializeField] private FixedJoystick variableJoystick;
        [SerializeField] private float speed;
        [SerializeField] private float turnSmoothTime = 0.1f;
        private float _turnSmoothVelocity;
        private float _horizontal;
        private float _vertical;

        //Attack
        [Header("Attack")] private float _cooldownAttack;
        [SerializeField] private float attackTime;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private float attackRange;

        //Jump
        [Header("Jump")] [SerializeField] private float gravity;
        [SerializeField] private float jumpSpeed;
        [SerializeField] private int maxJump;
        private int _jumpLeft;
        private Vector3 _velocity;

        //FallDown
        [SerializeField] private float fallHeight;

        //Check Transform
        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform bodyCheck;

        [Header("Layer")]
        //Layer
        [SerializeField]
        private LayerMask groundLayer;

        [SerializeField] private LayerMask deadLayer;
        [SerializeField] private LayerMask chestLayer;

        private Vector3 _startPosition;
        public Animator animator;
        private Coroutine _coroutine;


        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _startPosition = transform.position;
            _jumpLeft = maxJump;
        }

        private void Update()
        {
            if (PlayerManager.Manager.GameState == GameState.Moving)
            {
                Jump();
                Collect();
                Attack();
            }
        }

        private void FixedUpdate()
        {
            if (PlayerManager.Manager.GameState == GameState.Moving)
            {
                Moving();
                JoyMoving();
            }
        }

        private void Attack()
        {
            if (_cooldownAttack <= 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    animator.SetTrigger($"Attack");
                    Invoke(nameof(DetectAttack), 0.5f);
                    _cooldownAttack = attackTime;
                }
            }

            _cooldownAttack -= Time.deltaTime;
        }

        private void DetectAttack()
        {
            var objSphere = Physics.OverlapSphere(attackPoint.position, attackRange);
            if (objSphere.Length > 0)
            {
                foreach (var obj in objSphere)
                {
                    var enemy = obj.GetComponent<IEnemy>();
                    var detect = enemy?.EarnDamage(PlayerManager.Manager.Coin);
                    if (detect == false)
                    {
                        ReSpawn();
                    }
                }
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
            var direction = new Vector3(_horizontal, 0f, _vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                    turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                var moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                animator.SetBool($"Moving", true);
                controller.Move(moveDir * (speed * Time.deltaTime));
            }

            _velocity.y += gravity * Time.deltaTime;
            controller.Move(_velocity * Time.deltaTime);
            if (IsGround())
            {
                _jumpLeft = maxJump;
            }

            if (IsDead())
            {
                ReSpawn();
            }
        }

        private void JoyMoving()
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

        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_jumpLeft > 0)
                {
                    _velocity.y = jumpSpeed;
                    _jumpLeft--;
                    animator.SetTrigger($"Jump");
                }
            }
        }

        public void Climb()
        {
            animator.SetTrigger($"Climb");
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
                            PlayerManager.Manager.Coin++;
                        }
                    }
                }
            }
        }

        public void ReSpawn()
        {
            transform.position = _startPosition;
        }


//Conditions
        private bool IsGround()
        {
            return Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
        }

        private bool IsDead()
        {
            return Physics.CheckSphere(groundCheck.position, 0.1f, deadLayer);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(attackPoint.position, attackRange);
        }
    }
}