using Data.Game;
using System;
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
        float _speedModifier;

        //Methods needs
        float speed, power;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _characterController.slopeLimit = 90f;
        }

        private void Update()
        {
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
            _speedModifier = _movementGroundDot <= 0f ? 1 + _movementGroundDot : 1;
            _isSliding = false;
            if (_inertia.magnitude > _data.sprintSpeed + 1 || Vector3.Angle(normal, Vector3.up) > _data.slideAngle)
            {
                _isSliding = true;
                Vector3 dir = Vector3.ProjectOnPlane(_inertia, hit.normal) * (1- _data.bounceness) + Vector3.Reflect(_inertia, hit.normal) * _data.bounceness;
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
                    _fallHeight = transform.position.y;
                    _isFalling = true;
                }
                state = MovementState.Fall;
            }
            else
            {
                if (_isFalling)
                {
                    onGrounded?.Invoke(transform.position.y - _fallHeight);
                    _isFalling = false;
                }

                if (_isSliding || (_characterController.velocity.magnitude > _data.sprintSpeed))
                    state = MovementState.Slide;
                else if (_isSprinting && _movement.y > 0)
                    state = MovementState.Sprint;
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
                        _inertia = Vector3.MoveTowards(_inertia, newMov * _speedModifier, _data.speed * _data.acceleration * Time.deltaTime);
                        if (_isJumping)
                            _inertia.y = _data.jumpPower;

                        _characterController.Move(_inertia * Time.deltaTime);
                        if (!_isJumping)
                            _characterController.Move(Vector3.down * _characterController.stepOffset);
                        break;
                    }
                case MovementState.Sprint:
                    {
                        newMov.x *= _data.speed;
                        newMov.z *= _data.sprintSpeed;
                        newMov = _characterController.transform.localToWorldMatrix * newMov;
                        _inertia = Vector3.MoveTowards(_inertia, newMov * _speedModifier, _data.sprintSpeed * _data.acceleration * Time.deltaTime);
                        if (_isJumping)
                            _inertia.y = _data.jumpPower;

                        _characterController.Move(_inertia * Time.deltaTime);
                        if (!_isJumping)
                            _characterController.Move(Vector3.down * _characterController.stepOffset);
                        break;
                    }
                case MovementState.Fall:
                    {
                        newMov *= _data.speed;
                        newMov = _characterController.transform.localToWorldMatrix * newMov;
                        float y = _inertia.y;
                        _inertia.y = 0;

                        _inertia = Vector3.MoveTowards(_inertia, newMov * _speedModifier, _data.speed * _data.acceleration * Time.deltaTime);

                        _inertia.y = y;
                        _inertia += Physics.gravity * Time.deltaTime;

                        _characterController.Move(_inertia * Time.deltaTime);
                        break;
                    }
                case MovementState.Slide:
                    {
                        _inertia -= _inertia * _data.slideStoppingModifier * Time.deltaTime;
                        _inertia += Physics.gravity * Time.deltaTime * (1 - _movementGroundDot);
                        if (_isJumping)
                            _inertia.y = _data.jumpPower;

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

        public void Sprint(bool sprint)
        {
             _isSprinting = sprint;
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
        Sprint,
        Fall,
        Slide
    }
}