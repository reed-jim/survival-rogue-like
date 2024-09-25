using System.Collections.Generic;
using UnityEditor;
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
        [SerializeField] private Color[] colors;

        #region PRIVATE FIELD
        private int _maxTile;
        #endregion

        public bool IsPaintMode;

        private void Awake()
        {
            BlockSpawner.getSpawnPositionEvent += GetSpawnPosition;

            GetTiles();

            SetTileColors();
        }

        private void OnDestroy()
        {
            BlockSpawner.getSpawnPositionEvent -= GetSpawnPosition;
        }

        public void Init()
        {
            _maxTile = maxRow * maxColumn;

            SpawnTile();
        }

        private void GetTiles()
        {
            tiles = new GameObject[tileContainer.childCount];

            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = tileContainer.GetChild(i).gameObject;
            }
        }

        private void SpawnTile()
        {
            tiles = new GameObject[_maxTile];

            for (int i = 0; i < _maxTile; i++)
            {
                int x = i % maxColumn;
                int y = (i - x) / maxColumn;

                tiles[i] = Instantiate(tilePrefab, tileContainer);
                tiles[i].name = $"Tile {y} - {x}";
            }
        }

        public void ClearTile()
        {
            List<GameObject> children = new List<GameObject>();

            for (int i = 0; i < tileContainer.childCount; i++)
            {
                children.Add(tileContainer.GetChild(i).gameObject);
            }

            foreach (var item in children)
            {
                DestroyImmediate(item);
            }
        }

        public void Generate()
        {
            Vector3 tileSize = tilePrefab.transform.localScale;

            for (int i = 0; i < maxRow; i++)
            {
                for (int j = 0; j < maxColumn; j++)
                {
                    int index = j + maxColumn * i;

                    Vector3 position = new Vector3();

                    position.x = -(((maxColumn - 1) / 2f) * tileDistance * tileSize.x) + tileDistance * j * tileSize.x;
                    position.y = -0.5f * tileSize.y;
                    position.z = -(((maxRow - 1) / 2f) * tileDistance * tileSize.z) + tileDistance * i * tileSize.z;

                    tiles[index].transform.position = position;

                    // tiles[index].GetComponent<InstanceMaterialPropertyBlock>().SetColor(ColorUtil.GetGradientColor(colorOne, colorTwo, (float)index / _maxTile));

                    int tileRichness = ColorUtil.GetRandomNumber();

                    // tiles[index].GetComponent<InstanceMaterialPropertyBlock>().SetColor(ColorUtil.GetGradientColor(colorOne, colorTwo, tileRichness / 100f));
                    tiles[index].GetComponent<InstanceMaterialPropertyBlock>().SetColor(colors[Random.Range(0, colors.Length)]);

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
        }

        private void SetTileColors()
        {
            for (int i = 0; i < maxRow; i++)
            {
                for (int j = 0; j < maxColumn; j++)
                {
                    int index = j + maxColumn * i;

                    tiles[index].GetComponent<InstanceMaterialPropertyBlock>().SetColor(ColorUtil.GetGradientColor(colorOne, colorTwo, (float)index / tiles.Length));
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

        private Vector3 GetSpawnPosition()
        {
            return tiles[0].transform.position;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(BoardGenerator))]
    public class BoardGeneratorEditor : Editor
    {
        BoardGenerator _boardGenerator;

        private bool isClicking = false;

        private void OnSceneGUI()
        {
            // gizmoz is required
            if (_boardGenerator.IsPaintMode)
            {
                Event e = Event.current;
                
                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.GetComponent<ITile>() != null)
                        {
                            hit.collider.gameObject.SetActive(false);

                            e.Use();
                        }
                    }
                }
            }
        }

        private void OnEnable()
        {
            _boardGenerator = (BoardGenerator)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate Board"))
            {
                _boardGenerator.Init();
                _boardGenerator.Generate();
            }

            if (GUILayout.Button("Clear Tile"))
            {
                _boardGenerator.ClearTile();
            }

            _boardGenerator.IsPaintMode = GUILayout.Toggle(_boardGenerator.IsPaintMode, "Is Paint Mode");
        }

        private void PaintDisable(Vector2 mousePosition, Camera camera)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.GetComponent<ITile>() != null)
                {
                    hit.collider.gameObject.SetActive(false);
                }
            }
        }
    }
#endif
}
