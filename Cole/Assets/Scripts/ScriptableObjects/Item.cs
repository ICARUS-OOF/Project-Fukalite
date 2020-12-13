using UnityEngine;
using ProjectFukalite.Enums;
namespace ProjectFukalite.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Data/Item")]
    public class Item : ScriptableObject
    {
        public ItemType itemType;

        [DrawConditionedField("itemType", ItemType.Food, ComparisonType.Equals)]
        public int foodHealAmnt = 30;
        
        [DrawConditionedField("itemType", ItemType.Drink, ComparisonType.Equals)]
        public int drinkHealAmnt = 30;

        [DrawConditionedField("itemType", ItemType.Weapon, ComparisonType.Equals)]
        public Weapon _weapon;
        
        public Sprite spriteImage;

        [DrawConditionedField("itemType", ItemType.Weapon, ComparisonType.NotEqual)]
        public GameObject prefab;

        public bool canDrop = true;
        [Range(1, 50)]
        public int stackLimit = 1;
        [TextArea] public string Description;
    }
}
