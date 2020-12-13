using UnityEngine;
using UnityEngine.UI;
using ProjectFukalite.ScriptableObjects;
using ProjectFukalite.Systems;
namespace ProjectFukalite.Data.UI
{
    public class InventorySlot : MonoBehaviour
    {
        public Item item;
        public Image icon;
        public Text quantText;
        public int quantity;

        private void Update()
        {
            if (quantity == 0 || quantity == 1)
            {
                quantText.text = "";
            } else
            {
                quantText.text = quantity.ToString();
            }

            icon.gameObject.SetActive(item != null);

            if (quantity <= 0)
            {
                EmptySlot();
            }
        }

        public void FillSlot(Item _item, int quant)
        {
            item = _item;
            icon.sprite = item.spriteImage;
            quantity = quant;

            icon.gameObject.SetActive(true);
        }

        public void EmptySlot()
        {
            item = null;
            icon.sprite = null;
            quantity = 0;

            icon.gameObject.SetActive(false);
        }

        public void Inspect()
        {
            ItemInteraction.singleton.InspectItem(this);
        }
    }
}