using System.Collections;
using GamePlay.Players;
using UnityEngine;

namespace GamePlay.Trap
{
    public class InteractQuiz:MonoBehaviour
    {
        [SerializeField] private GameObject quizUi;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField]private Vector3 size;
        private void Update()
        {
            Interact();
        }

        private void Interact()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                var player=Physics.OverlapBox(transform.position, size,Quaternion.identity,playerLayer);
                if (player.Length > 0)
                {
                    quizUi.SetActive(true);
                    PlayerManager.Manager.GameState = GameState.Quiz;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawCube(transform.position,size);
        }
    }
}