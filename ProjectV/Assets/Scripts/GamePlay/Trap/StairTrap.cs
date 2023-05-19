using System;
using UnityEngine;

namespace GamePlay.Trap
{
    public class StairTrap:MonoBehaviour
    {
        [SerializeField] private Transform detectPos;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField]private Vector3 size;

        private void Update()
        {
            TrapDetect();
        }

        private void TrapDetect()
        {
            var player=Physics.OverlapBox(transform.position, size,Quaternion.identity,playerLayer);
            if (player.Length > 0)
            {
               Invoke(nameof(TrapActive),1);
            }
        }
        private void TrapActive()
        {
           transform.parent.gameObject.SetActive(false);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawCube(detectPos.position,size);
        }
    }
}