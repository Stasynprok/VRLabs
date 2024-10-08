using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PhysicsHand : MonoBehaviour
{
    [Header("PID")]
    [SerializeField] private float _frequency = 50.0f;
    [SerializeField] private float _damping = 1.0f;
    [SerializeField] private float _rotFrequency = 100f;
    [SerializeField] private float _rotDamping = 0.9f;

    [Space]
    [Header("Settings")]
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private Transform _target;
    [SerializeField] private XRDirectInteractor _interactor;

    [Space]
    [Header("Springs")]
    [SerializeField] private float _climbForce = 1000f;
    [SerializeField] private float _climbDrag = 500f;

    private Rigidbody _rigidbody;
    private Vector3 _previousPosition;
    private bool _isColliding;
    private bool _isObjectGrabbed;
    private Collider _collider;

    private void Start()
    {
        transform.position = _target.position;
        transform.rotation = _target.rotation;
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();  
        _rigidbody.maxAngularVelocity = float.PositiveInfinity;
        _previousPosition = transform.position;
    }

    private void OnEnable()
    {
        _interactor.selectEntered.AddListener(OnGrabObject);
        _interactor.selectExited.AddListener(OnReleseObject);
    }
    private void OnDisable()
    {
        _interactor.selectEntered.RemoveListener(OnGrabObject);
        _interactor.selectExited.RemoveListener(OnReleseObject);
    }

    private void OnGrabObject(SelectEnterEventArgs arg0)
    {
        arg0.interactableObject.transform.gameObject.layer = LayerMask.NameToLayer("Grabbable");

        _collider.enabled = false;
        _isObjectGrabbed = true;
    }

    private void OnReleseObject(SelectExitEventArgs arg0)
    {
        arg0.interactableObject.transform.gameObject.layer = LayerMask.NameToLayer("Default");
        _collider.enabled = true;
        _isObjectGrabbed = false;
    }

    private void FixedUpdate()
    {
        PIDMovement();
        PIDRotation();
        

        if (_isColliding && !_isObjectGrabbed)
        {
            HookesLaw();
        }
    }

    private void PIDMovement()
    {
        float kp = (6.0f * _frequency) * (6.0f * _frequency) * 0.25f;
        float kd = 4.5f * _frequency * _damping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Vector3 force = (_target.position - transform.position) * ksg + (_playerRigidbody.velocity  - _rigidbody.velocity) * kdg;

        _rigidbody.AddForce(force, ForceMode.Acceleration);
    }

    private void PIDRotation()
    {
        float kp = (6f * _rotFrequency) * (6f * _rotFrequency) * 0.25f;
        float kd = 4.5f * _rotFrequency * _rotDamping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Quaternion q = _target.rotation * Quaternion.Inverse(transform.rotation);

        if (q.w < 0)
        {
            q.x = -q.x;
            q.y = -q.y;
            q.z = -q.z;
            q.w = -q.w;
        }
        q.ToAngleAxis(out float angle, out Vector3 axis);
        axis.Normalize();
        axis *= Mathf.Deg2Rad;
        Vector3 torque = ksg * axis * angle + -_rigidbody.angularVelocity * kdg;
        _rigidbody.AddTorque(torque, ForceMode.Acceleration);
    }

    private void HookesLaw()
    {
        Vector3 displacementFromResting = transform.position - _target.position;
        Vector3 force = displacementFromResting * _climbForce;

        float drag = GetDrag();

        _playerRigidbody.AddForce(force, ForceMode.Acceleration);
        Vector3 anotherForce = drag * -_playerRigidbody.velocity * _climbDrag;
        _playerRigidbody.AddForce(anotherForce, ForceMode.Acceleration);
    }

    private float GetDrag()
    {
        Vector3 handVelocity = (_target.localPosition - _previousPosition) / Time.fixedDeltaTime;
        float drag = 1 / handVelocity.magnitude + 0.01f;

        drag = drag > 1 ? 1 : drag;
        drag = drag < 0.03f ? 0.03f : drag;
        _previousPosition = transform.position;
        return drag;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _isColliding = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        _isColliding = false;
    }
}
