using UnityEngine;
using UnityEngine.Events;

namespace MessengerTrigger
{
    [RequireComponent(typeof(Collider))]
    public class TriggerOnPath : MonoBehaviour
    {
        [System.Serializable]
        public class TriggerEvent : UnityEvent
        {
        }

        [Tooltip("Коллайдер, с которым нужно столкнуться игроку, чтобы активировать эвент.")] [SerializeField]
        private Collider triggerCollider;

        [Tooltip("Эвент, который активируется после столкновения с коллайдером")] [SerializeField]
        private TriggerEvent triggerEvent;

        [Tooltip("Показывает границы коллайдера (В виде куба)")] [SerializeField]
        private bool showDebugBounds;

        [Tooltip("Если true - эвент вызовется только один раз.")] [SerializeField]
        private bool triggerOneTime;

        /// <summary>
        /// Эвент, который активируется после столкновения игрока с коллайдером
        /// </summary>
        public TriggerEvent PlayerTriggerEvent => triggerEvent;

        private bool _wasTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (_wasTriggered) return;

            if (triggerOneTime) _wasTriggered = true;
            triggerEvent?.Invoke();
        }

        private void OnDrawGizmos()
        {
            if (!showDebugBounds || !triggerCollider) return;

            var bounds = triggerCollider.bounds;

            Gizmos.color = new Color(0, 240, 0, 200);
            Gizmos.DrawCube(bounds.center, bounds.size);
        }


#if UNITY_EDITOR
        private void Reset()
        {
            triggerCollider = GetComponent<Collider>();
        }

        private void OnValidate()
        {
            if (triggerCollider != null && !triggerCollider.isTrigger)
                triggerCollider.isTrigger = true;
        }
#endif
    }
}