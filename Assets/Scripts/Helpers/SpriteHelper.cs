using System;
using System.Collections.Generic;
using Game.Signals.Helpers;
using Scripts.Helpers;
using UnityEngine;
using UnityEngine.U2D;
using Zenject;
using Random = UnityEngine.Random;

namespace Scripts.Helpers
{
    public class SpriteHelper: MonoBehaviour
    {
        public static SpriteHelper Instance;
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