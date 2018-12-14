using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//체력을 가지고 있어서 일정이상의 데미지를 받으면 파괴되야한다.
//자기자신이 파괴됐을때 게임메니저에게 파괴됐음을 알리고 점수를 추가해야한다.
public class Prop : MonoBehaviour
{

    public int score = 5; //파괴될때마다 올릴 점수

    public ParticleSystem explosionParticle; //특수효과를 재생하기위해

    public float hp = 10f;

    //prop이 스스로 발동하는게 아니라 외부에서 prop한테가서 데미지를 받아라
    public void TakeDamage(float damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            ParticleSystem instance = Instantiate(explosionParticle, transform.position, transform.rotation);
            
            
            //새로 찍어낸 친구의 오디오 소스 가져오기
            AudioSource explosionAudio = instance.GetComponent<AudioSource>();
            explosionAudio.Play();


            Destroy(instance.gameObject, instance.duration);
            gameObject.SetActive(false); //prop을 껏다가 다시 켜는 방식으로 재활용
        }
    }
}
