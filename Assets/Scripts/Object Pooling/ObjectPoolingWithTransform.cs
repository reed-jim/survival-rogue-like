using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle.Merge
{
    public class ObjectPoolingWithTransform : ObjectPoolingWithOneComponent<Transform>
    {
        protected override void Awake()
        {
            base.Awake();

            BlockSpawner.getTransformEvent += GetComponentFromPool;
        }

        private void OnDestroy()
        {
            BlockSpawner.getTransformEvent -= GetComponentFromPool;
        }
    }
}
