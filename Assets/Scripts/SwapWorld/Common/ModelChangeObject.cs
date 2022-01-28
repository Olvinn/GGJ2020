using System.Collections.Generic;
using System.Linq;
using SwapWorld.Base;
using SwapWorld.Types;
using UnityEngine;

namespace SwapWorld.Common
{
    public class ModelChangeObject : WorldSwitchObject
    {
        [System.Serializable]
        private class ModelForState
        {
            [SerializeField] private WorldState worldState;
            [SerializeField] private GameObject prefabForThisState;
            [SerializeField] private Transform parentObject;
            private GameObject _cachedObject;

            public WorldState WorldState => worldState;

            public void ReplaceObjects()
            {
                if (prefabForThisState != null && _cachedObject == null)
                    _cachedObject = Instantiate(prefabForThisState, parentObject);

                foreach (Transform child in parentObject)
                {
                    var gameObject = child.gameObject;

                    gameObject.SetActive(gameObject == _cachedObject);
                }
            }

            private void DisableOtherGameObjects()
            {
            }
        }

        [SerializeField] private List<ModelForState> modelsForStates;

        protected override void OnWorldChanged(WorldState state)
        {
            var models = modelsForStates.Where(m => m.WorldState == state).ToList();
            models.ForEach(m => m.ReplaceObjects());
        }
    }
}