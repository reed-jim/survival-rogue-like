using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class RotateTile : BaseTile
{
    #region PRIVATE FIELD
    private bool _isRotating;
    #endregion

    protected override void Init()
    {
        StartCoroutine(Rotating());
    }

    private IEnumerator Rotating()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            if (!_isRotating)
            {
                _tweens.Add(Tween.EulerAngles(transform, transform.eulerAngles, new Vector3(360, 0, 0), duration: 1f)
                .OnComplete(() => _isRotating = false));

                _isRotating = true;
            }
        }
    }
}
