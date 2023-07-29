using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{ 
    Vector3 currentSpeed;
    Vector3 startPosition, position;
    Quaternion startRotation, rotation;
    float scrollWheelDeltaChange;

    void Awake()
    {
        startPosition = position = transform.position;
        startRotation = rotation = transform.rotation;
    }


    void Update()
    {
        scrollWheelDeltaChange = Input.mouseScrollDelta.y * 20;
    }

    void LateUpdate()
    {

        Vector2 deltaMouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (Input.GetMouseButtonDown(1))
        {
            rotation = transform.rotation;
            deltaMouse = Vector2.zero;
        }

        Vector3 speed = Vector3.zero;

        if (Input.GetMouseButton(1))
        {
            // rot *= Quaternion.Euler(new Vector3(-deltaMouse.y * data.mouseSensitity * deltaTime * 100, deltaMouse.x * data.mouseSensitity * deltaTime * 100, 0));

            Quaternion oldRot = transform.rotation;
            transform.Rotate(0, deltaMouse.x  * 1.66f, 0, Space.World);
            transform.Rotate(-deltaMouse.y *  1.66f, 0, 0, Space.Self);
            rotation = transform.rotation;
            transform.rotation = oldRot;

            if (Input.GetKey(KeyCode.W)) speed.z = 1;
            else if (Input.GetKey(KeyCode.S)) speed.z = -1;

            if (Input.GetKey(KeyCode.D)) speed.x = 1;
            else if (Input.GetKey(KeyCode.A)) speed.x = -1;

            if (Input.GetKey(KeyCode.E)) speed.y = 1;
            else if (Input.GetKey(KeyCode.Q)) speed.y = -1;

            speed *= GetSpeedMulti();
        }

        if (Input.GetMouseButton(2))
        {
            speed.x = -deltaMouse.x;
            speed.y = -deltaMouse.y;

            speed *= GetSpeedMulti();
            currentSpeed = speed;
        }

        position += transform.TransformDirection(currentSpeed * Time.deltaTime) + (transform.forward * scrollWheelDeltaChange * Time.deltaTime);

        transform.rotation = rotation;
        transform.position = position;
    }

    float GetSpeedMulti()
    {
        if (Input.GetKey(KeyCode.LeftShift)) return 25;
        if (Input.GetKey(KeyCode.LeftControl)) return 1;
        else return 10;
    }
}
