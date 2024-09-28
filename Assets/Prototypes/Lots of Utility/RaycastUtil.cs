using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;

public static class RaycastUtil
{
    public static void ClickObject(Action<RaycastHit> actionWithRaycastHit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            actionWithRaycastHit?.Invoke(hit);
        }
    }

    public static float DistanceBetweenTwoObject(Transform origin)
    {
        float distance = 0;

        return distance;
    }

    public static Vector2 GetAxisFromTwoDimensionalMatrix()
    {
        return Vector2.zero;
    }

    public static void Delay(MonoBehaviour target, float delayTime, Action actionOnCompleted)
    {
        target.StartCoroutine(Delaying());

        IEnumerator Delaying()
        {
            yield return new WaitForSeconds(delayTime);

            actionOnCompleted?.Invoke();
        }


    }

    #region RectTransform Snapping
    public static void SnapBottomSideToTopSideOf(RectTransform target, RectTransform reference)
    {
        target.localPosition = new Vector3(0, reference.localPosition.y + 0.5f * (reference.sizeDelta.y + target.sizeDelta.y));
    }

    public static void SnapTopSideToTopSideOf(RectTransform target, RectTransform reference)
    {
        target.localPosition = new Vector3(0, reference.localPosition.y + 0.5f * (reference.sizeDelta.y - target.sizeDelta.y));
    }
    #endregion

    #region Font size
    // This works only for scale, not for position
    public static void ScaleFontSizeAllTexts(Transform container, float referenceCanvasSizeX, float canvasSizeX)
    {
        List<TMP_Text> texts = GetAllComponentsOfChildren<TMP_Text>(container);

        foreach (var text in texts)
        {
            text.fontSize *= canvasSizeX / referenceCanvasSizeX;
        }
    }
    #endregion

    #region ROTATION
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
    #endregion

    #region SPRING ANIMATION
    public static IEnumerator SpringRotatingAnimation(
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
    #endregion

    #region TRANSFORM 
    public static void LockRotation(Transform target, Vector3 initialAngles)
    {
        target.eulerAngles = initialAngles;
    }
    #endregion

    #region PHYSICS
    // avoid receive too much force after collision
    public static void ApplyAutoBrake()
    {

    }

    // GENERATE 2D FROM 3D MODEL

    public static Vector3 GetForceAppliedAfterCollision(Rigidbody rigidbody, Vector3 lastVelocity)
    {
        Vector3 currentVelocity = rigidbody.velocity;
        Vector3 velocityChange = currentVelocity - lastVelocity;

        float collisionDuration = Time.fixedDeltaTime;

        Vector3 acceleration = velocityChange / collisionDuration;

        Vector3 force = rigidbody.mass * acceleration;

        return force;
    }

    public static void ManuallyReduceCollisionForce(Rigidbody rigidbody, float percentToReduce, Vector3 lastVelocity)
    {
        Vector3 receivedForce = GetForceAppliedAfterCollision(rigidbody, lastVelocity);

        Vector3 reversedForce = -percentToReduce * receivedForce;

        rigidbody.AddForce(reversedForce);
    }
    #endregion

    #region MESH
    public static void MergeMesh()
    {

    }
    #endregion

    #region PARTICLE SYSTEM

    #endregion

    #region SPRITE
    public static Vector2 GetSpriteSize(SpriteRenderer sprite)
    {
        return sprite.bounds.size;
    }
    #endregion

    public static Vector3 GetMeshSize(MeshRenderer mesh)
    {
        return mesh.bounds.size;
    }

    public static List<GameObject> GetAllChildren(GameObject root)
    {
        void Searcher(List<GameObject> list, GameObject root)
        {
            list.Add(root);

            if (root.transform.childCount > 0)
            {
                foreach (Transform VARIABLE in root.transform)
                {
                    Searcher(list, VARIABLE.gameObject);
                }
            }
        }

        List<GameObject> result = new List<GameObject>();

        if (root.transform.childCount > 0)
        {
            foreach (Transform VARIABLE in root.transform)
            {
                Searcher(result, VARIABLE.gameObject);
            }
        }

        return result;
    }

    public static List<T> GetAllComponentsOfChildren<T>(Transform root)
    {
        void Searcher(List<T> list, Transform root)
        {
            list.Add(root.GetComponent<T>());

            if (root.childCount > 0)
            {
                foreach (Transform VARIABLE in root)
                {
                    Searcher(list, VARIABLE);
                }
            }
        }

        List<T> result = new List<T>();

        if (root.childCount > 0)
        {
            foreach (Transform VARIABLE in root)
            {
                Searcher(result, VARIABLE);
            }
        }

        return result;
    }
}

public class SampleUtilUsage : MonoBehaviour
{
    private void Use()
    {
        RaycastUtil.ClickObject(actionWithRaycastHit: SelectObject);
    }

    private void SelectObject(RaycastHit hit)
    {
        hit.collider.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void TestDelay()
    {
        RaycastUtil.Delay(this, 1f, () => { });
    }
}
