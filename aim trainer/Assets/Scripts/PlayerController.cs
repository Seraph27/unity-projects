using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Transform playerHead;
    Transform playerBody;
    float headVerticalRotation;
    Rigidbody rb;
    Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {
        playerHead = transform.Find("Main Camera").transform;
        playerBody = transform;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime;
        headVerticalRotation -= mouseY * 100;
        headVerticalRotation = Mathf.Clamp(headVerticalRotation, -90, 90);
        playerHead.localEulerAngles = new Vector3(headVerticalRotation, 0, 0);

        var mouseX = Input.GetAxis("Mouse X") * Time.deltaTime;
        playerBody.localEulerAngles += new Vector3(0, mouseX * 100, 0);

        var movementX = Input.GetAxis("Horizontal");
        var movementY = Input.GetAxis("Vertical");
        movement = new Vector2(movementX, movementY);
    }

    void FixedUpdate(){
        Vector3 combinedMovement = (transform.forward * movement.y + transform.right * movement.x) * Time.fixedDeltaTime * 1000 + transform.up * rb.velocity.y;

        rb.velocity = combinedMovement;
    }
}
