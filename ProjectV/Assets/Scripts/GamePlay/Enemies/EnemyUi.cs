using System;
using TMPro;
using UnityEngine;

namespace GamePlay.Enemies
{
    public class EnemyUi:MonoBehaviour
    {
        [SerializeField] private Enemy enemy;
        [SerializeField] private TMP_Text text;

        private void Start()
        {
            text.text = enemy.point.ToString();
        }
    }
}