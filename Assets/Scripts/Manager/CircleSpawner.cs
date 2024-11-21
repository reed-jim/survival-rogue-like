using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CircleSpawner
{
    [Header("CUSTOMIZE")]
    [SerializeField] private float radius;
    [SerializeField] private float deltaPercent;

    private List<Vector3> _spawnPositionPool;

    private float _currentX;
    private float _currentY;
    private int _currentDirection;

    public float Radius
    {
        get => radius;
        set => radius = value;
    }

    public float DeltaPercent
    {
        get => deltaPercent;
        set => deltaPercent = value;
    }

    public void Setup(float radius, float deltaPercent)
    {
        this.radius = radius;
        this.deltaPercent = deltaPercent;

        _spawnPositionPool = new List<Vector3>();

        FillSpawnPositionPool();

        _currentDirection = -1;
        _currentX = _currentDirection * radius;
    }

    public Vector3 GetSpawnPosition(float initialPositionY, Vector3 offset)
    {
        Vector3 position;

        position.x = _currentX;
        position.y = initialPositionY;
        position.z = _currentDirection * Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(_currentX, 2));

        if (_currentDirection == -1)
        {
            _currentX += deltaPercent * radius;

            if (_currentX > radius)
            {
                _currentDirection = 1;
            }
        }
        else if (_currentDirection == 1)
        {
            _currentX -= deltaPercent * radius;

            if (_currentX < -radius)
            {
                _currentDirection = -1;

            }
        }

        _currentX = Mathf.Clamp(_currentX, -radius, radius);

        return position + offset;
    }

    private void FillSpawnPositionPool()
    {
        for (float i = -radius; i < radius; i += deltaPercent * radius)
        {
            Vector3 position;

            position.x = i;
            position.y = 0;
            position.z = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(i, 2));

            _spawnPositionPool.Add(position);
        }

        for (float i = -radius; i < radius; i += deltaPercent * radius)
        {
            Vector3 position;

            position.x = i;
            position.y = 0;
            position.z = -Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(i, 2));

            _spawnPositionPool.Add(position);
        }
    }

    public Vector3 GetRandomSpawnPositionFromPool(float initialPositionY, Vector3 offset)
    {
        int randomIndex = UnityEngine.Random.Range(0, _spawnPositionPool.Count);

        Vector3 position = _spawnPositionPool[randomIndex];

        _spawnPositionPool.RemoveAt(randomIndex);

        if (_spawnPositionPool.Count == 0)
        {
            FillSpawnPositionPool();
        }

        position += new Vector3(0, initialPositionY, 0);
        position += offset;

        return position;
    }
}
