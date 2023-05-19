using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class Inventory:MonoBehaviour
    {
        [SerializeField] private List<Item> items;
        [SerializeField] private ItemSlot[] itemSlots;
    }
}