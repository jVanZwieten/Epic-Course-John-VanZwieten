using Scripts.Managers;
using UnityEngine;

namespace Scripts.Towers
{
    public class TowerGhost : MonoBehaviour
    {
        [SerializeField]
        private Material[] Materials;

        void Start()
        {
            TowerBuildManager.TowerPlacementModeToggle += OnTowerPlacementModeToggle;
            TowerSlot.TowerGhostSnapToSlot += OnTowerGhostSnapToSlot;
        }

        void OnEnable()
        {
            ChangeColor(TowerBuildManager.Instance.BuildDisallowedColor);

            var hitPoint = Utils.RaycastToMouse()?.point;
            if (hitPoint.HasValue)
                transform.position = hitPoint.Value;
        }

        private void OnTowerPlacementModeToggle(bool placementMode)
        {
            if (!placementMode)
                gameObject.SetActive(false);
        }

        private void OnTowerGhostSnapToSlot(TowerSlot towerSlot)
        {
            transform.position = towerSlot.transform.position;
            ChangeColor(TowerBuildManager.Instance.BuildAllowedColor);
        }

        public void ChangeColor(Color color)
        {
            foreach (var material in Materials)
                material.color = color;
        }
    }
}
