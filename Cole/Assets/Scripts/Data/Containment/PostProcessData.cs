using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
namespace ProjectFukalite.Data.Containment
{
    [RequireComponent(typeof(PostProcessVolume))]
    public class PostProcessData : MonoBehaviour
    {
        [HideInInspector] public PostProcessVolume postProcessVolume;
        [HideInInspector] public float weight;
        private void Awake()
        {
            postProcessVolume = GetComponent<PostProcessVolume>();
            weight = postProcessVolume.weight;
        }

        public void SetWeight(float _weightMultiplier)
        {
            postProcessVolume.weight = weight * _weightMultiplier;
        }
    }
}