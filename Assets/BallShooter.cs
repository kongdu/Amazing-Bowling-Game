using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//볼 친구를 찍어내서 힘을줘서 날려버리는 역할
//볼이 힘을 채우는 상태를 보여줄 UI슬라이더가 필요
public class BallShooter : MonoBehaviour {
    public Rigidbody ball;
    public Transform firePos;
    public Slider powerSlider;
    public AudioSource shootingAudio;
    public AudioClip fireClip;
    public AudioClip chargingClip;

    public float minForce; //시작힘; 누르자마자 바로 지정되는 힘
    public float maxForce; //최대힘

    public float chargingTime = 0.75f;

    private float currentForce; //현재 힘
    private float chargingSpeed;//누르고있는동안 1초에 힘이 얼만큼충전될지 계산하기위해

    private bool fired; //발사했는지 체크하기위한 플래그

    //컴포넌트가 켜져있으면 자동으로 발동
    private void OnEnable() 
    {
        currentForce = minForce;
        powerSlider.value = minForce;
        fired = false;

    }

    private void Start()
    {
        chargingSpeed = (maxForce - minForce) / chargingTime;  //거리/시간 1초에 얼만큼 충전해야할지 계산할수있다

    }
    private void Update() //총 네가지 케이스
    {
        if(fired == true)
        {
            return;
        }
        powerSlider.value = minForce;
        //CASE1: 힘이 max에 도달해서 자동으로 발사되는 경우
        if(currentForce >= maxForce && !fired)
        {
            currentForce = maxForce;
            Fired();
         
        }else if (Input.GetButtonDown("Fire1"))
        //CASE 2: 버튼 누르는 순간
        {
            //fired = false; 연사가능
            currentForce = minForce;
            shootingAudio.clip = chargingClip;
            shootingAudio.Play();
        }else if (Input.GetButton("Fire1"))
         //CASE 3: 버튼 누르고 있는 동안
        {
            currentForce = currentForce + chargingSpeed * Time.deltaTime;
            powerSlider.value = currentForce;
        }else if(Input.GetButtonUp("Fire1") && !fired)
        {
            Fired();
        }
        
    }
    private void Fired()
    {
        fired = true;
        Rigidbody ballInstance = Instantiate(ball, firePos.position, firePos.rotation);
        ballInstance.velocity = currentForce * firePos.forward; //힘 x 방향
        shootingAudio.clip = fireClip;
        shootingAudio.Play();
        currentForce = minForce;
    }



}
