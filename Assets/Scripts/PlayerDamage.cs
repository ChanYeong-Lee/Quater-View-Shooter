using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerDamage : MonoBehaviourPun
{
    public Action onDie;
    public Action<float> onHPChanged;

    public bool alive;

    public float maxHP;
    public float currentHP;
    public float HPAmount
    {
        get
        {
            return Mathf.Clamp(currentHP / maxHP, 0.0f, 1.0f);
        }
    }

    private NavMeshAgent agent;
    private Rigidbody rigid;
    private Coroutine damagedCoroutine;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        currentHP = maxHP;
        alive = true;
    }

    [PunRPC]
    public void TakeDamageRPC(int attackerViewID, Vector3 position, Vector3 direction, float damage)
    {
        currentHP -= damage;

        animator.SetTrigger("Damaged");

        float damageValue = Mathf.Clamp((damage - 5.0f) / 5.0f, 0.0f, 1.0f);
        animator.SetFloat("DamageValue", damageValue);
       
        float forwardValue = Vector3.Dot(transform.forward, direction);
        float rightValue = Vector3.Dot(transform.right, direction);

        if (forwardValue > 0.71f)
        {
            animator.SetInteger("DamageDirection", 0);
        }
        else if (rightValue < -0.71f)
        {
            animator.SetInteger("DamageDirection", 1);
        }
        else if (forwardValue < -0.71f)
        {
            animator.SetInteger("DamageDirection", 2);
        }
        else 
        {
            animator.SetInteger("DamageDirection", 3);
        }

        if (photonView.IsMine)
        {
            agent.enabled = false;
            agent.updatePosition = false;
            agent.updateRotation = false;

            rigid.isKinematic = false;

            print($"dmg = {damage},forward = {forwardValue}, right = {rightValue}");
            rigid.AddForce(damage * direction, ForceMode.Impulse);
            
            float damageTime = Mathf.Lerp(0.25f, 1.0f, damageValue);

            if (damagedCoroutine != null)
            {
                StopCoroutine(damagedCoroutine);
            }

            damagedCoroutine = StartCoroutine(DamagedCoroutine(damageTime));
        }

        if (currentHP <= 0)
        {
            PlayerModel attacker = GameManager.Instance.wholeModels.Find((model) => model.photonView.ViewID == attackerViewID);

            if (attacker != null && attacker.photonView.IsMine)
            {
                GameManager.Instance.GetKill();
            }
            Die();
        }
    }

    public void Die()
    {
        onDie?.Invoke();
    }

    private IEnumerator DamagedCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        rigid.isKinematic = true;
        agent.enabled = true;
        agent.updatePosition = true;
        agent.updateRotation = true;

        damagedCoroutine = null;
    }
}
