using System.Collections;
using System.Collections.Generic;
using Saferio.Util.SaferioTween;
using UnityEngine;

public class ObjectPoolingPageViewAnimation : MonoBehaviour
{
    #region ANIMATION
    private RectTransform[] _realItem;
    private RectTransform[] _itemForAnimation;
    #endregion

    private Vector2 _canvasSize;

    public void Init(RectTransform[] pageViewSlots, RectTransform slotContainer, Vector2 canvasSize)
    {
        int slotNumber = pageViewSlots.Length;

        _itemForAnimation = new RectTransform[slotNumber];

        _realItem = pageViewSlots;

        Spawn(slotNumber, pageViewSlots[0], slotContainer);

        _canvasSize = canvasSize;

    }

    private void Spawn(int number, RectTransform slotPrefab, RectTransform container)
    {
        _itemForAnimation = new RectTransform[number];

        for (int i = 0; i < number; i++)
        {
            _itemForAnimation[i] = Instantiate(slotPrefab, container);
            _itemForAnimation[i].name = $"Item For Animation {i}";

            _itemForAnimation[i].gameObject.SetActive(false);

            UIUtil.SetSize(_itemForAnimation[i], _realItem[i].sizeDelta);
            UIUtil.SetLocalPosition(_itemForAnimation[i], _realItem[i].localPosition);
        }
    }

    public void RunAnimation()
    {
        foreach (var item in _realItem)
        {
            StartCoroutine(SpringAnimation.SpringScaleAnimation(item, 0.02f * Vector3.one, 0.1f, 4, 0.1f));
        }
    }

    // public void RunAnimation()
    // {
    //     foreach (var item in _realItem)
    //     {
    //         Vector3 initialPosition = item.localPosition;

    //         Vector3 dest = initialPosition;

    //         dest.x -= 1f * _canvasSize.x;

    //         SaferioTween.Position(item, dest, 0.5f, onCompletedAction: () => UIUtil.SetLocalPositionX(item, initialPosition.x));
    //     }

    //     foreach (var item in _itemForAnimation)
    //     {
    //         item.gameObject.SetActive(true);

    //         Vector3 initialPosition = item.localPosition;

    //         UIUtil.SetLocalPositionX(item, item.localPosition.x + 1f * _canvasSize.x);

    //         SaferioTween.Position(item, initialPosition, 0.5f, onCompletedAction: () => item.gameObject.SetActive(false));
    //     }
    // }
}
