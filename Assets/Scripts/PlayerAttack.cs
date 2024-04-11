using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange;
    public float attackDelay;
    public float attackTime;
    public float attackCoolAmount
    {
        get
        {
            return Mathf.Clamp((Time.time - attackTime) / attackDelay, 0.0f, 1.0f);
        }
    }

    public Bullet bulletPrefab;
    public float bulletSpeed;
    public Transform shotPoint;

    public bool inputAttack;
    public bool canAttack;


    public void TryShot()
    {
        inputAttack = true;
    }

    public void CancelShot()
    {
        inputAttack = false;
    }

    private void OnEnable()
    {
        attackTime = -attackDelay;
    }

    private void Update()
    {
        canAttack = attackTime + attackDelay < Time.time;
    }

    public void Shot(Vector3 direction)
    {
        Bullet bulletInstance = Instantiate(bulletPrefab);
        bulletInstance.transform.position = shotPoint.position;
        bulletInstance.transform.forward = direction;

        bulletInstance.Shot(bulletSpeed);

        attackTime = Time.time;
        inputAttack = false;
    }
}
