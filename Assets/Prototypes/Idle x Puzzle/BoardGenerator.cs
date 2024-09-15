using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform tileContainer;
    [SerializeField] private GameObject[] tiles;

    [Header("CUSTOMIZE")]
    [SerializeField] private int maxRow;
    [SerializeField] private int maxColumn;
    [SerializeField] private float tileDistance;

    [Header("COLOR")]
    [SerializeField] private Color colorOne;
    [SerializeField] private Color colorTwo;

    #region PRIVATE FIELD
    private MeshFilter _tileMeshFilter;
    private int _maxTile;
    #endregion

    private void Awake()
    {
        _tileMeshFilter = tilePrefab.GetComponent<MeshFilter>();

        Init();

        Generate();

        bool[] isTileChecked = new bool[_maxTile];

        // FillOut(0, 0, 0, 0, isTileChecked);
    }

    private void Init()
    {
        _maxTile = maxRow * maxColumn;

        tiles = new GameObject[_maxTile];

        for (int i = 0; i < _maxTile; i++)
        {
            tiles[i] = Instantiate(tilePrefab, tileContainer);
        }
    }

    private void Generate()
    {
        Vector3 tileSize = tilePrefab.transform.localScale;

        for (int i = 0; i < maxRow; i++)
        {
            for (int j = 0; j < maxColumn; j++)
            {
                int index = j + maxColumn * i;

                Vector3 position = new Vector3();

                position.x = -(((maxColumn - 1) / 2f) * tileDistance * tileSize.x) + tileDistance * j * tileSize.x;
                position.y = 0.5f * tileSize.y;
                position.z = -(((maxRow - 1) / 2f) * tileDistance * tileSize.z) + tileDistance * i * tileSize.z;

                tiles[index].transform.position = position;

                // tiles[index].GetComponent<InstanceMaterialPropertyBlock>().SetColor(ColorUtil.GetGradientColor(colorOne, colorTwo, (float)index / _maxTile));

                int tileRichness = ColorUtil.GetRandomNumber();

                tiles[index].GetComponent<InstanceMaterialPropertyBlock>().SetColor(ColorUtil.GetGradientColor(colorOne, colorTwo, tileRichness / 100f));

                ITile tileComponent = tiles[index].GetComponent<ITile>();

                tileComponent.X = j;
                tileComponent.Y = i;
                tileComponent.Position = position;
                tileComponent.TileRichness = tileRichness;
            }
        }
    }

    private void FillOut(int iteratorIndex, int numFill, int x, int y, bool[] isTileChecked)
    {
        List<Vector2Int> movableTiles = new List<Vector2Int>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int index = j + i * y;

                if (index >= 0 && index < _maxTile)
                {
                    if (!isTileChecked[index])
                    {
                        movableTiles.Add(new Vector2Int(j, i));
                    }
                }
            }
        }

        Vector2Int randomTile = movableTiles[Random.Range(0, movableTiles.Count)];
        int randomIndex = randomTile.x + randomTile.y * y;

        tiles[randomIndex].gameObject.SetActive(false);

        isTileChecked[randomIndex] = true;

        numFill++;

        if (numFill > 8)
        {
            iteratorIndex++;

            numFill = 0;

            if (iteratorIndex > 3)
            {
                return;
            }
        }

        FillOut(iteratorIndex, numFill, x + randomTile.x, y + randomTile.y, isTileChecked);

        return;
    }
}
