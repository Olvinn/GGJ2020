using UnityEngine;

namespace Data.Game
{
    /// <summary>
    /// Настройки персонажа игры
    /// v1.1
    /// </summary>
    [CreateAssetMenu(menuName = "Data/Player data")]
    public class PlayerData : ScriptableObject
    {
        //movement
        public float speed;
        public float sprintSpeed;
        public float jumpPower;
        public float acceleration;
        [Range(15, 89)] 
        public float slideAngle = 45f;
        public float slideStoppingModifier = .8f;
        [Range(0, 1)]
        public float bounceness = .25f;

        //interaction
        public float useDistance;
    }
}
