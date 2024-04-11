using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerDamage : MonoBehaviour
{
    public Action onDie;
    public Action<float> onHPChanged;


    public bool alive;

    public float maxHP;
    public float currentHP;
    public float CurrentHP
    {
        get { return currentHP; }
        set
        {
            currentHP = value;
            if (currentHP <= 0.0f)
            {
                currentHP = 0.0f;
                Die();
            }
            onHPChanged?.Invoke(currentHP);
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

    public void TakeDamage(Vector3 position, Vector3 direction, float damage)
    {
        agent.enabled = false;
        rigid.isKinematic = false;

        rigid.AddForce(damage * direction, ForceMode.Impulse);
        
        if (damagedCoroutine != null)
        {
            StopCoroutine(damagedCoroutine);
        }
        float damageValue = Mathf.Clamp((damage - 5.0f) / 5.0f, 0.0f, 1.0f);
        float damageTime = Mathf.Lerp(0.25f, 0.5f, damageValue);
        damagedCoroutine = StartCoroutine(DamagedCoroutine(damageTime));

        animator.SetBool("Damaged", true);
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
        else if (rightValue > 0.71f)
        {
            animator.SetInteger("DamageDirection", 3);
        }
    }

    public void Die()
    {
        onDie?.Invoke();
    }

    private IEnumerator DamagedCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("Damaged", false);

        rigid.isKinematic = true;
        agent.enabled = true;

        damagedCoroutine = null;
    }
}
