using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProjectFukalite.ScriptableObjects;
using ProjectFukalite.Data.UI;
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

        private Color origBGColor;
        private Color origBG3Color;

        private void Start()
        {
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
            obtainedPanel.gameObject.SetActive(true);
            Items.Add(_item);
            GameObject _obtainedUIItem = Instantiate(obtainedUIItem, obtainedHolder);
            ObtainedUIItem UIItemScript = _obtainedUIItem.GetComponent<ObtainedUIItem>();
            if (UIItemScript != null)
            {
                UIItemScript.Setup(_item);
            }
            StartCoroutine(DestroyObtainedUIItem(_obtainedUIItem));
            StartCoroutine(DisableList());
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