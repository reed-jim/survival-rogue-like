using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class TestUtil : MonoBehaviour
{
    [Header("TARGET")]
    [SerializeField] private Transform target;
    [SerializeField] private SpriteRenderer testSprite;
    [SerializeField] private SpriteRenderer fadeImage;

    [Header("AMPLITUDE")]
    [SerializeField] private Vector3 rotateAngleRange;
    [SerializeField] private Vector3 scaleRange;
    [SerializeField] private Color colorRange;

    [Header("DAMPEN RATE")]
    [SerializeField] private float dampenRate;

    [SerializeField] private float durationEachStep;

    private bool _isWaiting;

    // #if UNITY_EDITOR
    //     private void Awake()
    //     {
    //         DebugUtil.Log(target.eulerAngles + "/" + durationEachStep, LogImportance.LOW);
    //         DebugUtil.Log(target.eulerAngles + "/" + durationEachStep, LogImportance.NORMAL);
    //         DebugUtil.Log(target.eulerAngles + "/" + durationEachStep, LogImportance.HIGH);
    //     }
    // #endif

    private void Update()
    {
        if (_isWaiting == false)
        {
            // if (Input.GetMouseButton(0))
            // {
            //     // StartCoroutine(SpringAnimation.SpringScaleAnimation(target, scaleRange, 6, durationEachStep));
            //     // StartCoroutine(RaycastUtil.SpringRotatingAnimation(target, rotateAngleRange, 6, durationEachStep));
            //     // StartCoroutine(SpringAnimation.SpringColorAnimation(testSprite, colorRange, 6, durationEachStep));

            //     SpringAnimationChainable spring = new SpringAnimationChainable();

            //     spring.SetTarget(target)
            //         .SetAmplitude(scaleRange)
            //         .SetDampenRate(dampenRate)
            //         .SetStepNumber(6)
            //         .SetDurationEachStep(durationEachStep)
            //         .Run();

            //     _isWaiting = true;

            //     Tween.Delay(durationEachStep * 8).OnComplete(() => _isWaiting = false);
            // }

            if (Input.GetMouseButton(0))
            {
                Tween.Delay(0.8f).OnComplete(() =>
                {
                    target.eulerAngles = Vector3.zero;
                });

                CameraUtil.Fade(fadeImage, 0.5f, 0.8f);
            }
        }
    }
}
