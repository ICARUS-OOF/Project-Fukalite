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
        [SerializeField] private Vector3 offset = new Vector3(0f, 2f, 0f);

        public void Interact()
        {
            InventorySystem.singleton.AddItem(_item);
            Destroy(gameObject);
        }

        public void Display()
        {
            displayObj.SetActive(true);
            displayObj.transform.position = transform.position + offset;
        }

        public void Undisplay()
        {
            displayObj.SetActive(false);
        }
    }
}