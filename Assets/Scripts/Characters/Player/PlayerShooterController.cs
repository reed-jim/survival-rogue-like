using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooterController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletContainer;

    [SerializeField] private GameObject[] bullets;

    [Header("CUSTOMIZE")]
    [SerializeField] private float forceMultiplier;

    [Header("MANAGEMENT")]
    private int _currentEnemyIndex;
    [SerializeField] private Rigidbody[] bulletRigidBodies;

    private void Awake()
    {
        bullets = new GameObject[10];
        bulletRigidBodies = new Rigidbody[10];

        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(bulletPrefab, bulletContainer);
            bullets[i].SetActive(false);

            bulletRigidBodies[i] = bullets[i].GetComponent<Rigidbody>();
        }
    }

    public void Shoot(Vector3 targetPosition, Transform player)
    {
        Vector3 direction = (targetPosition - player.position).normalized;

        direction.y = 0;

        // Debug.Log(target.name + "/" + target.position + "/" + direction);

        bullets[_currentEnemyIndex].transform.position = player.position + player.forward;
        bullets[_currentEnemyIndex].SetActive(true);

        bulletRigidBodies[_currentEnemyIndex].velocity = Vector3.zero;

        bulletRigidBodies[_currentEnemyIndex].AddForce(forceMultiplier * direction, ForceMode.Force);

        _currentEnemyIndex++;

        if (_currentEnemyIndex >= bullets.Length)
        {
            _currentEnemyIndex = 0;
        }
    }

    public void Shoot(Transform target, Transform player)
    {
        Vector3 bulletStartPoint = player.position + player.forward + new Vector3(0, 1, 0);

        Vector3 direction = (target.position - bulletStartPoint).normalized;

        direction.y = 0;

        // Debug.LogError(target.name + "/" + target.position + "/" + direction);

        bullets[_currentEnemyIndex].transform.position = bulletStartPoint;
        bullets[_currentEnemyIndex].SetActive(true);

        bulletRigidBodies[_currentEnemyIndex].velocity = Vector3.zero;

        bulletRigidBodies[_currentEnemyIndex].AddForce(forceMultiplier * direction, ForceMode.Force);

        _currentEnemyIndex++;

        if (_currentEnemyIndex >= bullets.Length)
        {
            _currentEnemyIndex = 0;
        }
    }
}
