using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class RotateTile : MonoBehaviour
{
    #region PRIVATE FIELD
    private bool _isRotating;
    #endregion

    private void Awake()
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
                Tween.EulerAngles(transform, transform.eulerAngles, new Vector3(360, 0, 0), duration: 1f)
                .OnComplete(() => _isRotating = false);

                _isRotating = true;
            }
        }
    }
}
