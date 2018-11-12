using UnityEngine;
using System.Collections;

public static class Rigidbody2DExtensions {

    public static void AddCappedForce(this Rigidbody2D rigidbody2D, Vector2 force, float maxSpeed, ForceMode2D forceMode = ForceMode2D.Force) {
		rigidbody2D.AddForce(force, forceMode);
        
        if (rigidbody2D.velocity.magnitude > maxSpeed) {
            float brakeSpeed = rigidbody2D.velocity.magnitude - maxSpeed;
            rigidbody2D.AddForce(-rigidbody2D.velocity.normalized * brakeSpeed);
        }
	}

}