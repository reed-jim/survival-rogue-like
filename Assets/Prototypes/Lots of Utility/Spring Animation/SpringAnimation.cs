using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class SpringAnimation : MonoBehaviour
{
    public static IEnumerator SpringRotatingAnimation
    (
        Transform target,
        Vector3 angleRange,
        int numberStep,
        float durationEachStep
    )
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);

        int step = 0;
        bool isWaiting = false;

        Vector3 initialAngles = target.eulerAngles;

        while (step <= numberStep)
        {
            int remainingStep = numberStep - step;

            if (isWaiting)
            {
                yield return null;

                continue;
            }

            Vector3 endAngle = initialAngles + ((float)remainingStep / numberStep) * angleRange;

            if (step % 2 != 0)
            {
                endAngle = initialAngles - ((float)remainingStep / numberStep) * angleRange;
            }

            endAngle = GetNotNegativeEulerAngle(endAngle);

            Vector3 startAngle = GetNotNegativeEulerAngle(target.eulerAngles);

            Tween.Rotation(target, startAngle, endAngle, duration: durationEachStep)
            .OnComplete(() =>
            {
                isWaiting = false;

                step++;
            });

            isWaiting = true;

            yield return waitForSeconds;
        }
    }

    public static Vector3 GetNotNegativeEulerAngle(Vector3 eulerAngle)
    {
        Vector3 convertedEulerAngle = eulerAngle;

        if (convertedEulerAngle.x < 0)
        {
            convertedEulerAngle.x += 360;
        }

        if (convertedEulerAngle.y < 0)
        {
            convertedEulerAngle.y += 360;
        }

        if (convertedEulerAngle.z < 0)
        {
            convertedEulerAngle.z += 360;
        }

        return convertedEulerAngle;
    }

    public static IEnumerator SpringColorAnimation
    (
        SpriteRenderer spriteRenderer,
        Color colorRange,
        int numberStep,
        float durationEachStep
    )
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);

        int step = 0;
        bool isSetUpForNewCycle = false;

        Color initialColor = spriteRenderer.color;

        Color startColor = Color.white;
        Color endColor = Color.white;
        Color deltaColor = Color.white;

        while (step <= numberStep)
        {
            if (!isSetUpForNewCycle)
            {
                int remainingStep = numberStep - step;

                endColor = initialColor + ((float)remainingStep / numberStep) * colorRange;

                if (step % 2 != 0)
                {
                    endColor = initialColor - ((float)remainingStep / numberStep) * colorRange;
                }

                startColor = spriteRenderer.color;

                deltaColor = (endColor - startColor) / (durationEachStep / 0.02f);

                isSetUpForNewCycle = true;
            }

            if (spriteRenderer.color != endColor)
            {
                spriteRenderer.color += deltaColor;
            }
            else
            {
                isSetUpForNewCycle = false;

                step++;
            }

            yield return waitForSeconds;
        }
    }

    public static IEnumerator SpringColorAnimation
    (
        MaterialPropertyBlock materialPropertyBlock,
        Color colorRange,
        int numberStep,
        float durationEachStep
    )
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);

        int step = 0;
        bool isSetUpForNewCycle = false;

        Color initialColor = materialPropertyBlock.GetColor("_Color");

        Color startColor = Color.white;
        Color endColor = Color.white;
        Color deltaColor = Color.white;

        while (step <= numberStep)
        {
            if (!isSetUpForNewCycle)
            {
                int remainingStep = numberStep - step;

                endColor = initialColor + ((float)remainingStep / numberStep) * colorRange;

                if (step % 2 != 0)
                {
                    endColor = initialColor - ((float)remainingStep / numberStep) * colorRange;
                }

                startColor = materialPropertyBlock.GetColor("_Color");

                deltaColor = (endColor - startColor) / (durationEachStep / 0.02f);

                isSetUpForNewCycle = true;
            }

            if (materialPropertyBlock.GetColor("_Color") != endColor)
            {
                materialPropertyBlock.SetColor("_Color", materialPropertyBlock.GetColor("_Color") + deltaColor);
            }
            else
            {
                isSetUpForNewCycle = false;

                step++;
            }

            yield return waitForSeconds;
        }
    }

    public static IEnumerator SpringPositionAnimation
    (
        Transform target,
        Vector3 amplitude,
        float dampenRate,
        int numberStep,
        float durationEachStep
    )
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);

        int step = 0;
        bool isSetUpForNewCycle = false;

        Vector3 initialValue = target.position;

        Vector3 startValue;
        Vector3 endValue = Vector3.zero;
        Vector3 deltaValue = Vector3.zero;

        float remainingPower = 1;

        while (step <= numberStep)
        {
            if (!isSetUpForNewCycle)
            {
                int remainingStep = numberStep - step;

                endValue = initialValue + remainingPower * amplitude;

                if (step % 2 != 0)
                {
                    endValue = initialValue - remainingPower * amplitude;
                }

                startValue = target.position;

                deltaValue = (endValue - startValue) / (durationEachStep / 0.02f);

                isSetUpForNewCycle = true;
            }

            if (target.position != endValue)
            {
                target.position += deltaValue;
            }
            else
            {
                NextStep();
            }

            yield return waitForSeconds;
        }

        void NextStep()
        {
            ReducePower();

            isSetUpForNewCycle = false;

            step++;
        }

        void ReducePower()
        {
            remainingPower -= Mathf.Clamp(dampenRate, 0, GetMaxDampenRate());

            if (step == numberStep - 1)
            {
                remainingPower = 0;
            }

            remainingPower = Mathf.Clamp(remainingPower, 0, remainingPower);
        }

        float GetMaxDampenRate()
        {
            return 1f / numberStep;
        }
    }

    public static IEnumerator SpringScaleAnimation
    (
        Transform target,
        Vector3 amplitude,
        float dampenRate,
        int numberStep,
        float durationEachStep
    )
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);

        int step = 0;
        bool isSetUpForNewCycle = false;

        Vector3 initialValue = target.localScale;

        Vector3 startValue;
        Vector3 endValue = Vector3.zero;
        Vector3 deltaValue = Vector3.zero;

        float remainingPower = 1;

        while (step <= numberStep)
        {
            if (!isSetUpForNewCycle)
            {
                int remainingStep = numberStep - step;

                endValue = initialValue + remainingPower * amplitude;

                if (step % 2 != 0)
                {
                    endValue = initialValue - remainingPower * amplitude;
                }

                startValue = target.localScale;

                deltaValue = (endValue - startValue) / (durationEachStep / 0.02f);

                isSetUpForNewCycle = true;
            }

            if (target.localScale != endValue)
            {
                target.localScale += deltaValue;
            }
            else
            {
                NextStep();
            }

            yield return waitForSeconds;
        }

        void NextStep()
        {
            ReducePower();

            isSetUpForNewCycle = false;

            step++;
        }

        void ReducePower()
        {
            remainingPower -= Mathf.Clamp(dampenRate, 0, GetMaxDampenRate());

            if (step == numberStep - 1)
            {
                remainingPower = 0;
            }

            remainingPower = Mathf.Clamp(remainingPower, 0, remainingPower);
        }

        float GetMaxDampenRate()
        {
            return 1f / numberStep;
        }
    }
}

public class SpringAnimationChainable
{
    private Transform _target;
    private Vector3 _amplitude;
    private float _dampenRate;
    private int _numberStep;
    private float _durationEachStep;

    public SpringAnimationChainable SetTarget(Transform target)
    {
        _target = target;

        return this;
    }

    public SpringAnimationChainable SetAmplitude(Vector3 range)
    {
        _amplitude = range;

        return this;
    }

    public SpringAnimationChainable SetDampenRate(float dampenRate)
    {
        _dampenRate = dampenRate;

        return this;
    }

    public SpringAnimationChainable SetStepNumber(int number)
    {
        _numberStep = number;

        return this;
    }

    public SpringAnimationChainable SetDurationEachStep(float durationEachStep)
    {
        _durationEachStep = durationEachStep;

        return this;
    }

    public void Run()
    {
        CoroutineManager.Start(SpringAnimation.SpringScaleAnimation(_target, _amplitude, _dampenRate, _numberStep, _durationEachStep));
    }
}

public class CoroutineManager : MonoBehaviour
{
    public static Coroutine Start(IEnumerator coroutine)
    {
        return new GameObject("CoroutineManager").AddComponent<CoroutineManager>().StartCoroutine(coroutine);
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
