using System.Collections;
using System.Collections.Generic;
using Saferio.Util.SaferioTween;
using UnityEngine;

public class SampleSaferioPageViewUsage : MonoBehaviour
{
    [SerializeField] private ObjectPoolingPageView objectPoolingPageView;

    private void Start()
    {
        SaferioTween.Delay(0.2f, onCompletedAction: () => TestApplyAnimation());
    }

    private void TestApplyAnimation()
    {
        SequentialSlide sequentialSlide = new SequentialSlide();

        sequentialSlide.Items = objectPoolingPageView.Items;
        sequentialSlide.RowNumber = objectPoolingPageView.RowNumber;
        sequentialSlide.ColumnNumber = objectPoolingPageView.ColumnNumber;

        objectPoolingPageView.ApplyAnimation(sequentialSlide);
    }
}
