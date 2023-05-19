using System;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Enemies
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        private int _damage;
        public int health;
        [SerializeField] private Slider healthBar;

        private void Start()
        {
            healthBar.minValue = 0;
            healthBar.maxValue = health;
            healthBar.value = health;
        }

        public void EarnDamage(int damage)
        {
            Debug.Log("Earn damage" + damage);
            health -= damage;
            SetSlider();
            if (health <= 0)
            {
                transform.parent.gameObject.SetActive(false);
            }
        }

        private void SetSlider()
        {
            healthBar.value = health;
        }
    }
}