using UnityEngine;

namespace MessengerTrigger
{
    /// <summary>
    /// Класс, который хранит текущее сообщение.
    /// </summary>
    public class TextMessage
    {
        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Звук сообщения, пока функционала воспроизведения звука нет
        /// </summary>
        public AudioClip Sound { get; }

        /// <summary>
        /// Время в секундах, сколько должно висеть сообщение.
        /// Если есть звук - время звука.
        /// Если нет - примерно 10 символов в секунду.
        /// </summary>
        public float Length => Sound == null ? Text.Length / 20f : Sound.length;

        /// <summary>
        /// Валидность сообщения. Оно не должно быть пустым.
        /// При этом звука может не быть.
        /// </summary>
        public bool Valid => !string.IsNullOrWhiteSpace(Text);

        public TextMessage(string text, AudioClip audioClip = null)
        {
            Text = text;
            Sound = audioClip;
        }

        public override bool Equals(object obj)
        {
            if (obj is TextMessage)
            {
                TextMessage t = obj as TextMessage;
                if (t.Text == Text && Sound == t.Sound)
                    return true;
            }
            return base.Equals(obj);
        }
    }
}