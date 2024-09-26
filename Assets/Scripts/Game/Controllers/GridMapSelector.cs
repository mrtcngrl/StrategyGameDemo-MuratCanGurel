using System;
using Game.Components.GridSystem;
using Scripts.Helpers.InputSystem;
using Unity.VisualScripting;
using UnityEngine;
using Grid = Game.Components.GridSystem.Grid;

namespace Game.Controllers
{
    public class GridMapSelector : MonoBehaviour
    {
        private IGridObject _firstSelection;
        private IGridObject _secondSelection;
        private ISelectable _selectedItem;
        
        private Camera _camera;
        private Grid _grid;
        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _firstSelection = TryGetGridObject(Input.mousePosition);
                _firstSelection.OnSelect();
            }
            if (Input.GetMouseButtonDown(1))
            {
                _secondSelection = TryGetGridObject(Input.mousePosition);
                if (_secondSelection != null && _firstSelection is IMilitaryUnit)
                {
                    StartGridObjectMove();
                }
            }
        }

        private void StartGridObjectMove()
        {
            (_firstSelection as IMilitaryUnit)?.Attack(_secondSelection);
            _firstSelection = null;
            _secondSelection = null;
        }
        private IGridObject TryGetGridObject(Vector3 mousePosition)
        {
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            if (!Physics.Raycast(ray, out var hit, 150f, GameConstants.Selectable))
                return null;
            var selectable = hit.transform.GetComponentInParent<IGridObject>();
            if (selectable != null)
                return selectable;
            return null;
        }
    }
}