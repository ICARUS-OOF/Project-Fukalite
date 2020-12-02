using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace ProjectFukalite.Testing
{
    [RequireComponent(typeof(Text))]
    public class FpsCounter : MonoBehaviour
    {
        private Text FPSText;
        [SerializeField] private float fpsCounter;
        private void Start()
        {
            FPSText = GetComponent<Text>();
            StartCoroutine(DelayedUpdate());
        }
        private IEnumerator DelayedUpdate()
        {
            for ( ; ; )
            {
                fpsCounter = 1f / Time.deltaTime;
                FPSText.text = "FPS: " + Mathf.RoundToInt(fpsCounter).ToString();
                yield return new WaitForSeconds(.3f);
            }
        }
    }
}