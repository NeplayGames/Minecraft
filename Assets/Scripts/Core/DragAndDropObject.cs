using UnityEngine;
using UnityEngine.EventSystems;


public class DragAndDropObject : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private ObjectType objectType;
    private RectTransform rectTransform;
    private Vector3 rectPoint;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rectPoint = rectTransform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
            rectTransform.position = Input.mousePosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.position = rectPoint;
        FindObjectOfType<GameController>().AddBlockToGame(Input.mousePosition,objectType);
    }
}
