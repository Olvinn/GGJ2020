using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SwapWorld.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MessengerTrigger
{
    public class Messenger : MonoBehaviour
    {
        public static Messenger Instance;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Button blockerButton;
        [SerializeField] Transform _panel;
        private List<TextMessage> messages;
        private TextMessage _currentMessage;
        private int _messageIndex;
        private bool _isCurrentCompleted;
        private Coroutine _showTextCoroutine;

        public delegate void MessengerEvent();

        public static event MessengerEvent OnStartMessage;
        public static event MessengerEvent OnEndMessage;


        public static Messenger Show(params TextMessage[] message)
        {
            if (!Instance)
            {
                Instance = Instantiate(Resources.Load<Messenger>("Messenger"));
            }

            Instance._panel.gameObject.SetActive(true);
            Instance._messageIndex = -1;
            Instance.messages = message.ToList();
            Instance.ShowNextMessageOrDestroy();
            OnStartMessage?.Invoke();
            return Instance;
        }

        private void Awake()
        {
            messages = new List<TextMessage>();
        }

        private void Start()
        {
            if (blockerButton)
                blockerButton.onClick.AddListener(OnBlockerClicked);
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

            if (EventSystem.current)
                EventSystem.current.SetSelectedGameObject(null);
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
            Debug.Log($"index = {_messageIndex}");
            if (++_messageIndex >= messages.Count)
            {
                _panel.gameObject.SetActive(false);
                OnEndMessage?.Invoke();
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