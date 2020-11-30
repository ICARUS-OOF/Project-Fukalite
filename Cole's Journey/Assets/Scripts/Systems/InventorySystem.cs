using System.Collections.Generic;
using UnityEngine;
using ProjectFukalite.ScriptableObjects;
namespace ProjectFukalite.Systems
{
    public class InventorySystem : MonoBehaviour
    {
        #region Singleton
        public static InventorySystem singleton;
        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }
        }
        #endregion

        public List<Item> Items = new List<Item>();

        public void AddItem(Item _item)
        {
            Items.Add(_item);
        }

        public void RemoveItem(Item _item)
        {
            Items.Remove(_item);
        }
    }
}