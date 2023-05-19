using GamePlay.Players;
using UnityEngine;

namespace GamePlay.Enemies
{
    public class Trooper : Enemy, IEnemy
    {
        public bool EarnDamage(int damage)
        {
            var canSee = gameObject.GetComponentInParent<EnemyFov>();
            if (canSee.canSeePlayer)
            {
                if (damage >= point)
                {
                    PlayerManager.Manager.Coin += point;
                    transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                PlayerManager.Manager.Coin += point;
                transform.parent.gameObject.SetActive(false);
            }
            return true;
        }
    }
}