using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MessengerTrigger
{
    public class MessengerSpawnerPrototype : MonoBehaviour
    {
        [SerializeField] private List<string> messages;
        bool _messagesShowed;

        public void ShowMessage()
        {
            if (!_messagesShowed)
            {
                var m = messages.Select(t => new TextMessage(t));
                Messenger.Show(m.ToArray());
                _messagesShowed = true;
            }
            else
            {
                var m = new TextMessage(messages[messages.Count - 1]);
                Messenger.Show(m);
            }
        }
    }
}