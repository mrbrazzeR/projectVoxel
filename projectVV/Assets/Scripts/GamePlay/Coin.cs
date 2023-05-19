using System;
using Manager;
using Player;
using UnityEngine;

namespace GamePlay
{
    public class Coin:MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayer;
        private void Update()
        {
            Collect();
        }

        private void Collect()
        {
            var player=Physics.CheckSphere(transform.position, 0.5f,playerLayer);
            if (!player) return;
            PlayerManager.manager.coin++;
            Debug.Log(PlayerManager.manager.coin);
            gameObject.SetActive(false);
        }
    }
}