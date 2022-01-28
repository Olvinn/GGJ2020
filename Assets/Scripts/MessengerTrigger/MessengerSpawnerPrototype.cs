using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MessengerTrigger
{
    public class MessengerSpawnerPrototype : MonoBehaviour
    {
        [SerializeField] private List<string> messages;

        public void ShowMessage()
        {
            var m = messages.Select(t => new TextMessage(t));
            Messenger.Show(m.ToArray());
        }
    }
}