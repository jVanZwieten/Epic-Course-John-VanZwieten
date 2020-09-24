using Scripts.Managers;
using System;
using UnityEngine;

namespace Scripts.Towers
{
    [RequireComponent(typeof(ParticleSystem))]
    public class TowerSlot : MonoBehaviour
    {
        private const float snapDistance = 1;

        private ParticleSystem _particleSystem;

        public bool IsBuiltOn { get; set; }

        public static event Action<TowerSlot> onTowerGhostSnapToSlot;


        private void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            TowerBuildManager.onTowerPlacementModeToggle += OnTowerPlacementModeToggle;
        }

        private void OnTowerPlacementModeToggle(bool placementMode)
        {
            if (placementMode && !IsBuiltOn)
            {
                Show();
            }
            else if (_particleSystem.isPlaying)
            {
                Hide();
            }
        }

        private void Hide()
        {
            _particleSystem.Stop();
            TowerBuildManager.onTowerGhostMove -= OnTowerGhostMove;
            TowerBuildManager.onTowerBuilt -= OnTowerBuilt;
        }

        private void Show()
        {
            _particleSystem.Play();
            TowerBuildManager.onTowerGhostMove += OnTowerGhostMove;
            TowerBuildManager.onTowerBuilt += OnTowerBuilt;
        }

        private void OnTowerBuilt(GameObject Tower)
        {
            if (Tower.transform.position == transform.position)
            {
                IsBuiltOn = true;
                Hide();
            }
        }

        private void OnTowerGhostMove(Vector3 towerGhostPosition)
        {
            if (WithinSnapZone(towerGhostPosition))
                onTowerGhostSnapToSlot(this);
        }

        private bool WithinSnapZone(Vector3 towerGhostPosition) => 
            Vector3.Distance(towerGhostPosition, transform.position) <= snapDistance;
    }
}
