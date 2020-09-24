using UnityEngine;

namespace Scripts
{
    public static class Utils
    {
        public static RaycastHit? RaycastToMouse()
        {
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(rayOrigin, out RaycastHit hitInfo);
            return hitInfo;
        }
    }
}
