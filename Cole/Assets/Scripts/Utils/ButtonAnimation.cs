using UnityEngine;
using UnityEngine.EventSystems;
namespace ProjectFukalite.Utils
{
    public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Transform objToAnim;

        private bool isHighlighting;
        
        private Vector3 origScale;
        private Quaternion origRot;

        private Vector3 hlScale;
        private Quaternion hlRot;

        private void Start()
        {
            origScale = objToAnim.localScale;
            origRot = objToAnim.rotation;

            hlScale = origScale * 1.3f;
            hlRot = Quaternion.Euler(new Vector3(origRot.x, origRot.y, -5f));
        }

        private void OnDisable()
        {
            objToAnim.localScale = origScale;
            objToAnim.rotation = origRot;
            isHighlighting = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHighlighting = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHighlighting = false;
        }

        private void Update()
        {
            if (isHighlighting)
            {
                objToAnim.localScale = Vector3.Lerp(objToAnim.localScale, hlScale, Time.fixedDeltaTime * 3f);
                objToAnim.rotation = Quaternion.Lerp(objToAnim.rotation, hlRot, Time.fixedDeltaTime * 3f);
            } else
            {
                objToAnim.localScale = Vector3.Lerp(objToAnim.localScale, origScale, Time.fixedDeltaTime * 3f);
                objToAnim.rotation = Quaternion.Lerp(objToAnim.rotation, origRot, Time.fixedDeltaTime * 3f);
            }
        }
    }
}