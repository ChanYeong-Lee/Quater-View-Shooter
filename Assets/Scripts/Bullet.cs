using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    private PlayerAttack owner;
    private Rigidbody rigid;
    private float speed;

    private PlayerDamage target;

    public float damage;
    public float lifeTime;
    public ParticleSystem particlePrefab;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        object[] objects = photonView.InstantiationData;
        this.speed = (float)objects[0];
    }

    private void Update()
    {
        rigid.velocity = speed * transform.forward;
    }

    public void Shot(PlayerAttack owner, float speed)
    {
        this.owner = owner;
        this.speed = speed;

        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine == false)
        {
            Destroy(gameObject);
            return;
        }

        PlayerDamage enemy = collision.collider.GetComponent<PlayerDamage>();
        Vector3 hitPoint = collision.contacts[0].point;
        Vector3 hitDirection = -collision.contacts[0].normal;

        photonView.RPC("StartParticle", RpcTarget.All, hitPoint);

        if (enemy != null)
        {
            if (enemy == owner)
            {
                Destroy(gameObject);
            }
            else
            {
                target = enemy;
                photonView.RPC("Attack", RpcTarget.All, hitPoint, hitDirection, damage);
            }
        }

        Destroy(gameObject);
    }

    [PunRPC]
    public void StartParticle(Vector3 hitPoint)
    {
        ParticleSystem particle = Instantiate(particlePrefab, hitPoint, Quaternion.identity);
        Destroy(particle.gameObject, 0.25f);
    }

    [PunRPC]
    public void Attack(Vector3 hitPoint, Vector3 hitDirection, float damage)
    {
        target.TakeDamage(owner, hitPoint, hitDirection, damage);
    }
}