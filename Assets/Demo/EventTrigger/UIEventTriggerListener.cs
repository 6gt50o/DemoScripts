using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
/// <summary>
/// 嵌套集中EventTrigger
/// </summary>
public class UIEventTriggerListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IScrollHandler, IUpdateSelectedHandler, ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler, ICancelHandler, IEventSystemHandler
{
        [FormerlySerializedAs("delegates"), SerializeField]
        private List<Entry> m_Delegates;

        protected UIEventTriggerListener()
        {
        }

        private void Execute(EventTriggerType id, BaseEventData eventData)
        {
            int num = 0;
            int count = this.triggers.Count;
            while (num < count)
            {
                Entry entry = this.triggers[num];
                if ((entry.eventID == id) && (entry.callback != null))
                {
                    entry.callback.Invoke(eventData);
                }
                num++;
            }
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.BeginDrag, eventData);
        }

        public virtual void OnCancel(BaseEventData eventData)
        {
            this.Execute(EventTriggerType.Cancel, eventData);
        }

        public virtual void OnDeselect(BaseEventData eventData)
        {
            this.Execute(EventTriggerType.Deselect, eventData);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.Drag, eventData);
        }

        public virtual void OnDrop(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.Drop, eventData);
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.EndDrag, eventData);
        }

        public virtual void OnInitializePotentialDrag(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.InitializePotentialDrag, eventData);
        }

        public virtual void OnMove(AxisEventData eventData)
        {
            this.Execute(EventTriggerType.Move, eventData);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.PointerClick, eventData);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.PointerDown, eventData);
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.PointerEnter, eventData);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.PointerExit, eventData);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.PointerUp, eventData);
        }

        public virtual void OnScroll(PointerEventData eventData)
        {
            this.Execute(EventTriggerType.Scroll, eventData);
        }

        public virtual void OnSelect(BaseEventData eventData)
        {
            this.Execute(EventTriggerType.Select, eventData);
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            this.Execute(EventTriggerType.Submit, eventData);
        }

        public virtual void OnUpdateSelected(BaseEventData eventData)
        {
            this.Execute(EventTriggerType.UpdateSelected, eventData);
        }

        public List<Entry> triggers
        {
            get
            {
                if (this.m_Delegates == null)
                {
                    this.m_Delegates = new List<Entry>();
                }
                return this.m_Delegates;
            }
            set
            {
                this.m_Delegates = value;
            }
        }

        [Serializable]
        public class Entry
        {
            public UIEventTriggerListener.TriggerEvent callback = new UIEventTriggerListener.TriggerEvent();
            public EventTriggerType eventID = EventTriggerType.PointerClick;
        }

        [Serializable]
        public class TriggerEvent : UnityEvent<BaseEventData>
        {
        }
    }