using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototypes.Action
{
    [CreateAssetMenu(fileName = "Player Runtime", menuName = "ScriptableObjects/Prototypes/Action/PlayerRuntime")]
    public class PlayerRuntime : ScriptableObject
    {
        private Transform _player;

        public Transform Player
        {
            get => _player;
            set => _player = value;
        }
    }
}
