using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    private Coroutine rotateCoroutine;

    public float speed;
    public float dashSpeed;
    public float dashDistance;
    public float dashDelay;
    public float dashTime;
    public bool canDash;

    private float currentSpeed;
    private bool dash;
    private Queue<Action> actionQueue;

    private Action cancelCallback;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        actionQueue = new Queue<Action>();
    }

    private void OnEnable()
    {
        actionQueue.Clear();

        currentSpeed = speed;
        dashTime = -dashDelay;
    }

    private void Update()
    {
        agent.speed = currentSpeed;

        if (dash == false && agent.isActiveAndEnabled)
        {
            if (actionQueue.Count > 0)
            {
                actionQueue.Dequeue()?.Invoke();
            }
        }

        canDash = dashTime + dashDelay < Time.time;

        float forwardValue = Vector3.Dot(transform.forward, agent.velocity.normalized);
        forwardValue = Mathf.Clamp(forwardValue, 0.0f, 1.0f);

        animator.SetFloat("MoveSpeed", forwardValue * agent.velocity.magnitude / speed);
    }

    public void Dash(Vector3 direction)
    {
        if (agent.isActiveAndEnabled == false)
        {
            actionQueue.Enqueue(() => Dash(direction));
            return;
        }

        ResetAction();
        StartCoroutine(DashCoroutine(direction));
    }

    public void ResetAction()
    {
        agent.ResetPath();

        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
            rotateCoroutine = null;
        }

        if (cancelCallback != null)
        {
            cancelCallback?.Invoke();
        }
    }

    public void Stop()
    {
        ResetAction();
        if (actionQueue.Count > 0)
        {
            actionQueue.Clear();
        }
    }

    public void Move(Vector3 pos)
    {
        if (dash || agent.isActiveAndEnabled == false)
        {
            actionQueue.Enqueue(() => Move(pos));
            return;
        }

        ResetAction();
        agent.SetDestination(pos);
    }

    public void Rotate(Vector3 direction)
    {
        if (dash || agent.isActiveAndEnabled == false)
        {
            actionQueue.Enqueue(() => Rotate(direction));
            return;
        }

        ResetAction();
        rotateCoroutine = StartCoroutine(RotateCoroutine(direction));
    }

    public void Rotate(Vector3 direction, Action callback, Action cancelCallback)
    {
        if (dash || agent.isActiveAndEnabled == false)
        {
            actionQueue.Enqueue(() => Rotate(direction, callback, cancelCallback));
            return;
        }

        ResetAction();
        rotateCoroutine = StartCoroutine(RotateCoroutine(direction, callback, cancelCallback));
    }

    private IEnumerator RotateCoroutine(Vector3 direction)
    {
        direction.y = 0.0f;
        direction.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (true)
        {
            if (Vector3.Dot(transform.forward, direction) > 0.995)
            {
                break;
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, agent.angularSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = Quaternion.LookRotation(direction);
        rotateCoroutine = null;
    }

    private IEnumerator RotateCoroutine(Vector3 direction, Action nextAction, Action cancelCallback)
    {
        direction.y = 0.0f;
        direction.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        
        this.cancelCallback = cancelCallback;

        while (true)
        {
            if (Vector3.Dot(transform.forward, direction) > 0.995)
            {
                break;
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, agent.angularSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = Quaternion.LookRotation(direction);
        nextAction?.Invoke();

        this.cancelCallback = null;
        rotateCoroutine = null;
    }

    private IEnumerator DashCoroutine(Vector3 direction)
    {
        agent.SetDestination(transform.position + dashDistance * direction);
        transform.forward = direction;
        dash = true;
        animator.SetBool("Dash", true);
        currentSpeed = dashSpeed;
        dashTime = Time.time;
        yield return new WaitForSeconds(dashDistance / dashSpeed);
        currentSpeed = speed;
        dash = false;
        animator.SetBool("Dash", false);
    }
}