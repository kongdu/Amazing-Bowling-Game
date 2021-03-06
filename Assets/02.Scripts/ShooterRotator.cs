﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterRotator : MonoBehaviour {
    //한정된 상태에 대해 한번에 한가지 값을 갖게 할 수 있음
    //순서 : Idle > Horizontal > Vertical > Ready
    private enum RotateState
    {
        Idle, Vertical, Horizontal, Ready
    }

    private RotateState state = RotateState.Idle;
    public float verticalRotateSpeed = 360f;
    public float horizontalRotateSpeed = 360f;
    public BallShooter ballShooter; //볼슈터스크립트 가져와서 껐다 켰다 하기위해

    private void Update()
    {
        switch (state)
        {
            case RotateState.Idle:

                if (Input.GetButtonDown("Fire1")) //누르는 그 한순간
                {
                    state = RotateState.Horizontal;
                }

                break;
            case RotateState.Vertical:
                if (Input.GetButton("Fire1")) //마우스를 꾹 누르고있는 동안
                {
                    //x축을 기준으로 회전
                    transform.Rotate(new Vector3(-verticalRotateSpeed * Time.deltaTime, 0, 0));
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    state = RotateState.Ready;
                    ballShooter.enabled = true;
                }

                break;
            case RotateState.Horizontal:

                if (Input.GetButton("Fire1")) //누르고있는 동안
                {
                    transform.Rotate(new Vector3(0, horizontalRotateSpeed * Time.deltaTime, 0)); //수평방향으로 회전
                }
                else if (Input.GetButtonUp("Fire1")) //마우스에서 손을 떼는 순간 '버티컬'로 스위치
                {
                    state = RotateState.Vertical;
                }
                break;
            case RotateState.Ready:

                break;

        }


    }

    private void OnEnable()
    {
        transform.rotation = Quaternion.identity; //0도 0도 0도 회전값 리셋
        state = RotateState.Idle;
        ballShooter.enabled = false;
    }






}
