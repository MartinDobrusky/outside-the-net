using UnityEngine;
using UnityEngine.EventSystems;

public class Console : MonoBehaviour, IDragHandler
{
    [SerializeField] private PcData[] pcDataObjects;

    private Vector2 _differencePoint;

    // Mouse position Vector2 properties
    private Vector2 _mousePosition;
    private Vector2 _startPosition;

    // Start is called before the first frame update
    private void Start()
    {
        // Find correct PcData object by name
        foreach (var dataObject in pcDataObjects)
            if (gameObject.CompareTag(dataObject.name))
                transform.position = dataObject.cmdPosition;
    }

    private void Update()
    {
        // Update mouse position
        if (Input.GetMouseButton(0)) UpdateMousePosition();
        if (Input.GetMouseButtonDown(0))
        {
            UpdateStartPosition();
            UpdateDifferencePoint();
        }
    }

    // On drag event for cmd position
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = _mousePosition - _differencePoint;
    }

    private void UpdateMousePosition()
    {
        _mousePosition.x = Input.mousePosition.x;
        _mousePosition.y = Input.mousePosition.y;
    }

    private void UpdateStartPosition()
    {
        var position = transform.position;
        _startPosition.x = position.x;
        _startPosition.y = position.y;
    }

    private void UpdateDifferencePoint()
    {
        _differencePoint = _mousePosition - _startPosition;

        foreach (var dataObject in pcDataObjects)
            if (gameObject.CompareTag(dataObject.name))
                dataObject.cmdPosition = _differencePoint;
    }
}