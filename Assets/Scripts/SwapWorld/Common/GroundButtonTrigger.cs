using SwapWorld.Base;
using UnityEngine;

namespace SwapWorld.Common
{
    public class GroundButtonTrigger : WorldSwitchTrigger
    {
        [SerializeField] private float stayTimeTrigger;
        [SerializeField] private Vector3 buttonDirection = Vector3.up;
        [SerializeField] private LayerMask collisionMask;
        [SerializeField] private float buttonRadius;
        private float _stayTime;
        private bool _isTriggered;
        private Collision _lastTriggeredCollision;

        private void OnCollisionEnter(Collision other)
        {
            _stayTime = 0f;
            _isTriggered = true;
            _lastTriggeredCollision = other;
        }

        private void Update()
        {
            if (!_isTriggered) return;
            if (!CheckButtonCollision(_lastTriggeredCollision)) return;
            _stayTime += Time.deltaTime;
            if (_stayTime >= stayTimeTrigger)
            {
                Debug.Log($"Change world by Button {name} ({nameof(GroundButtonTrigger)})");
                _isTriggered = false;
                _stayTime = 0f;
                TriggerToSwitch();
            }
        }

        private void OnCollisionExit()
        {
            _isTriggered = false;
            _stayTime = 0f;
            _lastTriggeredCollision = null;
        }


        private void OnDrawGizmosSelected()
        {
            var position = transform.position;
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(position, buttonDirection);
            position += buttonDirection;

            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(position, Quaternion.LookRotation(buttonDirection, transform.up) * Quaternion.Euler(90, 0,0), new Vector3(1, 0.01f, 1));
            Gizmos.DrawSphere(Vector3.zero, buttonRadius);
            Gizmos.matrix = oldMatrix;
        }

        private bool CheckButtonCollision(Collision collision)
        {
            var currentPosition = transform.position - buttonDirection;
            var cast = Physics.SphereCast(currentPosition, buttonRadius, buttonDirection, out var hitInfo, 3f,
                collisionMask);
            Debug.DrawRay(currentPosition, buttonDirection, cast ? Color.green : Color.red);
            if (!cast) return false;

            return collision.transform == hitInfo.transform;
        }
    }
}