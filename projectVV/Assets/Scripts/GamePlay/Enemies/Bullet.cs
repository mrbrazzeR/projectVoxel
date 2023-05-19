using System;
using System.Collections;
using Manager;
using Player;
using UnityEngine;

namespace GamePlay.Enemies
{
    public class Bullet : MonoBehaviour
    {
        private Coroutine _coroutine;
        private Rigidbody _rb;
        [SerializeField] private float speed;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField]private Vector3 size;

        private void OnEnable()
        {
            _rb = GetComponent<Rigidbody>();
            _coroutine = StartCoroutine(Dispose());
        }

        private void OnDisable()
        {
            StopCoroutine(_coroutine);
        }

        private void Update()
        {
            Check();
        }

        public void Moving(Transform spawn)
        {
            transform.rotation = spawn.rotation;
            _rb.velocity = spawn.forward * speed;
        }

        private IEnumerator Dispose()
        {
            yield return new WaitForSeconds(3f);
            gameObject.SetActive(false);
        }

        private void Check()
        {
            var player = Physics.OverlapBox(transform.position, size, Quaternion.identity, playerLayer);
            if (player.Length > 0)
            {
                PlayerManager.manager.Health -= 1;
                StopCoroutine(_coroutine);
                gameObject.SetActive(false);
            }

            var ground = Physics.OverlapBox(transform.position, size, Quaternion.identity, groundLayer);
            if (ground.Length > 0)
            {
                StopCoroutine(_coroutine);
                gameObject.SetActive(false);
            }
        }
    }
}