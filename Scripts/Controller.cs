using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private float speed;
    //最后给摄像机限定一个边界，用一个立方盒将它包围起来
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
        //将摇杆上获取到的Vector2向量转化到三维空间中的X轴与Z轴
        Vector3 joystickInput = new Vector3(moveDir.x, 0, moveDir.y);
        //注意这里Space.Self
        transform.Translate(joystickInput * speed * Time.deltaTime, Space.Self);

        Vector3 currentPosition = transform.position;
        //将位置限定在指定范围内
        float clampedX = Mathf.Clamp(currentPosition.x, minX, MaxX);
        float clampedY = Mathf.Clamp(currentPosition.y, minY, MaxY);
        float clampedZ = Mathf.Clamp(currentPosition.z, minZ, MaxZ);

        Vector3 clampedPosition = new Vector3(clampedX, clampedY, clampedZ);
        transform.position = clampedPosition;
    }

    [SerializeField] private float sensitivity;
    private Vector2 mouseLookDir;
    //改进一下,可以判断一下，当点击到的物体是UI元素时，直接返回
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
                    //Debug.Log("操作摇杆");
                }
            }
            else if(touch.phase==UnityEngine.TouchPhase.Ended)
            {
                if(!EventSystem.current.IsPointerOverGameObject(touchId))
                {
                    IsControlRocker = false;
                    //Debug.Log("松开摇杆");
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
        //现在基本做完了，最后完善一下，限制一下旋转角度：
        mouseLookDir.y = Mathf.Clamp(mouseLookDir.y, -20f, 60f);
        //mouseLookDir.x = mouseLookDir.x % 360;
        //通过四元数旋转视角的横向与纵向，两个四元数相乘即可得目标旋转方向
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
        //使旋转平滑，做一个插值：
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothness * Time.deltaTime);
        //transform.rotation = targetRotation;
        //Debug.Log("input:" + input);
        //mouseLookDir = Vector2.zero;
    }
}
