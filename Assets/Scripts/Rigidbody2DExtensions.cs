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

	public static void AddForceToAchieveTargetVelocity(this Rigidbody2D rigidbody2D, Vector2 targetSpeed, float maxForce, AnimationCurve velocityForceCurve) {
		Vector2 curTargetDiff = targetSpeed - rigidbody2D.velocity;

		//avoid divide by 0 i guess
		float forceToUse = 0;
		//print(Vector2.Dot(targetSpeed, curTargetDiff));
		if (targetSpeed.magnitude > 0 && Vector2.Dot(targetSpeed, curTargetDiff) >= 0) {
			// if curTargetDiff.magnitude is 0, that means that we're moving at the target speed already
			forceToUse = velocityForceCurve.Evaluate(curTargetDiff.magnitude / targetSpeed.magnitude) * maxForce;
		}

		rigidbody2D.AddForce(forceToUse * curTargetDiff.normalized);
	}

}