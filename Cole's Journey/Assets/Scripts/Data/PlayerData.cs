using UnityEngine;
namespace ProjectFukalite.Data
{
    public class PlayerData : MonoBehaviour
    {
        public float Health = 100;
        private PlayerReferencer referencer;

        private void Start()
        {
            referencer = PlayerReferencer.singleton;    
        }
    }
}