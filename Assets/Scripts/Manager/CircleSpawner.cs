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
}
