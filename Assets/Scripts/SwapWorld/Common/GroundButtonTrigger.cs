using System.Collections;
using SwapWorld.Base;
using SwapWorld.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace SwapWorld.Common
{
    public class GroundButtonTrigger : WorldSwitchTrigger
    {
        [SerializeField] private float stayTimeTrigger;
        [SerializeField] private Vector3 buttonDirection = Vector3.up;
        [SerializeField] private LayerMask collisionMask;
        [SerializeField] private float buttonRadius;
        [SerializeField] private Vector3 pressedLocalPosition;
        [SerializeField] private UnityEvent onButtonPressed;

        public UnityEvent ButtonPressedEvent => onButtonPressed;

        private Vector3 _startPosition;
        private float _stayTime;
        private bool _isTriggered;
        private Coroutine _buttonAnimationCoroutine;

        private void Start()
        {
            _startPosition = transform.localPosition;
        }

        private void Update()
        {
            bool isPressed = CheckButtonCollision();
            if (isPressed)
            {
                if (!_isTriggered)
                {
                    _isTriggered = true;
                    _stayTime = 0f;
                    ButtonPressed();
                }
                else if (_stayTime >= 0)
                {
                    _stayTime += Time.deltaTime;
                    if (_stayTime >= stayTimeTrigger)
                    {
                        _stayTime = -1f;
                        Debug.Log($"Change world by Button {name} ({nameof(GroundButtonTrigger)})");
                        ButtonPressedEvent?.Invoke();
                        TriggerToSwitch();
                    }
                }
            }
            else if (_isTriggered)
            {
                _isTriggered = false;
                _stayTime = 0f;
                ButtonReleased();
            }
        }

        private void OnDrawGizmosSelected()
        {
            var position = transform.position;
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(position, buttonDirection);
            position += buttonDirection;

            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(position,
                Quaternion.LookRotation(buttonDirection, transform.up) * Quaternion.Euler(90, 0, 0),
                new Vector3(1, 0.01f, 1));
            Gizmos.DrawSphere(Vector3.zero, buttonRadius);
            Gizmos.matrix = oldMatrix;
        }

        private bool CheckButtonCollision()
        {
            var currentPosition = transform.position - buttonDirection;
            var cast = Physics.SphereCast(currentPosition, buttonRadius, buttonDirection, out var hit, 1f,
                collisionMask);
            Debug.DrawRay(currentPosition, buttonDirection, cast ? Color.green : Color.red);
            if (!cast) return false;

            Debug.Log(hit.collider.name);
            return true;
        }

        private void ButtonPressed()
        {
            this.RestartCoroutine(ref _buttonAnimationCoroutine, ButtonAnimation(true));
        }

        private void ButtonReleased()
        {
            this.RestartCoroutine(ref _buttonAnimationCoroutine, ButtonAnimation(false));
        }

        private IEnumerator ButtonAnimation(bool isPressed)
        {
            var currentPos = transform.localPosition;
            var targetPos = isPressed ? pressedLocalPosition + _startPosition : _startPosition;
            yield return CoroutineExtensions.ElapsedCoroutine(stayTimeTrigger,
                f => { transform.localPosition = Vector3.Lerp(currentPos, targetPos, f); });
        }
    }
}