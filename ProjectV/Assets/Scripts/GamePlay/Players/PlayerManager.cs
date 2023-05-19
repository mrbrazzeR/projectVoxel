using System;
using UI;
using UnityEngine;

namespace GamePlay.Players
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Manager;

        [SerializeField] private CoinUi coinUi;
        private int _coin;

        public int Coin
        {
            get => _coin;
            set
            {
                _coin = value;
                coinUi.SetText();
            }
        }

        public GameState GameState { get; set; }

        public PlayerManager()
        {
            _coin = 0;
            GameState = GameState.Moving;
        }


        private void Start()
        {
            Manager = GetComponent<PlayerManager>();
        }
    }
}