using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class BoardAnimation : MonoBehaviour
{
    [SerializeField] private Puzzle.Merge.BoardGenerator boardGenerator;

    [Header("CUSTOMIZE")]
    [SerializeField] private Vector3 amplitude;
    [SerializeField] private float duration;
    [SerializeField] private float delayBetweenEachTile;

    private void Awake()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        WaitForSeconds waitForDeltaTime = new WaitForSeconds(Time.deltaTime);
        WaitForSeconds waitForBetweenEachTile = new WaitForSeconds(delayBetweenEachTile);

        int currentIndex = 0;

        Vector3 initialScale = boardGenerator.Tiles[currentIndex].transform.localScale;

        yield return new WaitForSeconds(0.05f);

        foreach (var item in boardGenerator.Tiles)
        {
            item.transform.localScale = Vector3.zero;
        }

        while (currentIndex < boardGenerator.Tiles.Length)
        {
            if (boardGenerator.Tiles[currentIndex].activeSelf)
            {
                boardGenerator.Tiles[currentIndex].transform.localScale = Vector3.zero;

                Tween.Scale(boardGenerator.Tiles[currentIndex].transform, initialScale, duration: 0.2f);

                // StartCoroutine(
                //     SpringAnimation.SpringPositionAnimation
                //     (
                //         boardGenerator.Tiles[currentIndex].transform,
                //         amplitude,
                //         0.1f,
                //         6,
                //         duration
                //     )
                // );

                yield return waitForBetweenEachTile;
            }

            currentIndex++;

            yield return waitForDeltaTime;
        }
    }
}
