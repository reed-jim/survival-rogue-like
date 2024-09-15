using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Prototypes.Action
{
    public class CharacterUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Transform canvas;
        [SerializeField] private TMP_Text objectiveHoldText;

        #region PRIVATE FIELD
        private float _initialYPosition;
        #endregion

        private void Awake()
        {
            CharacterObjectiveHolder.setCharacterObjectiveHold += SetObjectiveHold;

            objectiveHoldText.gameObject.SetActive(false);

            _initialYPosition = objectiveHoldText.transform.position.y;
        }

        private void Update()
        {
            canvas.transform.eulerAngles = Vector3.zero;
        }

        private void OnDestroy()
        {
            CharacterObjectiveHolder.setCharacterObjectiveHold -= SetObjectiveHold;
        }

        private void SetObjectiveHold(int number)
        {
            objectiveHoldText.gameObject.SetActive(true);

            objectiveHoldText.text = $"{number}";
        }
    }
}
