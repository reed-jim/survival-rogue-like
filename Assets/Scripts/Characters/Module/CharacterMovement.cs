using System;
using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("CUSTOMIZE")]
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float deltaSpeed;

    #region PRIVATE FIELD
    private Rigidbody _rigidbody;
    private float _speed;
    private bool _isAllowRotating = true;
    private Coroutine _rotateCoroutine;
    private float _initialPositionY;
    #endregion

    #region ACTION
    public static event Action<int, float> setSpeedPropertyAnimation;
    #endregion

    #region LIFE CYCLE  
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _initialPositionY = transform.position.y;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            WalkFoward();

            // if (_isAttacking == false)
            // {
            //     WalkFoward();
            // }
        }
        else
        {
            if (_speed > 0)
            {
                _speed -= deltaSpeed;
            }
            else
            {
                _speed = 0;
            }
        }

        _rigidbody.velocity = speedMultiplier * _speed * transform.forward;

        setSpeedPropertyAnimation?.Invoke(gameObject.GetInstanceID(), _speed);

        FaceToMouseCursor();
    }
    #endregion

    private void WalkFoward()
    {
        if (_speed <= 1)
        {
            _speed += deltaSpeed;
        }
        else
        {
            _speed = 1;
        }
    }

    private void FaceToMouseCursor()
    {
        if (_isAllowRotating == false)
        {
            return;
        }

        Vector3 initialEulerAngles = transform.eulerAngles;

        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);

        transform.LookAt(hit.point);

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        // SMOOTH
        Vector3 endEulerAngles = transform.eulerAngles;

        if (endEulerAngles.y < 0)
        {
            endEulerAngles = new Vector3(0, endEulerAngles.y + 360, 0);
        }

        transform.eulerAngles = initialEulerAngles;

        float deltaEulerAngle = Mathf.Abs(endEulerAngles.y - initialEulerAngles.y);

        if (deltaEulerAngle < 5)
        {
            return;
        }

        if (_rotateCoroutine != null)
        {
            StopCoroutine(_rotateCoroutine);
        }

        _rotateCoroutine = StartCoroutine(SmoothRotateY(transform, initialEulerAngles.y, endEulerAngles.y));

        transform.position = new Vector3(transform.position.x, _initialPositionY, transform.position.z);

        _isAllowRotating = false;
    }

    private IEnumerator SmoothRotateY(Transform target, float startAngle, float endAngle)
    {
        float requiredAngleRotated = 0;
        float angleRotated = 0;

        bool isClockWise = endAngle - startAngle > 0 ? true : false;
        bool isMoveAlongSmallerArc = false;

        requiredAngleRotated = Mathf.Abs(endAngle - startAngle);

        if (startAngle < 180 && endAngle > 180)
        {
            isMoveAlongSmallerArc = true;

            if (startAngle + (360 - endAngle) < requiredAngleRotated)
            {
                isClockWise = !isClockWise;

                requiredAngleRotated = startAngle + (360 - endAngle);
            }
        }

        if (startAngle > 180 && endAngle < 180)
        {
            isMoveAlongSmallerArc = true;

            if (endAngle + (360 - startAngle) < requiredAngleRotated)
            {
                isClockWise = !isClockWise;

                requiredAngleRotated = endAngle + (360 - startAngle);
            }
        }

        _isAllowRotating = false;

        float deltaAngle = requiredAngleRotated / 10f;

        while (angleRotated < requiredAngleRotated)
        {
            if (isClockWise)
            {
                target.eulerAngles += new Vector3(0, deltaAngle, 0);

                angleRotated += deltaAngle;
            }
            else
            {
                target.eulerAngles -= new Vector3(0, deltaAngle, 0);

                angleRotated += deltaAngle;
            }

            yield return new WaitForFixedUpdate();
        }

        _isAllowRotating = true;
    }
}
