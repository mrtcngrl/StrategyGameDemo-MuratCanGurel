using System.Collections.Generic;
using Scripts.Helpers;
using UnityEngine;
using UnityEngine.U2D;
using Zenject;

namespace Game.Components
{
    public class AtlasManager: MonoBehaviour
    {
        public static AtlasManager Instance;
        [SerializeField] private SpriteAtlas _spriteAtlas;
        [Inject]
        private void OnInject()
        {
            if (!object.ReferenceEquals(Instance, null) && !object.ReferenceEquals(Instance, this)) this.Destroy();
            else
            {
                Instance = this;
            }
        }
        public Sprite GetSprite(string name)
        {
            return _spriteAtlas.GetSprite(name);
        }
    }
}