using SwapWorld.Types;
using UnityEngine;

namespace SwapWorld.Base
{
    /// <summary>
    /// Базовый класс для всех объектов мира, которые должны изменяться в зависимости от состояния.
    /// </summary>
    public abstract class WorldSwitchObject : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            WorldStateEventManger.OnWorldHasChanged += OnWorldChanged;
        }


        protected void OnDisable()
        {
            WorldStateEventManger.OnWorldHasChanged -= OnWorldChanged;
        }

        protected abstract void OnWorldChanged(WorldState state);

        protected WorldState GetCurrentState() => WorldStateEventManger.CurrentState;
    }
}