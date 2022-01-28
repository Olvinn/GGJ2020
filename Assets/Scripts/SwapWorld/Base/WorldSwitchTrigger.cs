using SwapWorld.Types;
using UnityEngine;

namespace SwapWorld.Base
{
    /// <summary>
    /// Базовый класс для всех переключателей миров.
    /// </summary>
    public abstract class WorldSwitchTrigger : MonoBehaviour
    {
        protected void TriggerToSwitch()
        {
            var state = WorldStateEventManger.CurrentState == WorldState.Black ? WorldState.White : WorldState.Black;
            WorldStateEventManger.CurrentState = state;
        }
    }
}