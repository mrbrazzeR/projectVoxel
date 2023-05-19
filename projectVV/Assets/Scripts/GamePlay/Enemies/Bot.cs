using System.Collections;
using UnityEngine;

namespace GamePlay.Enemies
{
    public class Bot : MonoBehaviour
    {
        [SerializeField] private float timeAttack;
        [SerializeField] private Transform bulletSpawnPoint;
        private WaitForSeconds _time;
        private Coroutine _coroutine;
        private void Start()
        {
            _time = new WaitForSeconds(timeAttack);
            _coroutine= StartCoroutine(Shoot());
        }

        private void OnDisable()
        {
            StopCoroutine(_coroutine);
        }

        private IEnumerator Shoot()
        {
            while (true)
            {
                yield return _time;
                var bullet = Pool.poolInstance.Spawn("Bullet");
                bullet.transform.position = bulletSpawnPoint.position;
                var bulletBody = bullet.GetComponent<Bullet>();
                if (bulletBody)
                {
                    bulletBody.Moving(bulletSpawnPoint);
                }
            }
        }
    }
}