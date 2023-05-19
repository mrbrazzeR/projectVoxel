using UI;
using UnityEngine;

namespace Manager
{
    public class PlayerManager:MonoBehaviour
    {
        public static PlayerManager manager;

        public int coin;
        public int health;
        public int damage;
        
        public int attackCount;

        [SerializeField] private PlayerUi playerUi;
        public PlayerManager()
        {
            coin = 0;
            health = 50;
            damage = 5;
            attackCount = 1;
        }

        public int Health
        {
            get => health;
            set
            {
                health = value;
                playerUi.SetView();
            }
        }

        private void Start()
        {
            manager =this;
            playerUi.StartView();
        }
    }
}