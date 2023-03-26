using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraTargetController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 20f;

    [SerializeField] float xDistance = 20f;
    [SerializeField] float yDistance = 20f;

    [SerializeField] CameraController cameraController;
    Transform followingTarget;

    public void Follow(Transform target)
    {
        followingTarget = target;
    }

    void Update()
    {
        if (followingTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, followingTarget.position, moveSpeed * Time.deltaTime);
            return;
        }

        float horInput = Input.GetAxis("Horizontal");
        float verInput = Input.GetAxis("Vertical");

        var v = Quaternion.Euler(0f, cameraController.yaw, 0f) * (Vector3.forward * verInput + Vector3.right * horInput) * moveSpeed * Time.deltaTime;
        transform.position += v;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -xDistance, xDistance),
            transform.position.y,
            Mathf.Clamp(transform.position.z, -yDistance, yDistance)
        );
    }
}
