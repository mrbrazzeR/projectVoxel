using System;
using GamePlay.Players;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Trap
{
    public class QuizTrap:MonoBehaviour
    {
        [SerializeField] private Button exitButton;

        private void Start()
        {
            exitButton.onClick.AddListener(Exit);
        }

        private void Exit()
        {
            PlayerManager.Manager.GameState = GameState.Moving;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameObject.SetActive(false);
        }
    }
}