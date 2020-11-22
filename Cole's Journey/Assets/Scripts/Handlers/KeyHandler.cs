using UnityEngine;
namespace ProjectFukalite.Handlers
{
    public static class KeyHandler
    {
        public static KeyCode JumpKey = KeyCode.Space;
        public static KeyCode CrouchKey = KeyCode.LeftControl;
        public static KeyCode DashKey = KeyCode.LeftShift;
        public static KeyCode AttackKey = KeyCode.Mouse0;
        public static float GetInputX()
        {
            return Input.GetAxisRaw("Horizontal");
        }
        public static float GetInputY()
        {
            return Input.GetAxisRaw("Vertical");
        }
    }
}