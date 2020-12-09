using UnityEngine;
using ProjectFukalite.Interfaces;
using ProjectFukalite.ScriptableObjects;
using ProjectFukalite.Systems;
namespace ProjectFukalite.Data.World
{
    public class Obtainable : MonoBehaviour, IInteractable, IDisplay
    {
        public Item _item;
        public GameObject displayObj;

        public void Interact()
        {
            InventorySystem.singleton.AddItem(_item);
            Destroy(gameObject);
        }

        public void Display()
        {
            displayObj.SetActive(true);
        }

        public void Undisplay()
        {
            displayObj.SetActive(false);
        }
    }
}