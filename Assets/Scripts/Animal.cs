using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public abstract class Animal : MonoBehaviour
{
    protected NavMeshAgent navMeshAgent;
    protected Animator animator;
    [SerializeField] protected TamingArea _tamingArea = null;

    public TamingArea TamingArea { get => _tamingArea; set
        {
            _tamingArea = value;
            OnTamingAreaChanged();
        } }

    protected bool isWalking = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isWalking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking && !navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    Stop();
                }
            }
        }
        OnUpdate();
    }

    public virtual void GoTo(Vector3 target)
    {
        navMeshAgent.SetDestination(target);
        navMeshAgent.isStopped = false;
        isWalking = true;
    }

    public virtual void Stop()
    {
        isWalking = false;
        navMeshAgent.isStopped = true;
        OnStopped();
    }

    protected virtual void OnStopped()
    {
    }

    protected virtual void OnUpdate()
    {

    }

    protected virtual void OnInteract()
    {
    }

    protected virtual void OnTamingAreaChanged()
    {
    }

    public abstract string GetStatus();

    private void OnMouseUpAsButton()
    {
        OnInteract();
    }
}
