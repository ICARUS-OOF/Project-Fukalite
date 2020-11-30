using UnityEngine;
using ProjectFukalite.Enums;
namespace ProjectFukalite.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Data/Item")]
    public class Item : ScriptableObject
    {
        public ItemType itemType;
        public GameObject model, prefab;
    }
}
