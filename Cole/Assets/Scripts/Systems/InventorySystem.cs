using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProjectFukalite.ScriptableObjects;
using ProjectFukalite.Data.UI;
using ProjectFukalite.Handlers;
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

        [Header("Obtaining UI")]
        [SerializeField] private GameObject obtainedUIItem;
        [SerializeField] private Transform obtainedPanel;
        [SerializeField] private Transform obtainedHolder;
        [SerializeField] private Text obtainedHeaderText;

        [SerializeField] private Image BGImg1;
        [SerializeField] private Image BGImg2;
        [SerializeField] private Image BGImg3;

        [Header("Inventory UI")]
        public bool isOnInventory = false;
        [SerializeField] private GameObject inventoryPanel;
        public InventorySlot[] slots;
        public int[] slotQuants;

        private Color origBGColor;
        private Color origBG3Color;

        private void Start()
        {
            inventoryPanel.SetActive(true);
            slots = inventoryPanel.GetComponentsInChildren<InventorySlot>();
            inventoryPanel.SetActive(false);
            slotQuants = new int[slots.Length];

            obtainedPanel.gameObject.SetActive(false);

            origBGColor = BGImg1.color;
            origBG3Color = BGImg3.color;

            BGImg1.color = Color.clear;
            BGImg2.color = Color.clear;
            BGImg3.color = Color.clear;

            obtainedHeaderText.color = Color.clear;
        }

        private void Update()
        {
            if (PlayerUI.singleton.isCutscene)
            { return; }

            if (Input.GetKeyDown(KeyHandler.InventoryKey))
            {
                isOnInventory = !isOnInventory;
            }

            if (isOnInventory)
            {
                UpdateUI();
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    isOnInventory = false;
                }
            }

            inventoryPanel.SetActive(isOnInventory);

            if (obtainedHolder.childCount > 0)
            {
                obtainedHeaderText.color = Color.Lerp(obtainedHeaderText.color, Color.white, Time.fixedDeltaTime * 2f);
                BGImg1.color = Color.Lerp(BGImg1.color, origBGColor, Time.fixedDeltaTime * 2f);
                BGImg2.color = Color.Lerp(BGImg2.color, origBGColor, Time.fixedDeltaTime * 2f);
                BGImg3.color = Color.Lerp(BGImg3.color, origBG3Color, Time.fixedDeltaTime * 2f);
            } else
            {
                obtainedHeaderText.color = Color.Lerp(obtainedHeaderText.color, Color.clear, Time.fixedDeltaTime * 2f);
                BGImg1.color = Color.Lerp(BGImg1.color, Color.clear, Time.fixedDeltaTime * 2f);
                BGImg2.color = Color.Lerp(BGImg2.color, Color.clear, Time.fixedDeltaTime * 2f);
                BGImg3.color = Color.Lerp(BGImg3.color, Color.clear , Time.fixedDeltaTime * 2f);
            }
        }

        public void AddItem(Item _item)
        {
            if (!AddItemToList(_item))
            { return; }
            UpdateUI();
            obtainedPanel.gameObject.SetActive(true);
            GameObject _obtainedUIItem = Instantiate(obtainedUIItem, obtainedHolder);
            ObtainedUIItem UIItemScript = _obtainedUIItem.GetComponent<ObtainedUIItem>();
            if (UIItemScript != null)
            {
                UIItemScript.Setup(_item);
            }
            StartCoroutine(DestroyObtainedUIItem(_obtainedUIItem));
            StartCoroutine(DisableList());
        }

        private bool AddItemToList(Item _item)
        {
            bool canAdd = false;

            for (int i = 0; i < slots.Length; i++)
            {
                InventorySlot currentSlot = slots[i];

                if (_item.stackLimit == 1)
                {
                    if (currentSlot.item == null)
                    {
                        canAdd = true;
                        Items.Add(_item);
                        slotQuants[i]++;
                        break;
                    }
                } else
                {
                    if (currentSlot.item == _item)
                    {
                        //When stacking
                        if (currentSlot.quantity < _item.stackLimit)
                        {
                            slotQuants[i]++;
                            canAdd = true;
                            break;
                        }
                    }
                    else
                    {
                        //When filling a new slot
                        if (currentSlot.item == null)
                        {
                            Items.Add(_item);
                            slotQuants[i]++;
                            canAdd = true;
                            break;
                        }
                    }
                }
            }

            return canAdd;
        }

        public void UpdateUI()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (i < Items.Count)
                {
                    slots[i].FillSlot(Items[i], slotQuants[i]);
                } else
                {
                    slots[i].EmptySlot();
                }
            }
        }

        public void RemoveItem(Item _item)
        {
            Items.Remove(_item);
        }

        private IEnumerator DisableList()
        {
            yield return new WaitForSeconds(5f);
            if (obtainedHolder.childCount == 1)
            {
                obtainedPanel.gameObject.SetActive(false);
            }
        }

        private IEnumerator DestroyObtainedUIItem(GameObject _obtainedUIItem)
        {
            yield return new WaitForSeconds(5f);
            Destroy(_obtainedUIItem);
        }
    }
}