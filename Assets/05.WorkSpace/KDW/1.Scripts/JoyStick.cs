using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour
{
    [SerializeField] GameObject joyStick;
    [SerializeField] GameObject stick;
    [SerializeField] Vector3 joyVector;

    private Vector3 joyFirstPos;
    private Vector3 stickFirstPos;
    private float joyRadius;

    public Vector3 JoyVector => joyVector;

    void Start()
    {
        joyRadius = joyStick.GetComponent<RectTransform>().sizeDelta.y / 2;
        joyFirstPos = joyStick.transform.position;
    }

    //터치 했을 때
    public void PointDown()
    {
        //터치한 곳에 조이스틱이 오게 함
        joyStick.transform.position = Input.mousePosition;
        stick.transform.position = Input.mousePosition;
        stickFirstPos = Input.mousePosition;
    }
    //터치한 상태에서 드래그 할 때
    public void Drag(BaseEventData baseEventData)
    {
        //받은 UI이벤트 데이터를 드래그 이벤트로 변환
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector3 dragPos = pointerEventData.position;

        joyVector = (dragPos - stickFirstPos).normalized; //스틱이 향하는 방향

        float stickDis = Vector3.Distance(dragPos, stickFirstPos); //스틱이 움직이는 거리

        if(stickDis < joyRadius)
        {
            stick.transform.position = stickFirstPos + joyVector * stickDis;
        }
        else
        {
            stick.transform.position = stickFirstPos + joyVector * joyRadius;
        }
    }
    //화면에 손을 뗏을 때
    public void PointUp()
    {
        joyVector = Vector3.zero;
        joyStick.transform.position = joyFirstPos;
        stick.transform.position = joyFirstPos;
    }
}
