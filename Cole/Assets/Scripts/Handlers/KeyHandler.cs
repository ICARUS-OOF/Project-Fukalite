using UnityEngine;
namespace ProjectFukalite.Handlers
{
    public static class KeyHandler
    {
        public static KeyCode JumpKey = KeyCode.Space;
        public static KeyCode DashKey = KeyCode.LeftShift;
        public static KeyCode AttackKey = KeyCode.Mouse0;
        public static KeyCode BlockKey = KeyCode.Mouse1;
        public static KeyCode StrafeKey = KeyCode.LeftControl;
        public static KeyCode PickupKey = KeyCode.E;
        public static KeyCode TriggerKey = KeyCode.F;
        public static KeyCode InventoryKey = KeyCode.I;

        public static float GetInputX()
        {
            return Input.GetAxisRaw("Horizontal");
        }

        public static float GetInputY()
        {
            return Input.GetAxisRaw("Vertical");
        }

        public static bool IsMoving()
        {
            return Input.GetKey(KeyCode.W)
                || Input.GetKey(KeyCode.A)
                || Input.GetKey(KeyCode.S)
                || Input.GetKey(KeyCode.D);
        }

        public static bool IsStrafing()
        {
            return (Input.GetKey(KeyCode.A) && Input.GetKey(StrafeKey))
                || (Input.GetKey(KeyCode.D) && Input.GetKey(StrafeKey));
        }
    }
}