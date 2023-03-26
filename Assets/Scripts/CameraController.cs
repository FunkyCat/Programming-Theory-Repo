using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float pitch = 30f;
    public float yaw = 0f;

    [SerializeField] Transform target;
    [SerializeField] float distance = 10f;
    [SerializeField] float minDistance = 5f;
    [SerializeField] float maxDistance = 30f;
    [SerializeField] float rotateSpeed = 90f;
    [SerializeField] float zoomSpeed = 10f;

    // Update is called once per frame
    void LateUpdate()
    {
        var targetRotation = Quaternion.Euler(pitch, yaw, 0f);
        var targetPosition = target.position + targetRotation * Vector3.back * distance;
        transform.position = targetPosition;
        transform.rotation = targetRotation;

        float rotateInput = Input.GetAxis("CameraRotate");
        yaw += rotateInput * rotateSpeed * Time.deltaTime;

        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        distance = Mathf.Clamp(distance + wheelInput * zoomSpeed * Time.deltaTime, minDistance, maxDistance);
    }
}
