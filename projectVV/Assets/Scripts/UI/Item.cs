using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "Item",menuName = "ItemMenu",order = 0)]    
    public class Item:ScriptableObject
    {
        public string itemName;
        public Sprite icon;
    }
}