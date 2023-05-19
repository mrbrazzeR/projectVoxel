using System.Collections;
using GamePlay.Players;
using UnityEngine;

namespace GamePlay
{
    public class Chest:MonoBehaviour
    {
        [SerializeField]private Animator animator;

        public bool IsClaimed { get; private set; }

        public void Claimed()
        {
            IsClaimed = true;
            animator.SetTrigger($"Open");
            PlayerManager.Manager.Coin += 3;
            StartCoroutine(OpenChess());
        }

        private IEnumerator OpenChess()
        {
            yield return new WaitForSeconds(3f);
            gameObject.SetActive(false);
        }
    }
}