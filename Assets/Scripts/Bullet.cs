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

        StartCoroutine(DestroyCoroutine(lifeTime));
    }

    private IEnumerator DestroyCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine == false)
        {
            return;
        }

        PlayerDamage enemy = collision.collider.GetComponent<PlayerDamage>();
        Vector3 hitPoint = collision.contacts[0].point;
        Vector3 hitDirection = -collision.contacts[0].normal;

        photonView.RPC("StartParticle", RpcTarget.All, hitPoint);

        if (enemy != null && enemy != owner)
        {
            int ownerViewID = owner.photonView.ViewID;
            enemy.photonView.RPC("TakeDamageRPC", RpcTarget.All, ownerViewID, hitPoint, hitDirection, damage);
        }

        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    public void StartParticle(Vector3 hitPoint)
    {
        ParticleSystem particle = Instantiate(particlePrefab, hitPoint, Quaternion.identity);
        Destroy(particle.gameObject, 0.25f);
    }
}