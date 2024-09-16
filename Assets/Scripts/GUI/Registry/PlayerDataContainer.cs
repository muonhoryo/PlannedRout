

using UnityEngine;

namespace PlannedRout.PlayersRegistry
{
    public sealed class PlayerDataContainer : MonoBehaviour
    {
        public static PlayerDataContainer Instance_ { get; private set; }
        public PlayerData CurrentPlayerData_;

        private void Awake()
        {
            if(Instance_!=null)
            {
                Destroy(gameObject);
                return;
            }
            Instance_ = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}