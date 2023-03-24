using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TamingArea : MonoBehaviour
{
    [SerializeField] float width;
    [SerializeField] float height;
    
    public Vector3 GetRandomPoint()
    {
        return transform.position + new Vector3(
            Random.Range(-width / 2, width / 2),
            0f,
            Random.Range(-height / 2, height / 2)
        );
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, 0.1f, height));
    }
}
