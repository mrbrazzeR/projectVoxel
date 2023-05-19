using GamePlay.Players;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CoinUi:MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        

        public void SetText()
        {
            text.text = PlayerManager.Manager.Coin.ToString();
        }
    }
}