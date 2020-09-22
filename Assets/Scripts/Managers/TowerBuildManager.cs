using Scripts.Towers;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Managers
{
    public class TowerBuildManager : MonoSingleton<TowerBuildManager>
    {
        [SerializeField]
        private List<TowerGhost> _buildableTowers;

        private bool _towerPlacementMode;
        private TowerGhost _towerGhost;

        private bool mouseMoved => Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0;

        [SerializeField]
        private Color _buildAllowedColor;
        public Color BuildAllowedColor { get { return _buildAllowedColor; } }
        [SerializeField]
        private Color _buildDisallowedColor;
        public Color BuildDisallowedColor { get { return _buildDisallowedColor; } }

        public delegate void TowerGhostMoveEventHandler(Vector3 towerGhostPosition);
        public static event TowerGhostMoveEventHandler TowerGhostMove;

        public delegate void TowerPlacementModeToggleEventHandler(bool placementMode);
        public static event TowerPlacementModeToggleEventHandler TowerPlacementModeToggle;


        void Start()
        {
            _towerPlacementMode = false;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                EnterPlacementMode(0);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                ExitPlacementMode();
            }

            if (_towerPlacementMode && mouseMoved)
                TowerGhostUpdate();
        }
        private void EnterPlacementMode(int towerSelection)
        {
            _towerPlacementMode = true;

            SetTowerGhost(_buildableTowers[towerSelection]);

            TowerPlacementModeToggle(_towerPlacementMode);
        }

        private void ExitPlacementMode()
        {
            _towerPlacementMode = false;

            TowerPlacementModeToggle(_towerPlacementMode);
        }


        private void SetTowerGhost(TowerGhost towerGhostSelection)
        {
            if (_towerGhost != towerGhostSelection)
            {
                _towerGhost?.gameObject.SetActive(false);
                _towerGhost = towerGhostSelection;
            }

            _towerGhost.gameObject.SetActive(true);
        }

        private void TowerGhostUpdate()
        {
            Vector3? hitPoint = Utils.RaycastToMouse()?.point;

            if (hitPoint.HasValue)
            {
                _towerGhost.transform.position = hitPoint.Value;
                _towerGhost.ChangeColor(BuildDisallowedColor);

                TowerGhostMove(hitPoint.Value);
            }
        }
    }
}
