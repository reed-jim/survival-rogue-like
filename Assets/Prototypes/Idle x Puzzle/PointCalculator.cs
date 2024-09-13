using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointCalculator : MonoBehaviour
{
    [SerializeField] private TMP_Text pointText;

    [SerializeField] private List<IBlock> blocks;

    #region PRIVATE FIELD
    private int _totalPoint;
    #endregion

    private void Awake()
    {
        blocks = new List<IBlock>();

        BuildManager.addBlockEvent += AddBlock;

        StartCoroutine(CalculatingPoint());
    }

    private void OnDestroy()
    {
        BuildManager.addBlockEvent -= AddBlock;
    }

    private IEnumerator CalculatingPoint()
    {
        WaitForSeconds waitOneSeconds = new WaitForSeconds(1);

        while (true)
        {
            foreach (var block in blocks)
            {
                block.ShowPoint();
                
                _totalPoint += block.Point;

                pointText.text = $"{_totalPoint}";
            }

            yield return waitOneSeconds;
        }
    }

    private void AddBlock(IBlock block)
    {
        if (blocks == null)
        {
            blocks = new List<IBlock>();
        }

        blocks.Add(block);
    }
}
