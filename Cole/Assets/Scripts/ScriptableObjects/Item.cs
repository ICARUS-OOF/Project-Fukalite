using UnityEngine;
using ProjectFukalite.Enums;
namespace ProjectFukalite.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Data/Item")]
    public class Item : ScriptableObject
    {
        public ItemType itemType;
        public Sprite spriteImage;
        public GameObject prefab;
        [TextArea] public string Description;
    }
}
