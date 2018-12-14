using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    //tag는 1대1 비교만 가능
    //한번에 여러개 다중 필터링 가능 : prop인 친구만 거르기위해
    public LayerMask whatIsProp; //유니티상에 드롭다운 메뉴가 나온다.



    public ParticleSystem explosionParticle;
    public AudioSource explosionAudio;

    public float maxDamage = 100f;
    //자기 반경으로 1000포스의힘을 뿌려서 주변의 물건들이 튕겨나가도록
    public float explosionForce = 1000f;

    public float lifeTime = 10f;
    public float explosionRadius = 20f; //폭발 반경

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other) //충돌하자마자
    {
        //가상의 구를 그려서 prop이라는 콜라이더를 가진애만 가져오기
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, whatIsProp);

        //너 혹시 콜라이더있니? 있으면 맞아라!!
        for(int i=0; i< colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            //어떤 지점의 폭발의 위치와 폭발
            targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            Prop targetProp = colliders[i].GetComponent<Prop>();

            float damage = CalculateDamage(colliders[i].transform.position);
            targetProp.TakeDamage(damage);

        }


        //Ball의 자식으로 들어있는 파티클과 오디오는 잠시동안 자식이 아닌 상태를 만들어준다.
        explosionParticle.transform.parent = null; //자식의 부모를 없앤다
        explosionParticle.Play(); //파티클 직접 재생
        explosionAudio.Play();      //오디오도 직접 재생   

        Destroy(explosionParticle.gameObject, explosionParticle.duration);
        Destroy(gameObject);        //자기자신을 파괴
    }

    //1210 프롭제작, 데미지시스템 14분
    //데미지를 차등으로 주자. 거리를 계산해서 가까울수록 큰타격
    private float CalculateDamage(Vector3 targetPosition)
    {
        //상대 위치에서 나의위치를 빼면 방향과 속도를 가진 Vector3 값이 나옴
        Vector3 explosionToTarget = targetPosition - transform.position;
        //Vector3.magnititude 함수를 쓰면 피타고라스 법칙을 사용해 하나의 숫자로 뽑아냄
        float distance = explosionToTarget.sqrMagnitude;

        // radius - x / radius
        //distance가 0이면 : 상대방이 원점에 있으면
        //distance가 1이면 : 상대방이 원의 반지름끝에 있으면

        //원의 바깥에서 안쪽까지의 거리
        float edgeToCenterDistance = explosionRadius - distance;
        float percentage = edgeToCenterDistance / explosionRadius;
        float damage = maxDamage * percentage;
        
        //**적의 콜라이더가 반경 원에 살짝 걸친경우, 적의 위치는 원밖에 있으나 콜라이더만 원에 걸친 경우
        //적의 데미지가 오히려 늘어날 수도 있다 이를 방지하기위해 0


        //두 수 중에 큰 수를 반환하는 함수
        //damage가 음수값이 나오면 0을 반환
        damage = Mathf.Max(0, damage);
        return damage;
    }
}
