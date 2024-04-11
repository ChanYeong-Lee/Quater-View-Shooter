using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rigid;
    private float speed;

    public float damage;
    public float lifeTime;
    public ParticleSystem particlePrefab;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rigid.velocity = speed * transform.forward;
    }

    public void Shot(float speed)
    {
        this.speed = speed;

        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {

        PlayerDamage enemy = collision.collider.GetComponent<PlayerDamage>();
        Vector3 hitPoint = collision.contacts[0].point;
        Vector3 hitDirection = -collision.contacts[0].normal;

        ParticleSystem particle = Instantiate(particlePrefab, hitPoint, Quaternion.identity);
        Destroy(particle.gameObject, 0.25f);

        if (enemy != null)
        {
            enemy.TakeDamage(hitPoint, hitDirection, damage);
        }

        Destroy(gameObject);
    }
}