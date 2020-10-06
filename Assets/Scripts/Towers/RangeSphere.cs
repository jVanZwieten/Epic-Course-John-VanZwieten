using UnityEngine;

namespace Scripts.Towers
{
    public class RangeSphere : MonoBehaviour
    {
        [SerializeField]
        private Tower _parent;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("enemy"))
                _parent.OnEnemyEnter(collider);
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider.CompareTag("enemy"))
                if (_parent != null)
                    _parent.OnEnemyExit(collider);
        }
    }
}
