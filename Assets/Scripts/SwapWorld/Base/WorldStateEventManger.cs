using SwapWorld.Types;
using UnityEngine;

namespace SwapWorld.Base
{
    /// <summary>
    /// Класс, который оповещает подписчиков об изменении состояния мира.
    /// </summary>
    public sealed class WorldStateEventManger : MonoBehaviour
    {
        [SerializeField] private WorldState worldState = WorldState.Black;

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(this);
        }

        private void Start()
        {
            InvokeSwitchWorld(worldState);
        }

        private static WorldStateEventManger _instance;

        public static WorldState CurrentState
        {
            get
            {
                if (_instance == null) CreateInstance();
                return _instance.worldState;
            }

            set
            {
                if (_instance == null) return;
                if (_instance.worldState == value) return;
                _instance.worldState = value;
                InvokeSwitchWorld(value);
            }
        }

        public delegate void WorldStateChanged(WorldState state);

        public static event WorldStateChanged OnWorldHasChanged;

        private static void InvokeSwitchWorld(WorldState worldState)
        {
            Debug.Log($"world state = {worldState}");
            OnWorldHasChanged?.Invoke(worldState);
        }

        private static void CreateInstance()
        {
            var createdObject = new GameObject($"{nameof(WorldStateEventManger)}");
            _instance = createdObject.AddComponent<WorldStateEventManger>();
        }
    }
}