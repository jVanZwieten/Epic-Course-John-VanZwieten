using Scripts.Managers;
using UnityEngine;

namespace Scripts.Towers
{
    [RequireComponent(typeof(ParticleSystem))]
    public class TowerSlot : MonoBehaviour
    {
        private const float snapDistance = 1;

        private ParticleSystem _particleSystem;

        public bool IsBuiltOn { get; set; }

        public delegate void TowerGhostSnapToSlotEventHandler(TowerSlot towerSlot);
        public static event TowerGhostSnapToSlotEventHandler TowerGhostSnapToSlot;


        private void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            TowerBuildManager.TowerPlacementModeToggle += OnTowerPlacementModeToggle;
        }

        private void OnTowerPlacementModeToggle(bool placementMode)
        {
            if (placementMode && !IsBuiltOn)
            {
                _particleSystem.Play();
                TowerBuildManager.TowerGhostMove += OnTowerGhostMove;
            }
            else if (_particleSystem.isPlaying)
            {
                _particleSystem.Stop();
                TowerBuildManager.TowerGhostMove -= OnTowerGhostMove;
            }
        }

        private void OnTowerGhostMove(Vector3 towerGhostPosition)
        {
            if (WithinSnapZone(towerGhostPosition))
            {
                TowerGhostSnapToSlot(this);
            }
        }

        private bool WithinSnapZone(Vector3 towerGhostPosition) => 
            Vector3.Distance(towerGhostPosition, transform.position) <= snapDistance;
    }
}
