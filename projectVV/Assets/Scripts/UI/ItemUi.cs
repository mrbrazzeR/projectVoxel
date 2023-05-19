using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ItemUi:MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler
    {
        private Transform _parent;
        public void OnBeginDrag(PointerEventData eventData)
        {
            _parent = transform.parent;
            transform.position = Input.mousePosition;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
            transform.SetParent(_parent);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }
    }
}