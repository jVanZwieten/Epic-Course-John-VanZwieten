using UnityEngine;

namespace Scripts.Towers
{
    public class RangeSphere : MonoBehaviour
    {
        [SerializeField]
        private Tower _parent;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("enemy"))
                _parent.OnEnemyEnter(collider);
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider.CompareTag("enemy"))
                _parent.OnEnemyExit(collider);
        }
    }
}
