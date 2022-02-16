using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickZ : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] RectTransform Handle;
    [SerializeField] RectTransform Background;
    [SerializeField]float forgivessRadius;
    Vector3 initialPos;

    public Vector2 Input
    {
        get;
        private set;
    }

    private void Awake()
    {
        initialPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 DragPos = eventData.position;
        Vector2 BGPosition = Background.position;

        Debug.DrawLine(DragPos, BGPosition);




        
        Input = Vector2.ClampMagnitude(DragPos - BGPosition, Background.rect.width / 2);
        Handle.localPosition = Input;
        Input = Input / (Background.rect.width/2);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
           
        if(Background.rect.width + forgivessRadius >= forgivessRadius)
        {
            transform.position = eventData.position;
        }

        if (Background.rect.height + forgivessRadius >= forgivessRadius)
        {
            transform.position = eventData.position;
        }


    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Up");
        transform.position = initialPos;
        Handle.position = Background.position;
        Input = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }




}
