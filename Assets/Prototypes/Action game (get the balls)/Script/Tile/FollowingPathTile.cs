using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class FollowingPathTile : MonoBehaviour
{
    [SerializeField] private LineRenderer path;

    [Header("CUSTOMIZE")]
    [SerializeField] private float durationMultiplier;

    #region PRIVATE FIELD
    private List<Tween> _tweens;
    #endregion

    private void Awake()
    {
        _tweens = new List<Tween>();

        StartCoroutine(FollowPath());
    }

    private void OnDestroy()
    {
        CommonUtil.StopAllTweens(_tweens);
    }

    private IEnumerator FollowPath()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

        int currentNode = 0;

        bool isNodeEnd = true;
        bool isMovingBackward = false;

        while (true)
        {
            if (isNodeEnd)
            {
                if (!isMovingBackward)
                {
                    currentNode++;

                    if (currentNode == path.positionCount - 1)
                    {
                        isMovingBackward = true;
                    }
                }
                else
                {
                    currentNode--;

                    if (currentNode == 0)
                    {
                        isMovingBackward = false;
                    }
                }

                Vector3 nextPosition = path.GetPosition(currentNode);

                float duration = durationMultiplier * Vector3.Distance(nextPosition, transform.position);

                _tweens.Add(Tween.Position(transform, nextPosition, duration: duration, ease: Ease.Linear)
                .OnComplete(() =>
                {
                    isNodeEnd = true;
                }));

                isNodeEnd = false;
            }
            else
            {

            }

            yield return waitForSeconds;
        }
    }
}
