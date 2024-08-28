

using PlannedRout.LevelManagment;
using UnityEngine;
using UnityEngine.UI;

namespace PlannedRout
{
    public sealed class GUISizeInitialization : MonoBehaviour
    {
        [SerializeField] private CanvasScaler Owner;
        [SerializeField] private RectTransform AdditionalInfo;
        [SerializeField] private RectTransform LegendInfo;

        private void Awake()
        {
            LevelManager.LevelInitializedEvent += ReferredInitialization;
        }
        private void ReferredInitialization()
        {
            LevelManager.LevelInitializedEvent -= ReferredInitialization;
            Owner.scaleFactor = LevelManager.Instance_.LevelData_.GUISize;
            AdditionalInfo.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, AdditionalInfo.rect.width / LevelManager.Instance_.LevelData_.GUISize);
            AdditionalInfo.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, AdditionalInfo.rect.height / LevelManager.Instance_.LevelData_.GUISize);
            LegendInfo.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, LegendInfo.rect.width / LevelManager.Instance_.LevelData_.GUISize);
            LegendInfo.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, LegendInfo.rect.height / LevelManager.Instance_.LevelData_.GUISize);
        }
    }
}