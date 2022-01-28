using UnityEngine;

namespace MessengerTrigger
{
    /// <summary>
    /// Включает какое-то событие, если пользователь находится внутри коллайдера и нажал на кнопку
    /// Не вызывает событие дважды. Нужно выйти из области и опять зайти в неё
    /// </summary>
    public class TriggerOnButton : ScriptTrigger
    {
        [SerializeField] private KeyCode keyToTrigger;
        [SerializeField] private GameObject helpMessage;

        private bool isPlayerInsideTrigger;

        private bool IsPlayerInsideTrigger
        {
            get => isPlayerInsideTrigger;
            set
            {
                isPlayerInsideTrigger = value;
                SetHelpVisibility(isPlayerInsideTrigger);
            }
        }


        protected override void OnTriggerEnter(Collider other)
        {
            if (WasTriggered && triggerOneTime) return;
            IsPlayerInsideTrigger = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (IsPlayerInsideTrigger)
                IsPlayerInsideTrigger = false;
        }


        private void Update()
        {
            if (!IsPlayerInsideTrigger) return;
            if (Input.GetKeyDown(keyToTrigger))
            {
                TriggerScript();
                IsPlayerInsideTrigger = false;
            }
        }

        private void SetHelpVisibility(bool isVisible)
        {
            // TODO показать подсказку для кнопки
            if (isVisible)
                print($"Нажми {keyToTrigger}");

            if (helpMessage)
                helpMessage.SetActive(isVisible);
        }
    }
}