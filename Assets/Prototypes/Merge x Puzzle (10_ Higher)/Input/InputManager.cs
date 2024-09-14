using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

namespace Puzzle.Merge
{
    public class InputManager : MonoBehaviour
    {
        [Header("SCRIPTABLE OBJECT")]
        [SerializeField] private BoardProperty boardProperty;

        [Header("CUSTOMIZE")]
        [SerializeField] private LayerMask checkInputLayerMask;

        #region PRIVATE FIELD
        private List<Vector3> _mousePositions;
        private bool _isStopInput;
        #endregion

        #region ACTION 
        public static event Action<Vector3, Vector3> spawnBlockEvent;
        public static event Action<int, Vector3> moveBlockEvent;
        #endregion

        private void Awake()
        {
            _mousePositions = new List<Vector3>();

            StartCoroutine(MoveBlock());
        }

        // void Update()
        // {
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //         RaycastHit hit;

        //         if (Physics.Raycast(ray, out hit))
        //         {
        //             AnimationUtil.ScaleUpDown(hit.collider.transform, 1.05f, duration: 0.1f);


        //         }
        //     }
        // }

        private IEnumerator MoveBlock()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);

            GameObject selectedBlock = null;

            while (true)
            {
                if (_isStopInput)
                {
                    yield return waitForSeconds;

                    continue;
                }

                if (Input.GetMouseButton(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit, checkInputLayerMask))
                    {
                        if (_mousePositions.Count == 0)
                        {
                            selectedBlock = hit.collider.gameObject;

                            _mousePositions.Add(hit.point);
                        }
                        if (_mousePositions.Count == 1)
                        {
                            _mousePositions.Add(hit.point);
                        }
                        else
                        {
                            _mousePositions[1] = hit.point;
                        }

                        Vector3 direction = InputToDirection(_mousePositions[0]);

                        spawnBlockEvent?.Invoke(_mousePositions[0], direction);

                        _mousePositions.Clear();

                        _isStopInput = true;

                        Tween.Delay(0.5f).OnComplete(() => _isStopInput = false);
                    }
                }
                // else
                // {
                //     if (_mousePositions.Count == 2)
                //     {
                //         Vector3 direction = InputToDirection(_mousePositions[0]);

                //         Debug.Log(direction);

                //         moveBlockEvent?.Invoke(selectedBlock.GetInstanceID(), direction);

                //         _mousePositions.Clear();
                //     }
                // }

                yield return waitForSeconds;
            }
        }

        private Vector3 InputToDirection(Vector3 mousePosition)
        {
            Vector3 direction = new Vector3();

            if (mousePosition.x > boardProperty.MinPositionX && mousePosition.x < boardProperty.MaxPositionX)
            {
                if (mousePosition.z < boardProperty.MinPositionZ)
                {
                    direction = Vector3.forward;
                }
                else if (mousePosition.z > boardProperty.MaxPositionZ)
                {
                    direction = Vector3.back;
                }
            }
            else
            {
                if (mousePosition.x < boardProperty.MinPositionX)
                {
                    direction = Vector3.right;
                }
                else if (mousePosition.x > boardProperty.MaxPositionX)
                {
                    direction = Vector3.left;
                }
            }

            return direction;
        }

        // private Vector3 InputToDirection(List<Vector3> mousePositions)
        // {
        //     if (Mathf.Abs(_mousePositions[1].x - _mousePositions[0].x) > Mathf.Abs(_mousePositions[1].z - _mousePositions[0].z))
        //     {
        //         if (_mousePositions[1].x > _mousePositions[0].x)
        //         {
        //             direction = Vector3.right;
        //         }
        //         else
        //         {
        //             direction = Vector3.left;
        //         }
        //     }
        //     else
        //     {
        //         if (_mousePositions[1].z > _mousePositions[0].z)
        //         {
        //             direction = Vector3.forward;
        //         }
        //         else
        //         {
        //             direction = Vector3.back;
        //         }
        //     }
        // }
    }
}
