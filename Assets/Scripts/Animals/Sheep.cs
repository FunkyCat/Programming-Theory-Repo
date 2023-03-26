using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : Animal
{
    [SerializeField] GameObject InteractFx;
    [SerializeField] Transform InteractFxHolder;

    private Coroutine activeCoroutine = null;
    private bool needReact = false;
    private bool dontWait = false;

    public override string GetName()
    {
        return "Sheep";
    }

    public override string GetStatus()
    {
        return "Baa-a-a-a";
    }

    // Start is called before the first frame update
    void Start()
    {
        Tame();
    }

    void StopActiveCoroutine()
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }
    }

    void Tame()
    {
        StopActiveCoroutine();
        activeCoroutine = StartCoroutine(TameCycleCoroutine());
    }

    IEnumerator TameCycleCoroutine()
    {
        if (!dontWait)
        {
            animator.SetTrigger("Idle");
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
        dontWait = false;
        if (_tamingArea != null)
        {
            GoTo(_tamingArea.GetRandomPoint());
        }
        else
        {
            Tame();
        }
    }

    void React()
    {
        StopActiveCoroutine();
        activeCoroutine = StartCoroutine(ReactCoroutine());
    }

    IEnumerator ReactCoroutine()
    {
        animator.SetTrigger("jump");
        yield return new WaitForSeconds(Random.Range(2f, 3f));
        Tame();
    }

    public override void GoTo(Vector3 target)
    {
        animator.ResetTrigger("Idle");
        animator.SetTrigger("Walk");
        base.GoTo(target);
    }

    protected override void OnStopped()
    {
        base.OnStopped();

        if (needReact)
        {
            needReact = false;
            React();
        }
        else
        {
            Tame();
        }
    }

    protected override void OnInteract()
    {
        if (InteractFx != null && InteractFxHolder != null)
        {
            Instantiate(InteractFx, InteractFxHolder, false);
        }
        needReact = true;
        Stop();
    }

    protected override void OnTamingAreaChanged()
    {
        dontWait = true;
        StopActiveCoroutine();
        Stop();
    }
}
