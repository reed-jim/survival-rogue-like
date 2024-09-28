using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleEventSubcriber : MonoBehaviour
{
    private void Awake()
    {
        SampleEventPublisher.sampleEvent += SampleSubcribe;
    }

    private void OnDestroy()
    {
        SampleEventPublisher.sampleEvent -= SampleSubcribe;
    }

    private void SampleSubcribe()
    {
        Debug.Log("Sample Event");
    }
}
