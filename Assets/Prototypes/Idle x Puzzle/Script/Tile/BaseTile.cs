using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class BaseTile : MonoBehaviour, ITweenDisposable
{
    protected List<Tween> _tweens;

    private void Awake()
    {
        _tweens = new List<Tween>();
    }

    protected virtual void Init() {

    }

    public void StopAllTweens()
    {
        CommonUtil.StopAllTweens(_tweens);
    }
}
