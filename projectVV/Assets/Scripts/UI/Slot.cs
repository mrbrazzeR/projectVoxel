using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Slot:MonoBehaviour
    {
        private Item _item;

        public Item Item
        {
            get => _item;
            set
            {
                _item = value;
                if (_item == null)
                {
                    image.enabled = false;
                }
                else
                {image.enabled = true;
                    SetSprite();
                }
            }
        }

        public Image image;

        private void Start()
        {
            SetSprite();
        }

        public void SetSprite()
        {
            if (_item != null)
            {
                image.sprite = _item.icon;
            }
        }
    }
}