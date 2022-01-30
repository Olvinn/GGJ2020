using System.Collections.Generic;
using System.Linq;
using SwapWorld.Base;
using SwapWorld.Types;
using UnityEngine;
using UnityEngine.Events;

namespace MessengerTrigger
{
    public class NpcTask : MonoBehaviour
    {
        [SerializeField] private bool isCompleted;
        [SerializeField] private Sprite npcSprite;
        [SerializeField] private List<string> taskMessages;
        [SerializeField] private List<string> wrongMessages;
        [SerializeField] private List<string> completeMessages;
        [SerializeField] private ScriptTrigger npcTrigger;
        [SerializeField] private UnityEvent onTaskReload;
        [SerializeField] private UnityEvent onTaskCompleted;
        [SerializeField] private WorldState rightWorldStateForTask;

        private bool canSpeek = true;
        private bool isWrongTask;
        private bool taskAdded;

        private void OnEnable()
        {
            npcTrigger.PlayerTriggerEvent.AddListener(OnNpcFirstTimeTriggered);
        }


        private void OnDisable()
        {
            npcTrigger.PlayerTriggerEvent.RemoveListener(OnNpcFirstTimeTriggered);
        }

        private void OnNpcFirstTimeTriggered()
        {
            if (!canSpeek) return;
            var character = new CharacterMessage(npcSprite);
            if (!isCompleted)
            {
                if (taskAdded) Messenger.Show(new TextMessage(taskMessages.Last(), characterMessage: character));
                else
                {
                    taskAdded = true;
                    var m = taskMessages.Select(c => new TextMessage(c, characterMessage: character));
                    Messenger.Show(m.ToArray());
                }
            }
            else
            {
                if (isWrongTask)
                {
                    var m = wrongMessages.Select(c => new TextMessage(c, characterMessage: character));
                    Messenger.Show(m.ToArray());
                    onTaskReload?.Invoke();
                    isWrongTask = isCompleted = false;
                }
                else
                {
                    //Messenger.OnEndMessage += OnComplete;
                    var m = completeMessages.Select(c => new TextMessage(c, characterMessage: character));
                    Messenger.Show(m.ToArray());
                    OnComplete();
                }
            }
        }

        private void OnComplete()
        {
            canSpeek = false;
            Messenger.OnEndMessage -= OnComplete;
            onTaskCompleted?.Invoke();
        }

        public void SetTaskCompleted(bool completed)
        {
            var currentState = WorldStateEventManger.CurrentState;
            Debug.Log($"{currentState} {rightWorldStateForTask}");

            isWrongTask = currentState != rightWorldStateForTask;
            isCompleted = completed;
        }
    }
}