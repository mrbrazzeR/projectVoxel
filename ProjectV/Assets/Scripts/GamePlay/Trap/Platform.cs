using System;
using System.Collections;
using GamePlay.Players;
using UnityEngine;

namespace GamePlay.Trap
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private Vector3 size;
        [SerializeField] private LayerMask playerLayer;
        private bool _state;
        private Collider _playerDetect;
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            StartCoroutine(ChangeState());
        }

        private void Update()
        {
            CheckPlatform();
        }

        private IEnumerator ChangeState()
        {
            while (isActiveAndEnabled)
            {
                yield return new WaitForSeconds(2f);
                _state = !_state;
                _animator.SetBool($"On", _state);
            }
        }

        private void CheckPlatform()
        {
            var player = Physics.OverlapBox(transform.position, size, Quaternion.identity, playerLayer);
            if (player.Length > 0)
            {
                foreach (var pl in player)
                {
                    var obj = pl.GetComponent<ThirdPersonMovement>();
                    if (obj != null)
                    {
                        pl.transform.SetParent(transform);
                        _playerDetect = pl;
                    }
                }
            }
            else
            {
                if (_playerDetect != null)
                {
                    _playerDetect.transform.SetParent(null);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawCube(transform.position, size);
        }
    }
}