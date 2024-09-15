using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototypes.Merge
{
    public class GameScoreManager : MonoBehaviour
    {
        [SerializeField] private int requiredScore;

        #region PRIVATE FIELD
        private int _score;
        #endregion

        #region ACTION
        public static event Action<int, int> setScoreEvent;
        #endregion

        private void Awake()
        {
            CharacterObjectiveHolder.earnObjectiveEvent += EarnScore;
        }

        private void OnDestroy()
        {
            CharacterObjectiveHolder.earnObjectiveEvent -= EarnScore;
        }

        private void EarnScore(int additionalScore)
        {
            _score += additionalScore;

            setScoreEvent?.Invoke(_score, requiredScore);
        }
    }
}
