using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class EnumerableHelper
{
    public static T RandomElement<T>(this List<T> l) {
        var r = UnityEngine.Random.Range(0, l.Count);
        return l[r];
    }

    public static void bulletRotationAndVelocity(Transform playerTransform, Transform bulletTransform, float rotationOffset, Rigidbody2D rb, float bulletSpeedMultiplier = 1){
        var worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //bullet shooting
        var direction = (Vector2)(worldMousePos - playerTransform.position);
        direction.Normalize();
        var rotationDegrees = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset;
        var rotationVector =  (Vector2)(Quaternion.Euler(0, 0, rotationDegrees) * Vector2.right);
        bulletTransform.rotation = Quaternion.Euler(0, 0, rotationDegrees);
        bulletTransform.position = playerTransform.position + (Vector3)(rotationVector * 1.0f);
        rb.velocity = rotationVector * 10 * bulletSpeedMultiplier;
    }

    public static void bulletRotationAndVelocityJoystick(Transform playerTransform, Transform bulletTransform, float rotationOffset, Rigidbody2D rb, VariableJoystick joystick, float bulletSpeedMultiplier = 1){
        var direction = new Vector2(joystick.Horizontal, joystick.Vertical);
        direction.Normalize();
        var rotationDegrees = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset;
        var rotationVector =  (Vector2)(Quaternion.Euler(0, 0, rotationDegrees) * Vector2.right);
        bulletTransform.rotation = Quaternion.Euler(0, 0, rotationDegrees);
        bulletTransform.position = playerTransform.position + (Vector3)(rotationVector * 1.0f);
        rb.velocity = rotationVector * 10 * bulletSpeedMultiplier;
    }

}
