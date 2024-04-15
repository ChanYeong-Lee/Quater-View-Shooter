using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviourPun
{
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
        Bullet bulletInstance = PhotonNetwork.Instantiate("Prefabs/Bullet", shotPoint.position, Quaternion.LookRotation(direction), 0, new object[] { bulletSpeed }).GetComponent<Bullet>();
        bulletInstance.transform.position = shotPoint.position;
        bulletInstance.transform.forward = direction;

        bulletInstance.Shot(this, bulletSpeed);

        attackTime = Time.time;
        inputAttack = false;
    }
}
