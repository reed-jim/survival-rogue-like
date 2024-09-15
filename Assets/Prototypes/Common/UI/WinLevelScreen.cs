using System;
using System.Collections;
using System.Collections.Generic;
using Prototypes.Merge;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle.Merge
{
    public class WinLevelScreen : UIScreen
    {
        [SerializeField] private Button nextLevelButton;

        #region ACTION
        public static event Action nextLevelEvent;
        #endregion

        protected override void GenerateUI()
        {
            base.GenerateUI();
        }

        public override void RegisterEvent()
        {
            base.RegisterEvent();

            GameScoreManager.winLevelEvent += Show;

            nextLevelButton.onClick.AddListener(NextLevel);
        }

        public override void UnregisterEvent()
        {
            base.UnregisterEvent();

            GameScoreManager.winLevelEvent -= Show;
        }

        private void NextLevel()
        {
            nextLevelEvent?.Invoke();

            Hide();
        }
    }
}
