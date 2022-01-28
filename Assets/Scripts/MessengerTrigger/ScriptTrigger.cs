using UnityEngine;
using UnityEngine.Events;

namespace MessengerTrigger
{
    /// <summary>
    /// Включает какое-то событие, если игрок задевает коллайдер
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ScriptTrigger : MonoBehaviour
    {
        [System.Serializable]
        public class TriggerEvent : UnityEvent
        {
        }

        [Tooltip("Коллайдер, с которым нужно столкнуться игроку, чтобы активировать эвент.")] [SerializeField]
        protected Collider triggerCollider;

        [Tooltip("Эвент, который активируется после столкновения с коллайдером")] [SerializeField]
        protected TriggerEvent triggerEvent;

        [Tooltip("Показывает границы коллайдера (В виде куба)")] [SerializeField]
        protected bool showDebugBounds;

        [Tooltip("Если true - эвент вызовется только один раз.")] [SerializeField]
        protected bool triggerOneTime;

        protected bool WasTriggered;

        /// <summary>
        /// Эвент, который активируется после столкновения игрока с коллайдером
        /// </summary>
        public TriggerEvent PlayerTriggerEvent => triggerEvent;

        protected virtual void OnTriggerEnter(Collider other)
        {
            // TODO check if collider is player
            TriggerScript();
        }

        protected void TriggerScript()
        {
            if (WasTriggered) return;
            if (triggerOneTime) WasTriggered = true;
            triggerEvent?.Invoke();
        }


        protected virtual void OnDrawGizmos()
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