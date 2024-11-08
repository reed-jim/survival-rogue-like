using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class TestIKWalking : MonoBehaviour
{
    [SerializeField] private Transform character;
    [SerializeField] private Transform targetLeftFeet;
    [SerializeField] private Transform targetRightFeet;

    private void Awake()
    {
        StartCoroutine(IKWalking());
    }

    private IEnumerator IKWalking()
    {
        bool isWait = false;
        int side = 0;

        while (true)
        {
            if (!isWait)
            {
                if (side == 0)
                {
                    float endPosition = targetRightFeet.position.z + 0.5f;

                    Tween.PositionZ(targetLeftFeet, endPosition, duration: 1, ease: Ease.Linear)
                    .OnComplete(() =>
                    {
                        Tween.PositionZ(character, endPosition, duration: 0.3f, ease: Ease.Linear)
                        .OnComplete(() =>
                        {
                            side = 1;
                            isWait = false;
                        });
                    });

                    isWait = true;
                }
                else
                {
                    float endPosition = targetLeftFeet.position.z + 0.5f;

                    Tween.PositionZ(targetRightFeet, endPosition, duration: 1, ease: Ease.Linear)
                    .OnComplete(() =>
                    {
                        Tween.PositionZ(character, endPosition, duration: 0.3f, ease: Ease.Linear)
                        .OnComplete(() =>
                        {
                            side = 0;
                            isWait = false;
                        });
                    });

                    isWait = true;
                }
            }

            yield return new WaitForSeconds(0.02f);
        }
    }

}
