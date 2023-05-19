using System.Collections.Generic;
using System.Linq;
using GamePlay.Enemies;
using Manager;
using UnityEngine;
using UnityEngine.VFX;

namespace Player
{
    public class AttackTrigger : MonoBehaviour
    {
        [SerializeField] private List<Collider> triggerList;
        [SerializeField] private VisualEffect attackVfx;
        private bool _hit;
        private int _count;

        public void CanHit()
        {
            _hit = true;
        }

        public void CantHit()
        {
            _hit = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            _count = PlayerManager.manager.attackCount;
            if (_hit)
            {
                if (other.CompareTag($"Enemy"))
                {
                    CallSoundDetect();

                    if (!triggerList.Contains(other))
                    {
                        //add the object to the list
                        triggerList.Add(other);
                        attackVfx.Play();
                        foreach (var enemy in triggerList.Select(trigger => trigger.GetComponent<Enemy>())
                            .Where(enemy => enemy))
                        {
                            enemy.EarnDamage(PlayerManager.manager.damage);
                        }
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            //if the object is in the list
            if (triggerList.Contains(other))
            {
                //remove it from the list
                triggerList.Remove(other);
            }
        }

        private void CallSoundDetect()
        {
            switch (_count)
            {
                case 1:
                    SoundManager.instance.audioSource.clip = SoundManager.instance.chem1Detect;
                    SoundManager.instance.audioSource.Play();
                    break;
                case 2:
                    SoundManager.instance.audioSource.clip = SoundManager.instance.chem2Detect;
                    SoundManager.instance.audioSource.Play();
                    break;
            }
        }
    }
}