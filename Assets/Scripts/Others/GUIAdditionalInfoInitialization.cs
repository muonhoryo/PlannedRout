

using PlannedRout.LevelManagment;
using UnityEngine;
using UnityEngine.UI;

namespace PlannedRout
{
    public sealed class GUIAdditionalInfoInitialization : MonoBehaviour 
    {
        [SerializeField] private Text Owner;

        private void Awake()
        {
            LevelManager.LevelInitializedEvent += ReferredInitialization;
        }
        private void ReferredInitialization()
        {
            LevelManager.LevelInitializedEvent-= ReferredInitialization;
            Owner.text = LevelManager.Instance_.LevelData_.GUIAdditionalInformationText;
        }
    }
}