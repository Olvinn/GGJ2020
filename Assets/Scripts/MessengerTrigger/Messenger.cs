using System;
using System.Collections;
using System.Linq;
using SwapWorld.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MessengerTrigger
{
    public class Messenger : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Button blockerButton;
        private TextMessage[] messages;
        private TextMessage _currentMessage;
        private int _messageIndex;
        private bool _isCurrentCompleted;
        private Coroutine _showTextCoroutine;

        public static Messenger Show(params TextMessage[] message)
        {
            var messenger = Instantiate(Resources.Load<Messenger>("Messenger"));
            messenger.messages = message.ToArray();
            return messenger;
        }

        private void Start()
        {
            if (blockerButton)
                blockerButton.onClick.AddListener(OnBlockerClicked);

            _messageIndex = -1;
            ShowNextMessageOrDestroy();
        }

        private IEnumerator Show()
        {
            if (_currentMessage == null) yield break;

            var message = _currentMessage;
            var messageText = message.Text;
            var messageLength = message.Length;
            yield return CoroutineExtensions.ElapsedCoroutine(messageLength, f =>
            {
                var totalSymbols = Mathf.RoundToInt(messageText.Length * f);
                var textForThisTime = messageText.Substring(0, totalSymbols);
                text.text = textForThisTime;
            });

            _isCurrentCompleted = true;
        }

        private void OnBlockerClicked()
        {
            if (_isCurrentCompleted) ShowNextMessageOrDestroy();
            else SkipCurrentMessage();
        }

        private void SkipCurrentMessage()
        {
            if (_currentMessage == null)
            {
                ShowNextMessageOrDestroy();
                return;
            }

            if (_showTextCoroutine != null) StopCoroutine(_showTextCoroutine);
            text.text = _currentMessage.Text;
            _isCurrentCompleted = true;
        }

        private void ShowNextMessageOrDestroy()
        {
            if (++_messageIndex >= messages.Length)
            {
                Destroy(gameObject);
            }
            else
            {
                _currentMessage = messages[_messageIndex];
                text.text = String.Empty;
                _isCurrentCompleted = false;
                if (!_currentMessage.Valid) ShowNextMessageOrDestroy();
                else this.RestartCoroutine(ref _showTextCoroutine, Show());
            }
        }
    }
}