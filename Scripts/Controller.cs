using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private float speed;
    //����������޶�һ���߽磬��һ�������н�����Χ����
    [SerializeField] float minX, MaxX;
    [SerializeField] float minY, MaxY;
    [SerializeField] float minZ, MaxZ;

    private void Update()
    {
        CameraMove();
        RotateView();
    }

    private void CameraMove()
    {
        var moveDir = moveAction.action.ReadValue<Vector2>();
        //��ҡ���ϻ�ȡ����Vector2����ת������ά�ռ��е�X����Z��
        Vector3 joystickInput = new Vector3(moveDir.x, 0, moveDir.y);
        //ע������Space.Self
        transform.Translate(joystickInput * speed * Time.deltaTime, Space.Self);

        Vector3 currentPosition = transform.position;
        //��λ���޶���ָ����Χ��
        float clampedX = Mathf.Clamp(currentPosition.x, minX, MaxX);
        float clampedY = Mathf.Clamp(currentPosition.y, minY, MaxY);
        float clampedZ = Mathf.Clamp(currentPosition.z, minZ, MaxZ);

        Vector3 clampedPosition = new Vector3(clampedX, clampedY, clampedZ);
        transform.position = clampedPosition;
    }

    [SerializeField] private float sensitivity;
    private Vector2 mouseLookDir;
    //�Ľ�һ��,�����ж�һ�£����������������UIԪ��ʱ��ֱ�ӷ���
    private int touchId;
    private bool IsControlRocker;
    [SerializeField] private float smoothness;

    Quaternion horizontalRotation;
    Quaternion verticalRotation;
    Quaternion targetRotation;
    Vector2 mouseLookDirJudge;

    private void RotateView()
    {
        var input = Vector2.zero;

        if(Input.touchCount>0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase==UnityEngine.TouchPhase.Began)
            {
                touchId = touch.fingerId;
                if(EventSystem.current.IsPointerOverGameObject(touchId))
                {
                    IsControlRocker = true;
                    //Debug.Log("����ҡ��");
                }
            }
            else if(touch.phase==UnityEngine.TouchPhase.Ended)
            {
                if(!EventSystem.current.IsPointerOverGameObject(touchId))
                {
                    IsControlRocker = false;
                    //Debug.Log("�ɿ�ҡ��");
                }
            }
            if(Input.touchCount==1&&!IsControlRocker)
            {
                input = new Vector2(Input.GetTouch(0).deltaPosition.x*0.2f, Input.GetTouch(0).deltaPosition.y*0.2f);
            }               
            else if(Input.touchCount>1 && IsControlRocker)
                input = new Vector2(Input.GetTouch(1).deltaPosition.x*0.2f,
                    Input.GetTouch(1).deltaPosition.y*0.2f);          
        }
       
        mouseLookDir += input * sensitivity;
        //���ڻ��������ˣ��������һ�£�����һ����ת�Ƕȣ�
        mouseLookDir.y = Mathf.Clamp(mouseLookDir.y, -20f, 60f);
        //mouseLookDir.x = mouseLookDir.x % 360;
        //ͨ����Ԫ����ת�ӽǵĺ���������������Ԫ����˼��ɵ�Ŀ����ת����
        //Debug.Log("mouseLookDir:" + mouseLookDir);

        if (mouseLookDirJudge != mouseLookDir)
        {
            //verticalRotation = Quaternion.AngleAxis(-mouseLookDir.y, transform.right);
            //horizontalRotation = Quaternion.AngleAxis(mouseLookDir.x, Vector3.up);

            //horizontalRotation.z = 0f;
            //verticalRotation.z = 0f;

            //var Rotation = horizontalRotation* verticalRotation;
            //targetRotation = new Quaternion(Rotation.x, Rotation.y, 0, Rotation.w);
            //mouseLookDirJudge = mouseLookDir;

            float horizontalAngle = mouseLookDir.x;
            float verticalAngle = -mouseLookDir.y;

            horizontalRotation = Quaternion.Euler(0f, horizontalAngle, 0f);
            verticalRotation = Quaternion.Euler(verticalAngle, 0f, 0f);

            targetRotation = horizontalRotation * verticalRotation;
            mouseLookDirJudge = mouseLookDir;
        }


        //Debug.Log("targetRotation:" + targetRotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothness * Time.deltaTime);

        //Debug.Log("verticalRotation:" + verticalRotation);
        //Debug.Log("horizontalRotation:" + horizontalRotation);
        //Debug.Log("targetRotation:" + targetRotation);
        //ʹ��תƽ������һ����ֵ��
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothness * Time.deltaTime);
        //transform.rotation = targetRotation;
        //Debug.Log("input:" + input);
        //mouseLookDir = Vector2.zero;
    }
}
