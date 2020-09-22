using UnityEngine;

namespace Scripts
{
    public static class Utils
    {
        public static RaycastHit? RaycastToMouse()
        {
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            Physics.Raycast(rayOrigin, out hitInfo);
            return hitInfo;
        }
    }
}
