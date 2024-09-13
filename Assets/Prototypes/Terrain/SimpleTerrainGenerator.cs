using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTerrainGenerator : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float baseNoiseScale = 20f;
    public float mountainNoiseScale = 60f;
    public float mountainIntensity = 0.2f;
    public Terrain terrain;

    [Header("CUSTOMIZE")]
    [SerializeField] private float heightMultiplier;


    [Header("TEXTURE")]
    public Texture2D[] textures;

    void Start()
    {
        GenerateTerrain();

        ApplyTextures();
    }

    void GenerateTerrain()
    {
        terrain.terrainData = CreateTerrain();
    }

    TerrainData CreateTerrain()
    {
        TerrainData terrainData = new TerrainData
        {
            heightmapResolution = width + 1,
            size = new Vector3(width, heightMultiplier, height) // Adjust height as needed
        };

        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateBaseNoise()
    {
        float[,] noise = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x / width * baseNoiseScale;
                float yCoord = (float)y / height * baseNoiseScale;
                noise[x, y] = Mathf.PerlinNoise(xCoord, yCoord);
            }
        }
        return noise;
    }

    float[,] GenerateMountainNoise()
    {
        float[,] noise = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x / width * mountainNoiseScale;
                float yCoord = (float)y / height * mountainNoiseScale;
                noise[x, y] = Mathf.PerlinNoise(xCoord, yCoord);
            }
        }
        return noise;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        float[,] baseNoise = GenerateBaseNoise();
        float[,] mountainNoise = GenerateMountainNoise();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float baseHeight = baseNoise[x, y];
                float mountainHeight = mountainNoise[x, y] * mountainIntensity;

                if (mountainHeight > 0.1f) // Adjust threshold as needed
                {
                    heights[x, y] = Mathf.Clamp01(baseHeight + mountainHeight);
                }
                else
                {
                    heights[x, y] = Mathf.Clamp01(baseHeight);
                }
            }
        }
        return heights;
    }

    void ApplyTextures()
    {
        // Create new TerrainLayer array
        TerrainLayer[] terrainLayers = new TerrainLayer[textures.Length];

        for (int i = 0; i < textures.Length; i++)
        {
            TerrainLayer layer = new TerrainLayer();
            layer.diffuseTexture = textures[i];
            layer.tileSize = new Vector2(width / textures.Length, height / textures.Length); // Adjust as needed

            terrainLayers[i] = layer;
        }

        // Apply the terrain layers
        terrain.terrainData.terrainLayers = terrainLayers;
    }
}
