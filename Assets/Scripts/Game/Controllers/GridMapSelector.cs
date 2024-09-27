using Game.Components.GridSystem.Interface;
using Game.Components.Interface;
using UnityEngine;
using Grid = Game.Components.GridSystem.Core.Grid;

namespace Game.Controllers
{
    public class GridMapSelector : MonoBehaviour
    {
        private IGridObject _firstSelection;
        private IGridObject _secondSelection;
        
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
                    ProcessAttack();
                }
            }
        }
        
        
        
        private void ProcessAttack()
        {
            if (_firstSelection is IMilitaryUnit militaryUnit && _secondSelection is IHittable)
            {
                militaryUnit.MoveAndAttack(_secondSelection);
            }

            _firstSelection = null;
            _secondSelection = null;
        }
        private IGridObject TryGetGridObject(Vector3 mousePosition)
        {
            Vector2 mousePosition2D = _camera.ScreenToWorldPoint(mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero, 100, GameConstants.Selectable);
            if (hit.collider != null)
            {
                var selectable = hit.collider.GetComponentInParent<IGridObject>();
                if (selectable != null)
                    return selectable;
            }
            return null;
        }
    }
}