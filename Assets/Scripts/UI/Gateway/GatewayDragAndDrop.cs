using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace BlackSite.UI.Gateway
{
    public class GatewayDragAndDrop : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;

        private VisualElement _root;
        private List<VisualElement> _draggables = new List<VisualElement>();
        private List<VisualElement> _slots = new List<VisualElement>();

        private VisualElement _activeDragElement;
        private Vector2 _dragStartPos;
        private VisualElement _originalParent;

        private void OnEnable()
        {
            if (_document == null) return;
            _root = _document.rootVisualElement;

            // Find all draggables
            _root.Query<VisualElement>(className: "draggable").ForEach(el =>
            {
                _draggables.Add(el);
                el.RegisterCallback<PointerDownEvent>(evt => OnPointerDown(evt, el));
                el.RegisterCallback<PointerMoveEvent>(evt => OnPointerMove(evt, el));
                el.RegisterCallback<PointerUpEvent>(evt => OnPointerUp(evt, el));
            });

            // Find all slots
            _root.Query<VisualElement>(className: "drop-target").ForEach(slot =>
            {
                _slots.Add(slot);
            });
        }

        private void OnPointerDown(PointerDownEvent evt, VisualElement element)
        {
            _activeDragElement = element;
            _originalParent = element.parent;
            _dragStartPos = evt.position;

            // Bring to front
            element.BringToFront();
            
            element.CapturePointer(evt.pointerId);
            evt.StopPropagation();
        }

        private void OnPointerMove(PointerMoveEvent evt, VisualElement element)
        {
            if (_activeDragElement != element || !element.HasPointerCapture(evt.pointerId)) return;

            Vector2 delta = (Vector2)evt.position - _dragStartPos;
            element.style.left = element.layout.x + delta.x;
            element.style.top = element.layout.y + delta.y;
            _dragStartPos = evt.position;

            // Highlight hover targets
            foreach (var slot in _slots)
            {
                if (slot.ClassListContains("locked")) continue;

                if (slot.worldBound.Overlaps(element.worldBound))
                {
                    slot.AddToClassList("drop-target-active");
                }
                else
                {
                    slot.RemoveFromClassList("drop-target-active");
                }
            }

            evt.StopPropagation();
        }

        private void OnPointerUp(PointerUpEvent evt, VisualElement element)
        {
            if (_activeDragElement != element || !element.HasPointerCapture(evt.pointerId)) return;

            element.ReleasePointer(evt.pointerId);
            _activeDragElement = null;

            bool dropped = false;

            // Check if dropped on a slot
            foreach (var slot in _slots)
            {
                slot.RemoveFromClassList("drop-target-active");

                if (!slot.ClassListContains("locked") && slot.worldBound.Overlaps(element.worldBound))
                {
                    // Snap to slot
                    element.style.left = 0;
                    element.style.top = 0;
                    element.style.position = Position.Relative;
                    
                    slot.Add(element);
                    dropped = true;
                    Debug.Log($"Dropped {element.name} into {slot.name}");
                    break;
                }
            }

            // Return to original palette if not dropped
            if (!dropped)
            {
                element.style.left = 0;
                element.style.top = 0;
                element.style.position = Position.Relative;
                _originalParent.Add(element);
            }

            evt.StopPropagation();
        }
    }
}
