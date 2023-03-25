using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float xRangeFrom = -17f;
    [SerializeField] float xRangeTo = 17f;
    [SerializeField] float zRangeFrom = -17f;
    [SerializeField] float zRangeTo = 17f;
    [SerializeField] Vector3 initialPosition;
    [SerializeField] Vector3 initialRotation;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float rotateSpeed = 90f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = initialPosition;
        transform.rotation = Quaternion.Euler(initialRotation);
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float rotate = Input.GetAxis("Side") * rotateSpeed * Time.deltaTime;

        transform.Translate(new Vector3(moveHorizontal, 0f, moveVertical));
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, xRangeFrom, xRangeTo),
            transform.position.y,
            Mathf.Clamp(transform.position.z, zRangeFrom, zRangeTo)
        );

        transform.Rotate(new Vector3(0f, rotate, 0f));
    }
}
