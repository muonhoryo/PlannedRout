using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

namespace PlannedRout
{
    public abstract class MainMenuButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action PointUpEvent = delegate { };
        public event Action PointDownEvent = delegate { };
        public event Action PointEnterEvent = delegate { };
        public event Action PointExitEvent = delegate { };

        [SerializeField] private Graphic TargetGraphic;

        [SerializeField] private Color NormalColor;
        [SerializeField] private Color HighlightedColor;
        [SerializeField] private Color PressedColor;
        [SerializeField] private Color InactiveColor;

        private bool IsPressed = false;
        private bool IsEnteredPoint = false;

        private void Awake()
        {
            TargetGraphic.color = NormalColor;
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            IsPressed = false;

            if (!enabled)
                return;

            if (IsEnteredPoint)
            {
                TargetGraphic.color = HighlightedColor;
                PointUpEvent();
                OnClickAction();
            }
            else
            {
                TargetGraphic.color = NormalColor;
            }
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;

            if (!enabled)
                return;

            TargetGraphic.color = PressedColor;
            PointDownEvent();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            IsEnteredPoint = true;

            if (!enabled)
                return;

            if(!IsPressed)
                TargetGraphic.color = HighlightedColor;
            PointEnterEvent();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            IsEnteredPoint = false;

            if (!enabled)
                return;

            if (!IsPressed)
            {
                TargetGraphic.color = NormalColor;
            }
            PointExitEvent();
        }

        protected abstract void OnClickAction();

        private void OnEnable()
        {
            TargetGraphic.color =IsPressed?PressedColor:(IsEnteredPoint?HighlightedColor:NormalColor);
        }
        private void OnDisable()
        {
            TargetGraphic.color = InactiveColor;
        }
    }
}
