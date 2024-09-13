using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle.Merge
{
    public class InputManager : MonoBehaviour
    {
        [Header("CUSTOMIZE")]
        [SerializeField] private LayerMask checkInputLayerMask;

        #region PRIVATE FIELD
        private List<Vector3> _mousePositions;
        #endregion

        #region ACTION 
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
                if (Input.GetMouseButton(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, checkInputLayerMask))
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
                    }
                }
                else
                {
                    if (_mousePositions.Count == 2)
                    {
                        Vector3 direction;

                        if (Mathf.Abs(_mousePositions[1].x - _mousePositions[0].x) > Mathf.Abs(_mousePositions[1].z - _mousePositions[0].z))
                        {
                            if (_mousePositions[1].x > _mousePositions[0].x)
                            {
                                direction = Vector3.right;
                            }
                            else
                            {
                                direction = Vector3.left;
                            }
                        }
                        else
                        {
                            if (_mousePositions[1].z > _mousePositions[0].z)
                            {
                                direction = Vector3.forward;
                            }
                            else
                            {
                                direction = Vector3.back;
                            }
                        }

                        moveBlockEvent?.Invoke(selectedBlock.GetInstanceID(), direction);

                        _mousePositions.Clear();
                    }
                }

                yield return waitForSeconds;
            }
        }
    }
}
