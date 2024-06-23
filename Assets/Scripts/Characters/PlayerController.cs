using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using PrimeTween;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [Header("ANIMATOR")]
    [SerializeField] private Animator playerAnimator;

    [Header("CUSTOMIZE")]
    [SerializeField] private float force;
    [SerializeField] private float deltaSpeed;
    [SerializeField] private float _speedMultiplier;

    [Header("MANAGEMENT")]
    private List<Tween> _tweens;
    private Coroutine _waitCoroutine;
    private bool _isTurning;

    private float _speed;




    // [SerializeField] private AnimancerComponent _Animancer;
    // [SerializeField] private Float1ControllerTransitionAsset.UnShared _Controller;

    // public float Speed
    // {
    //     get => _Controller.State.Parameter;
    //     set => _Controller.State.Parameter = value;
    // }

    // private void OnEnable()
    // {
    //     _Animancer.Play(_Controller);
    // }









    private void Awake()
    {
        _tweens = new List<Tween>();

        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     _rigidbody.AddForce(force * Vector3.forward, ForceMode.Force);

        //     playerAnimator.SetInteger("State", 1);

        //     StopCoroutine(_waitCoroutine);

        //     if (_waitCoroutine == null)
        //     {
        //         _waitCoroutine = StartCoroutine(WaitFor(0.5f, () => playerAnimator.SetInteger("State", 0)));
        //     }
        // }

        if (Input.GetKey(KeyCode.W))
        {
            WalkFoward();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (!_isTurning)
            {
                TransformUtil.RotateRight(transform, _tweens, onCompletedAction: () => _isTurning = false);

                _isTurning = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (!_isTurning)
            {
                TransformUtil.RotateLeft(transform, _tweens, onCompletedAction: () => _isTurning = false);

                _isTurning = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (!_isTurning)
            {
                TransformUtil.RotateBack(transform, _tweens, onCompletedAction: () => _isTurning = false);

                _isTurning = true;
            }
        }
        else
        {
            Idle();

            if (_speed > 0)
            {
                _speed -= deltaSpeed;
            }
            else
            {
                _speed = 0;
            }
        }

        _rigidbody.velocity = _speedMultiplier * _speed * transform.forward;

        playerAnimator.SetFloat("Speed", _speed);

        FaceToMouseCursor();
    }

    private void FaceToMouseCursor()
    {
        // Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // float angle = Vector3.Angle(mousePosition, transform.position);

        // transform.eulerAngles = new Vector3(0, angle, 0);

        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);

        transform.LookAt(hit.point);
    }

    private void WalkFoward()
    {
        // _rigidbody.AddForce(force * Vector3.forward, ForceMode.Impulse);

        // if (playerAnimator.GetInteger("State") == 1) return;

        // playerAnimator.SetInteger("State", 1);

        // if (_waitCoroutine == null)
        // {
        //     _waitCoroutine = StartCoroutine(WaitFor(1.033f, () => playerAnimator.SetInteger("State", 0)));
        // }

        if (_speed <= 1)
        {
            _speed += deltaSpeed;
        }
        else
        {
            _speed = 1;
        }
    }

    private void Idle()
    {
        if (playerAnimator.GetInteger("State") == 0) return;

        playerAnimator.SetInteger("State", 0);
    }

    private IEnumerator WaitFor(float amount, Action onCompletedAction)
    {
        float deltaTime = Time.deltaTime;

        WaitForSeconds waitForSeconds = new WaitForSeconds(deltaTime);

        float time = 0;

        while (time < amount)
        {
            time += deltaTime;

            yield return waitForSeconds;
        }

        onCompletedAction?.Invoke();

        _waitCoroutine = null;
    }
}
