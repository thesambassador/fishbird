using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVortexShot : MonoBehaviour {

	Rigidbody2D rb;
	PlayerMovement playerMovement;

	public GameObject vortexShotPrefab;
	public float projectileOffset;

	public float vortexShotCooldown = 3;
	public float vortexLifetimeAfterRelease = 3;
	public float vortexMinDistBeforeRelease = 2;
	public float vortexMaxDist = 8;

	public VortexShotBehavior currentVortexShot;

	private float _shotCooldown = -1;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		playerMovement = GetComponent<PlayerMovement>();
	}

	void Update() {
		switch (playerMovement.state) {
			case PlayerMovementState.FLYING:
				UpdateFly();
				break;
			case PlayerMovementState.SWIMMING:
			default:
				UpdateSwim();
				break;
		}

		if (_shotCooldown > 0) {
			_shotCooldown -= Time.deltaTime;
		}
	}

	void FixedUpdate() {
		switch (playerMovement.state) {
			case PlayerMovementState.FLYING:
				FixedUpdateFly();
				break;
			case PlayerMovementState.SWIMMING:
			default:
				FixedUpdateSwim();
				break;
		}
	}

	void UpdateFly() {

	}

	void UpdateSwim() {
		Vector2 aim = playerMovement.birdPlayer.GetAxis2D("Horizontal", "Vertical");
		if (playerMovement.singlePlayer) {
			aim = rb.velocity.normalized;
		}
		if (aim.sqrMagnitude != 0) {
			playerMovement.aimDirection = aim.normalized;
		}

		if(currentVortexShot != null) {
			if (currentVortexShot.released || !currentVortexShot.gameObject.activeInHierarchy) {
				currentVortexShot = null;
			}
		}
		


		if (playerMovement.birdPlayer.GetButtonDown("SecondaryAbility") && _shotCooldown <= 0) {
			currentVortexShot = ObjectPoolManager.GetObject(vortexShotPrefab).GetComponent<VortexShotBehavior>();
			currentVortexShot.transform.position = transform.position + (Vector3)playerMovement.aimDirection * projectileOffset;

			currentVortexShot.Shoot(playerMovement.aimDirection, Vector2.Dot(rb.velocity, playerMovement.aimDirection), vortexMinDistBeforeRelease);
			currentVortexShot.maxDist = vortexMaxDist;

			currentVortexShot.transform.right = playerMovement.aimDirection;
			_shotCooldown = vortexShotCooldown;
		}

		//holding down button
		if (playerMovement.birdPlayer.GetButton("SecondaryAbility")) {

		}

		//release button, stop vortexshot and start cooldown
		if (playerMovement.birdPlayer.GetButtonUp("SecondaryAbility") && currentVortexShot != null) {
			currentVortexShot.ReleaseVortexShot();
			currentVortexShot = null;
		}
	}

	void FixedUpdateFly() {

	}

	void FixedUpdateSwim() {

	}
}
