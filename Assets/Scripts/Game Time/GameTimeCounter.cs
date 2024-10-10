using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeCounter : MonoBehaviour
{
    private float _secondElasped;

    public static event Action<float> scaleEnemyPowerOverTimeEvent;

    private void Awake()
    {
        StartCoroutine(Counting());
    }

    private IEnumerator Counting()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);

        while (true)
        {
            yield return waitForSeconds;

            _secondElasped++;

            if (_secondElasped % 10 == 0)
            {
                scaleEnemyPowerOverTimeEvent?.Invoke(_secondElasped / 10f);
            }
        }
    }
}
