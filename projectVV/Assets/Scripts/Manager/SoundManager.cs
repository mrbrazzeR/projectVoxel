using System;
using UnityEngine;

namespace Manager
{
    public class SoundManager:MonoBehaviour
    {
        public static SoundManager instance;

        public AudioClip chem1;
        public AudioClip chem1Detect;
        public AudioClip chem2;
        public AudioClip chem2Detect;
        public AudioClip chem3;
        public AudioClip chem3Detect;
        public AudioSource audioSource;
        private void Start()
        {
            instance = this;
        }
    }
}