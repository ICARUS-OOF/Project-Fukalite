using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ProjectFukalite.ScriptableObjects;
namespace ProjectFukalite.Data.UI
{
    public class ObtainedUIItem : MonoBehaviour
    {
        [SerializeField] private Text itemNameText;
        [SerializeField] private Image itemIcon;

        [SerializeField] private Transform NormalPoint, StartPoint;

        public Image bgImg;
        public Transform itemHolder;

        private bool obtained = false;

        private void Start()
        {
            itemHolder.position = StartPoint.position;
        }

        public void Setup(Item _item)
        {
            itemNameText.text = _item.name;
            itemIcon.sprite = _item.spriteImage;
            StartCoroutine(lerpBGImg());
            StartCoroutine(changeObtainedBool());
        }

        private IEnumerator lerpBGImg()
        {
            while (true)
            {
                bgImg.color = Color.Lerp(bgImg.color, Color.clear, Time.fixedDeltaTime * 1.4f);
                yield return null;
            }
        }

        private IEnumerator changeObtainedBool()
        {
            obtained = true;
            yield return new WaitForSeconds(3f);
            obtained = false;
        }

        private void Update()
        {
            if (obtained)
            {
                itemHolder.position = Vector3.Lerp(itemHolder.position, NormalPoint.position, Time.fixedDeltaTime * 8f);
            } else
            {
                itemHolder.position = Vector3.Lerp(itemHolder.position, StartPoint.position, Time.fixedDeltaTime * 8f);
            }
        }
    }
}