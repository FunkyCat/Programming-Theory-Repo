using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Animal
{
    private Coroutine activeCoroutine = null;
    private bool needReact = false;

    public override string GetName()
    {
        return "Chicken";
    }

    public override string GetStatus()
    {
        return "Cluck-cluck";
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
        animator.SetTrigger("idle");
        yield return new WaitForSeconds(Random.Range(1f, 3f));
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
        animator.SetTrigger("stun");
        yield return new WaitForSeconds(Random.Range(2f, 3f));
        Tame();
    }

    public override void GoTo(Vector3 target)
    {
        animator.ResetTrigger("idle");
        animator.SetTrigger("walk");
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
        needReact = true;
        Stop();
    }
}
