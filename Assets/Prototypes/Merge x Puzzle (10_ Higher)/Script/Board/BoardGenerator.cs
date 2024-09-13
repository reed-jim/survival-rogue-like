using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle.Merge
{
    public class BoardGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private Transform tileContainer;
        [SerializeField] private GameObject[] tiles;

        [Header("WALL")]
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private Transform wallContainer;
        [SerializeField] private GameObject[] walls;

        [Header("SCRIPTABLE OBJECT")]
        [SerializeField] private BoardProperty boardProperty;

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

            BlockSpawner.getSpawnPositionEvent += GetSpawnPosition;

            Init();

            Generate();
        }

        private void OnDestroy()
        {
            BlockSpawner.getSpawnPositionEvent -= GetSpawnPosition;
        }

        private void Init()
        {
            _maxTile = maxRow * maxColumn;

            SpawnTile();
            // SpawnWall();
        }

        private void SpawnTile()
        {
            tiles = new GameObject[_maxTile];

            for (int i = 0; i < _maxTile; i++)
            {
                tiles[i] = Instantiate(tilePrefab, tileContainer);

                tiles[i].GetComponent<InstanceMaterialPropertyBlock>().Init();
            }
        }

        private void SpawnWall()
        {
            walls = new GameObject[4];

            for (int i = 0; i < 4; i++)
            {
                walls[i] = Instantiate(wallPrefab, wallContainer);
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

            boardProperty.MaxRow = maxRow;
            boardProperty.MaxColumn = maxColumn;
            boardProperty.TileSize = tileSize;
            boardProperty.TileDistance = tileDistance * tileSize;
            boardProperty.FirstTilePosition = tiles[0].transform.position;

            // Vector2 boardSize;

            // boardSize.x = maxColumn * tileSize.x + (maxColumn - 1) * (tileDistance - 1) * tileSize.x;
            // boardSize.y = maxRow * tileSize.z + (maxRow - 1) * (tileDistance - 1) * tileSize.z;

            // for (int i = 0; i < walls.Length; i++)
            // {
            //     if (i % 2 == 0)
            //     {
            //         walls[i].transform.localScale = new Vector3(boardSize.x, 10, 1);
            //     }
            //     else
            //     {
            //         walls[i].transform.localScale = new Vector3(1, 10, boardSize.y);
            //     }
            // }

            // walls[0].transform.position = new Vector3(0, 0, 0.51f * (boardSize.y + walls[0].transform.localScale.z));
            // walls[1].transform.position = new Vector3(0.51f * (boardSize.x + walls[1].transform.localScale.x), 0, 0);
            // walls[2].transform.position = new Vector3(0, 0, -0.51f * (boardSize.y + walls[2].transform.localScale.z));
            // walls[3].transform.position = new Vector3(-0.51f * (boardSize.x + walls[3].transform.localScale.x), 0, 0);
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

        private Vector3 GetSpawnPosition()
        {
            return tiles[0].transform.position;
        }
    }
}
