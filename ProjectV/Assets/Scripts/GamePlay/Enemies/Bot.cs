using System.Collections;
using GamePlay.Players;
using UnityEngine;

namespace GamePlay.Enemies
{
    public class Bot : Enemy, IEnemy
    {
        [SerializeField] private float timeAttack;
        [SerializeField] private Transform bulletSpawnPoint;
        private WaitForSeconds _time;
        private Coroutine _coroutine;

        private void Start()
        {
            _time = new WaitForSeconds(timeAttack);
            _coroutine = StartCoroutine(Shoot());
        }

        private void OnDisable()
        {
            StopCoroutine(_coroutine);
        }

        private IEnumerator Shoot()
        {
            while (isActiveAndEnabled)
            {
                yield return _time;
                var bullet = Pool.PoolInstance.Spawn("Bullet");
                bullet.transform.position = bulletSpawnPoint.position;
                var bulletBody = bullet.GetComponent<Bullet>();
                if (bulletBody)
                {
                    bulletBody.Moving(bulletSpawnPoint);
                }
            }
        }

        public bool EarnDamage(int damage)
        {
            PlayerManager.Manager.Coin += point;
            transform.parent.gameObject.SetActive(false);
            return true;
        }
    }
}