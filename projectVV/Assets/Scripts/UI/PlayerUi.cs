using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerUi:MonoBehaviour
    {
        [SerializeField] private Slider healthBar;
        [SerializeField] private TMP_Text healthText;

        public void SetView()
        {
            healthBar.value = PlayerManager.manager.health;
            healthText.text = PlayerManager.manager.health.ToString();
        }

        public void StartView()
        {
            healthBar.minValue = 0;
            healthBar.maxValue = PlayerManager.manager.health;
            healthBar.value = PlayerManager.manager.health;
            healthText.text = PlayerManager.manager.health.ToString();
        }
    }
}