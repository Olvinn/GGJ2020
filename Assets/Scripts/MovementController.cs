using Data.Game;
using System;
using MessengerTrigger;
using UnityEngine;

namespace Controllers.Player
{
    /// <summary>
    /// ����������� CharacterController'a, �������������� � �����
    /// version 2.1.2
    [RequireComponent(typeof(CharacterController))]
    public class MovementController : MonoBehaviour
    {
        /// <summary>
        /// ���������� ��� �����������, �������� - ������ �������
        /// </summary>
        public event Action<float> onGrounded;

        public event Action onJump;
        public MovementState state { get; private set; }

        [SerializeField] PlayerData _data;
        [SerializeField] Transform _visuals;

        CharacterController _characterController;

        //Conditions
        bool _isJumping;
        bool _isSprinting;
        bool _isSliding;
        bool _isFalling;

        float _fallHeight;

        Vector3 _inertia;
        Vector2 _movement;
        Vector3 _groundNormal;

        //Movement modifiers
        float _movementGroundDot;

        //Methods needs
        float speed, power;

        private bool _isBlocked;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _characterController.slopeLimit = 90f;

            Messenger.OnStartMessage += BlockController;
            Messenger.OnEndMessage += UnBlockController;
            UnBlockController();
        }

        private void OnDestroy()
        {
            Messenger.OnStartMessage -= BlockController;
            Messenger.OnEndMessage -= UnBlockController;
        }

        private void BlockController()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _isBlocked = true;
        }

        private void UnBlockController()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _isBlocked = false;
        }

        private void Update()
        {
            if (_isBlocked && state == MovementState.Normal) return;
            DefineState();
            MovementProcessor();
            RotationProcessor();
        }

        private void RotationProcessor()
        {
            float rotationEffector = GetCurrentSpeed() / _data.speed;
            Vector3 temp = _inertia;
            temp.y = 0;
            if (GetCurrentSpeed() > 0 && temp != Vector3.zero)
                _visuals.rotation = Quaternion.Lerp(_visuals.rotation, Quaternion.LookRotation(temp), rotationEffector);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Vector3 origin = hit.point + Vector3.up * _characterController.height * .5f;
            RaycastHit boxHit;
            Vector3 normal = Vector3.zero;
            if (Physics.BoxCast(origin, Vector3.one * .1f, Vector3.down, out boxHit))
                normal = boxHit.normal;

            if (hit.rigidbody)
            {
                speed = Vector3.Dot(hit.moveDirection, hit.normal);
                power = 120 * speed;
                hit.rigidbody.AddForceAtPosition(hit.normal * power, hit.point, ForceMode.Force);
            }

            _movementGroundDot = Vector3.Dot(new Vector3(_movement.x, 0, _movement.y).normalized, _groundNormal);
            _isSliding = false;
            if (_inertia.magnitude > _data.speed + 1 || Vector3.Angle(normal, Vector3.up) > _data.slideAngle)
            {
                _isSliding = true;
                Vector3 dir = Vector3.ProjectOnPlane(_inertia, hit.normal) * (1 - _data.bounceness) +
                              Vector3.Reflect(_inertia, hit.normal) * _data.bounceness;
                _inertia = dir;
            }

            _groundNormal = hit.normal;
        }

        void DefineState()
        {
            if (!_characterController.isGrounded)
            {
                if (!_isFalling)
                {
                    _fallHeight = 0;
                    _isFalling = true;
                }

                state = MovementState.Fall;
                _fallHeight += _inertia.y < 0 ? _inertia.y * Time.deltaTime : 0;
            }
            else
            {
                if (_isFalling)
                {
                    onGrounded?.Invoke(_fallHeight);
                    _isFalling = false;
                }

                if (_isSliding || (_characterController.velocity.magnitude > _data.speed * 1.5f))
                    state = MovementState.Slide;
                else
                    state = MovementState.Normal;
            }
        }

        void MovementProcessor()
        {
            Vector3 newMov = new Vector3(_movement.x, 0, _movement.y).normalized;

            switch (state)
            {
                default:
                case MovementState.Normal:
                {
                    newMov *= _data.speed;
                    newMov = _characterController.transform.localToWorldMatrix * newMov;
                    _inertia = Vector3.MoveTowards(_inertia, newMov, _data.acceleration * Time.deltaTime);
                    if (_isJumping)
                    {
                        onJump?.Invoke();
                        _inertia.y = _data.jumpPower;
                    }

                    _characterController.Move(_inertia * Time.deltaTime);
                    if (!_isJumping)
                        _characterController.Move(Vector3.down * _characterController.stepOffset);
                    break;
                }
                case MovementState.Fall:
                {
                    newMov *= _data.speed;
                    newMov = _characterController.transform.localToWorldMatrix * newMov;
                    newMov.y = 0;

                    float y = _inertia.y;
                    _inertia.y = 0;

                    _inertia = Vector3.MoveTowards(_inertia, newMov, _data.acceleration * Time.deltaTime);

                    _inertia.y = y;
                    _inertia += Physics.gravity * Time.deltaTime;

                    _characterController.Move(_inertia * Time.deltaTime);
                    break;
                }
                case MovementState.Slide:
                {
                    newMov = _characterController.transform.localToWorldMatrix * newMov;
                    _inertia += newMov * Time.deltaTime * _data.acceleration * .1f;
                    _inertia -= _inertia * _data.slideStoppingModifier * Time.deltaTime;
                    _inertia += Physics.gravity * Time.deltaTime * (1 - _movementGroundDot);
                    if (_isJumping)
                    {
                        onJump?.Invoke();
                        _inertia.y = _data.jumpPower;
                    }

                    _characterController.Move(_inertia * Time.deltaTime);
                    break;
                }
            }

            _isJumping = false;
        }

        public void Init(PlayerData data)
        {
            _data = data;
        }

        public void Jump()
        {
            _isJumping = true;
        }

        public void Move(Vector3 movement)
        {
            Move(new Vector2(movement.x, movement.z));
        }

        public void Move(Vector2 movement)
        {
            _movement = movement;
        }

        public float GetCurrentSpeed()
        {
            return _inertia.magnitude;
        }

        public Vector3 GetCurrentVelocity()
        {
            return _inertia;
        }
    }

    public enum MovementState
    {
        Normal = 0,
        Fall,
        Slide
    }
}