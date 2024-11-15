using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHeadIKController : MonoBehaviour
{
    [SerializeField] private Transform source;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mouseScreenPosition = Input.mousePosition;

            mouseScreenPosition.z = -_mainCamera.transform.position.z;

            Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mouseScreenPosition);

            // camera offset, because the camera is not at (0, 0, 0)
            mouseWorldPosition -= _mainCamera.transform.position;

            mouseWorldPosition.z = source.position.z;

            source.position = mouseWorldPosition;
        }
        // useGUILayout render texture to render mesh on canvas
    }
}
