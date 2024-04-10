using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    private NavMeshAgent agent;

    private Coroutine rotateCoroutine;

    public float speed;
    public float acceleration;
    public float angularSpeed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.speed = speed;
        agent.acceleration = acceleration;
        agent.angularSpeed = angularSpeed;
    }

    public void ResetAction()
    {
        agent.ResetPath();

        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
            rotateCoroutine = null;
        }
    }

    public void Move(Vector3 pos)
    {
        ResetAction();
        agent.SetDestination(pos);
    }

  

    public void Rotate(Vector3 direction)
    {
        ResetAction();
        rotateCoroutine = StartCoroutine(RotateCoroutine(direction));
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
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angularSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = Quaternion.LookRotation(direction);
        rotateCoroutine = null;
    }

    
}
