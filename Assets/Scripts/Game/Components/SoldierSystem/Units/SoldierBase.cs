using System;
using System.Collections.Generic;
using Game.Components.BuildingSystem.Buildings;
using Game.Components.GridSystem;
using Game.Components.Interface;
using Game.Components.PathFindingSystem;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Components.SoldierSystem.Units
{
    public class SoldierBase : MonoBehaviour, IHittable, IMilitaryUnit
    {
        private int _health;
        public int Health => _health;
        private Barracks _currentBarracks;
        [SerializeField] private Transform target;
        private IDisposable _movementUpdate;
        private List<Node> _path = new();
        private int _currentNodeIndex;
 
        public void Initialize(int health, Barracks barracks)
        {
            _health = health;
            barracks.SetSoldier(this);
            
        }
        
        #region IGridObject Properties

        public bool Available { get; }
        public Vector2Int Center { get; }
        public Vector2Int Size { get; }

        public void GetNeighbours()
        {
            throw new NotImplementedException();
        }

        public void OnSelect()
        {
            
        }

        public bool CanMove { get; }
        public void Attack(IGridObject target)
        {
            // _path =  PathFinding.Instance.FindPath(transform.position, target);
            // if (_path != null)
            // {
            //     _currentBarracks.ClearSoldier();
            //     _currentBarracks = null;
            //     _currentNodeIndex = 0;
            //     _movementUpdate = Observable.EveryUpdate().Subscribe(_ => MoveToTarget());
            // }
        }

    

        #endregion


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                _path =  PathFinding.Instance.FindPath(transform.position, target.position);
                if (_path != null)
                {
                    _currentBarracks.ClearSoldier();
                    _currentBarracks = null;
                    _currentNodeIndex = 0;
                    _movementUpdate = Observable.EveryUpdate().Subscribe(_ => MoveToTarget());
                }
            }
        }

        public void MoveToTarget()
        {
            Vector3 targetPosition = _path[_currentNodeIndex].WorldPosition;
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * 3 * Time.deltaTime;
            
            if (Vector3.Distance(transform.position, targetPosition) <= .01f)
            {
                _currentNodeIndex++;
                if (_currentNodeIndex < _path.Count) return;
                transform.position = targetPosition;
                _movementUpdate?.Dispose();
            }
        }
        public void OnHit(int damage)
        {
            throw new System.NotImplementedException();
        }
        public void OnDestroyed()
        {
            _currentBarracks = null;
        }
        
    }
}