using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MessengerTrigger
{
    public class MessengerSpawnerPrototype : MonoBehaviour
    {
        [SerializeField] private Sprite characterSprite;
        [SerializeField] private bool isMainCharacter;
        [SerializeField] private List<string> messages;
        bool _messagesShowed;
        private CharacterMessage charMessage;

        private void Start()
        {
            charMessage = new CharacterMessage(characterSprite, isMainCharacter);
        }


        public void ShowMessage()
        {
            if (!_messagesShowed)
            {
                var m = messages.Select(t => new TextMessage(t, characterMessage: charMessage));
                Messenger.Show(m.ToArray());
                _messagesShowed = true;
            }
            else
            {
                var m = new TextMessage(messages[messages.Count - 1], characterMessage: charMessage);
                Messenger.Show(m);
            }
        }

        public void ShowMessageAtIndex(int index)
        {
            var m = messages.ElementAtOrDefault(index);
            if (string.IsNullOrEmpty(m)) return;
            Messenger.Show(new TextMessage(m, characterMessage: charMessage));
        }
    }
}