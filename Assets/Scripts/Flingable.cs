using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flingable : MonoBehaviour {

	public bool slottable = false;
	public SpringJoint2D springJoint;
	public FlingableSlot flingSlot;

	public Rigidbody2D rb;
	public Collider2D collider;
	private Collider2D _playerCollider;

	public float FlingCooldown;
	private float _flingCooldown = -1;

	public bool focused = false;
	public SpriteGlow.SpriteGlowEffect glowEffect;

	public bool CanFling
	{
		get
		{
			return _flingCooldown <= 0;
		}
	}

	private void OnEnable() {
		FlingControl.AllFlingables.Add(this);
	}
	private void OnDisable() {
		FlingControl.AllFlingables.Remove(this);
	}
	private void OnDestroy() {
		FlingControl.AllFlingables.Remove(this);
	}

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		collider = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(_flingCooldown > 0) {
			_flingCooldown -= Time.deltaTime;
			if(_playerCollider != null && _flingCooldown <= 0) {
				//Physics2D.IgnoreCollision(collider, _playerCollider, false);
			}
		}
	}

	public void Fling(Rigidbody2D other, Vector2 direction, float flingSpeed, float flingAcceleration) {
		other.transform.position = this.transform.position;

		FlingForceComponent ffcThis = gameObject.AddComponent<FlingForceComponent>();
		ffcThis.SetFlingProperties(direction, flingSpeed, flingAcceleration);

		FlingForceComponent ffcOther = other.gameObject.AddComponent<FlingForceComponent>();
		ffcOther.SetFlingProperties(-direction, flingSpeed, flingAcceleration);

		_playerCollider = other.GetComponent<Collider2D>();

		//Physics2D.IgnoreCollision(collider, _playerCollider, true);
		_flingCooldown = FlingCooldown;
		focused = false;

		if(flingSlot != null && slottable) {
			flingSlot.UnslotFlingable();
			flingSlot = null;
		}

		springJoint.enabled = false;
		
	}

	public void Highlight(bool highlight) {
		glowEffect.enabled = highlight;
	}

}
