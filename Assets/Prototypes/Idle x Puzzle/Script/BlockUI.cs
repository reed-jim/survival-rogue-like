using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;

public class BlockUI : MonoBehaviour
{
    [SerializeField] private TMP_Text pointText;

    #region PRIVATE FIELD
    private List<Tween> _tweens;
    #endregion

    private void Awake()
    {
        _tweens = new List<Tween>();

        BaseBlock.showPointTextEvent += ShowPoint;

        pointText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        BaseBlock.showPointTextEvent -= ShowPoint;
    }

    private void ShowPoint(int instanceID, int point)
    {
        if (instanceID == gameObject.GetInstanceID())
        {
            pointText.text = $"{point}";

            AnimationUtil.ShowDamage(pointText, _tweens);
        }
    }
}
