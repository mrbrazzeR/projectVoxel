using System;
using System.Collections;
using GamePlay.Enemies;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        public static PlayerAttack instance;
        public bool canReceiveInput;
        public bool inputReceived;
        public ParticleSystem slashEffect;

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            Attack();
        }

        private void Attack()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (canReceiveInput)
                {
                    inputReceived = true;
                    canReceiveInput = false;
                }
                else
                {
                    return;
                }
            }
        }

        public void InputManager()
        {
            if (!canReceiveInput)
            {
                canReceiveInput = true;
            }
            else
            {
                return;
            }
        }

        public void OnEffect()
        {
            slashEffect.Play();
        }

        public void OffEffect()
        {
            slashEffect.Stop();
        }
    }
}