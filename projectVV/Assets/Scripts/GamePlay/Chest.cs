using System.Collections;
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
            StartCoroutine(OpenChess());
        }

        private IEnumerator OpenChess()
        {
            yield return new WaitForSeconds(3f);
            gameObject.SetActive(false);
        }
    }
}