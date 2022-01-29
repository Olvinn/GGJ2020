using UnityEngine;

namespace MessengerTrigger
{
    public class CharacterMessage
    {
        public Sprite CharacterSprite { get; }
        public bool IsMainCharacter { get; }

        public CharacterMessage(Sprite characterSprite = null, bool isMain = false)
        {
            CharacterSprite = characterSprite;
            IsMainCharacter = isMain;
        }
    }
}