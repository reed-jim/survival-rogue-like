using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPageViewCallback : MonoBehaviour
{
    [SerializeField] private ObjectPoolingPageView pageView;

    private void Awake()
    {
        pageView.OnPageSwitched += TestSwitchPage;
        pageView.OnSlotSelected += TestSelectSlot;
    }

    private void OnDestroy()
    {
        pageView.OnPageSwitched -= TestSwitchPage;
        pageView.OnSlotSelected -= TestSelectSlot;
    }

    private void TestSwitchPage(int page)
    {
        DebugUtil.DistinctLog("Switch to page " + page);
    }

    private void TestSelectSlot(int slot)
    {
        DebugUtil.DistinctLog("Select slot " + slot);
    }
}
