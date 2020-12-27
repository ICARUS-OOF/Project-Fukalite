using UnityEngine;
using UnityEngine.UI;
using ProjectFukalite.Enums;
using ProjectFukalite.ScriptableObjects;
using ProjectFukalite.Data.UI;
using ProjectFukalite.Data.Containment;
using ProjectFukalite.Handlers;
namespace ProjectFukalite.Systems
{
    public class ItemInteraction : MonoBehaviour
    {
        #region Singleton
        public static ItemInteraction singleton;
        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }
        }
        #endregion

        private InventorySlot currentSlot;
        private PlayerReferencer referencer;

        private InventorySystem invSys;
        private InteractionSystem intSys;

        [SerializeField] private Camera cam;

        [SerializeField] private Text itemNameText, descriptionText, UseText;
        [SerializeField] private Button UseButton, DropButton;

        private void Start()
        {
            invSys = InventorySystem.singleton;
            referencer = PlayerReferencer.singleton;
            intSys = InteractionSystem.singleton;
        }
        private void OnEnable()
        {
            itemNameText.gameObject.SetActive(false);
            descriptionText.gameObject.SetActive(false);
            UseText.gameObject.SetActive(false);
            UseButton.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            itemNameText.gameObject.SetActive(false);
            descriptionText.gameObject.SetActive(false);
            UseText.gameObject.SetActive(false);
            UseButton.gameObject.SetActive(false);
            if (TutorialHandler.singleton == null)
            {
                DropButton.gameObject.SetActive(false);
            }
        }

        public void InspectItem(InventorySlot _slot)
        {
            currentSlot = _slot;

            Item _item = currentSlot.item;
            itemNameText.text = _item.name;
            descriptionText.text = _item.Description;

            itemNameText.gameObject.SetActive(true);
            descriptionText.gameObject.SetActive(true);
            UseText.gameObject.SetActive(true);
            UseButton.gameObject.SetActive(true);

            if (TutorialHandler.singleton == null)
            {
                DropButton.gameObject.SetActive(_item.canDrop);
            }

            switch (_item.itemType)
            {
                case ItemType.Food:
                    UseText.text = "Eat 1";
                    break;
                case ItemType.Drink:
                    UseText.text = "Drink 1";
                    break;
                case ItemType.Weapon:
                    UseText.text = "Equip";
                    break;
            }
        }

        public void UseItem()
        {
            Item _item = currentSlot.item;

            switch (_item.itemType)
            {
                case ItemType.Food:
                    referencer.playerData.Heal(_item.foodHealAmnt);
                    invSys.slotQuants[invSys.Items.IndexOf(_item)]--;
                    currentSlot.quantity--;
                    AudioHandler.PlaySoundEffect("Eat");
                    break;
                case ItemType.Drink:
                    referencer.playerData.IncreaseStamina(_item.drinkHealAmnt);
                    invSys.slotQuants[invSys.Items.IndexOf(_item)]--;
                    currentSlot.quantity--;
                    AudioHandler.PlaySoundEffect("Drink");
                    break;
                case ItemType.Weapon:
                    referencer.weaponSystem.SetWeapon(_item._weapon);
                    invSys.slotQuants[invSys.Items.IndexOf(_item)]--;
                    currentSlot.EmptySlot();
                    break;
            }

            if (currentSlot.quantity <= 0)
            {
                itemNameText.gameObject.SetActive(false);
                descriptionText.gameObject.SetActive(false);
                UseText.gameObject.SetActive(false);
                UseButton.gameObject.SetActive(false);
                if (TutorialHandler.singleton == null)
                {
                    DropButton.gameObject.SetActive(false);
                }
            }

            invSys.UpdateUI();
        }

        public void DropItem()
        {
            Item _item = currentSlot.item;
            
            invSys.slotQuants[invSys.Items.IndexOf(_item)]--;
            currentSlot.quantity--;

            switch (_item.itemType)
            {
                case ItemType.Food:
                    GameObject foodObj = Instantiate(_item.prefab, intSys.dropPos, Quaternion.identity);
                    break;
                case ItemType.Drink:
                    GameObject drinkObj = Instantiate(_item.prefab, intSys.dropPos, Quaternion.identity);
                    break;
                case ItemType.Weapon:
                    GameObject weaponObj = Instantiate(_item._weapon.prefab, intSys.dropPos, Quaternion.identity);
                    break;
            }
        }
    }
}