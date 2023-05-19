using System;
using GamePlay.Players;
using UnityEngine;

namespace GamePlay.Trap
{
    public class LadderAction:MonoBehaviour
    {
        [SerializeField] private Ladder ladder;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField]private Vector3 size;
        private void Update()
        {
            Climb();
        }

        private void Climb()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                var player=Physics.OverlapBox(transform.position, size,Quaternion.identity,playerLayer);
                if (player.Length > 0)
                {
                    foreach (var pl in player)
                    {
                        pl.GetComponent<ThirdPersonMovement>().Climb();
                    }
                    ladder.Moving();
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawCube(transform.position,size);
        }
    }
}