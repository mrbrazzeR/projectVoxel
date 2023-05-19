using System;
using GamePlay.Players;
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
            PlayerManager.Manager.Coin++;
            gameObject.SetActive(false);
        }
    }
}