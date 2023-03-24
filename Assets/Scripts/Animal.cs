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

    [SerializeField] Sprite _portait;
    public Sprite Portrait { get  => _portait; }

    protected bool isSelected = false;

    protected bool isWalking = false;

    private GameManager gameManager;

    public TamingArea TamingArea { get => _tamingArea; set
        {
            _tamingArea = value;
            OnTamingAreaChanged();
        } }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        gameManager = FindAnyObjectByType<GameManager>();

        gameManager.OnSelectedAnimalChanged.AddListener(animal =>
        {
            if (animal == this)
            {
                if (!isSelected)
                {
                    isSelected = true;
                    OnSelected();
                }
            }
            else
            {
                if (isSelected)
                {
                    isSelected = false;
                    OnDeselected();
                }
            }
        });
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

    protected virtual void OnSelected()
    {
    }

    protected virtual void OnDeselected()
    {
    }

    public abstract string GetName();

    public abstract string GetStatus();

    public void OnMouseClick(int mouseButton)
    {
        if (mouseButton == 1)
        {
            OnInteract();
        }
    }
}
