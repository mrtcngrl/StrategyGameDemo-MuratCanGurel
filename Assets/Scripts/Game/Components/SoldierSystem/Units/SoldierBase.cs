using System;
using System.Collections.Generic;
using System.Linq;
using Game.Components.BuildingSystem.Buildings;
using Game.Components.GridSystem.Interface;
using Game.Components.GridSystem.Managers;
using Game.Components.GridSystem.PathFindingSystem;
using Game.Components.Interface;
using Game.Pool;
using Game.UI.ProductionMenu.Interface;
using UniRx;
using UnityEngine;

namespace Game.Components.SoldierSystem.Units
{
    public class SoldierBase : MonoBehaviour, IHittable, IMilitaryUnit
    {
        [SerializeField] private int _damage;
        private int _health;
        private Barracks _currentBarracks;
        private IDisposable _movementUpdate;
        private IDisposable _attackTimer;
        private int _currentNodeIndex;
        private IHittable _currentHitTarget;
        private List<Node> _path = new();
        private IProduct _properties;
        private Node _stayedNode;
        public void Initialize(IProduct product, Barracks barracks)
        {
            _properties = product;
            _health = _properties.Health;
            barracks.SetSoldier(this);
            _stayedNode = GridManager.Instance.Grid.GetNodeByWorldPos(WorldPosition);
            TrySetStayedNodeWalkable(false);
            _currentBarracks = barracks;
        }
        
        public int Health => _health;
        #region IGridObject Properties

        public bool Available => _movementUpdate == null;
        public Vector2Int Size => Vector2Int.one;
        public Vector3 WorldPosition => transform.position;
        public void OnSelect()
        {
        }

        public int Damage => _damage;

        public void MoveAndAttack(IGridObject gridObject)
        {
            _path = PathFinding.Instance.FindPath(transform.position, gridObject.WorldPosition, gridObject);
            if (_path != null)
            {
               
                _currentNodeIndex = 0;
                if (_path.Count == 0)
                {
                    OnTargetReached();
                    return;
                }
                //for spawn point stay Unwalkable
                TrySetStayedNodeWalkable(_currentBarracks == null);
                
                _currentBarracks?.ClearSoldier();
                _currentBarracks = null;
               
                _stayedNode = _path.Last();
                _movementUpdate = Observable.EveryUpdate().Subscribe(_ => MoveToTarget());
                _currentHitTarget = gridObject as IHittable;
            }
        }
        #endregion

        private void TrySetStayedNodeWalkable(bool key)
        {
            if(_stayedNode == null) return;
            _stayedNode.Walkable = key;
        }
        private void MoveToTarget()
        {
            Vector3 targetPosition = _path[_currentNodeIndex].WorldPosition;
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * 3 * Time.deltaTime;
            if (Vector3.Distance(transform.position, targetPosition) <= .01f)
            {
                _currentNodeIndex++;
                if (_currentNodeIndex < _path.Count) return;
                transform.position = targetPosition;
                OnTargetReached();
            }
        }

        private void OnTargetReached()
        {
            TrySetStayedNodeWalkable(false);
            _movementUpdate?.Dispose();
            if(_currentHitTarget != null)
                InitializeAttack();
        }

        private void InitializeAttack()
        {
            _attackTimer?.Dispose();
            _attackTimer = Observable.Timer(TimeSpan.FromSeconds(GameConstants.AttackInterval)).Repeat().Subscribe(_ => Attack());
        }

        private void Attack()
        {
            if (_currentHitTarget.Health > 0)
            {
                _currentHitTarget.OnHit(Damage);
            }
            else
            {
                _attackTimer?.Dispose();
                _currentHitTarget = null;
            }
        }
        public void OnHit(int damage)
        {
            _health -= _damage;
            if(_health <= 0)
                OnDestroyed();
        }
        
        public void OnDestroyed()
        {
            TrySetStayedNodeWalkable(true);
            _currentBarracks?.ClearSoldier();
            _currentBarracks = null;
            MonoPool.Instance.ReturnToPool(_properties.ProductName,gameObject);
        }
        
    }
}