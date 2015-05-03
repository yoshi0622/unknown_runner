using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	public static float ACCELERATION = 10.0f;
	public static float SPEED_MIN = 4.0f;
	public static float SPEED_MAX = 8.0f;
	public static float JUNP_HEIGHT_MAX = 3.0f;
	public static float JUNP_KEY_RELEASE_REDUCE = 0.5f;
	
	public enum STEP{
		NONE = -1,
		RUN  = 0,
		JUNP,
		MISS,
		NUM,
	};
	
	public STEP step      = STEP.NONE;
	public STEP next_step = STEP.NONE;
	
	public float  step_timer = 0.0f;
	private bool is_landed = false;
	private bool is_collided = false;
	private bool is_key_released =false;
	
	
	// Use this for initialization
	void Start () {
		this.next_step = STEP.RUN;
		
	}
	private void check_landed()
	{
		this.is_landed = false;

		do {
			Vector3 s = this.transform.position;
			Vector3 e = s + Vector3.down * 1.0f;
			
			RaycastHit hit;
			if (! Physics.Linecast (s, e, out hit)) {
				break;
			}
			if (this.step == STEP.JUNP) {
				if (this.step_timer < Time.deltaTime * 3.0f) {
				}
			}
			this.is_landed = true;
		} while(false);
	}
	
	
	// Update is called once per frame
	void Update () {
		Vector3 velocity = this.rigidbody.velocity;
		this.check_landed ();
		this.step_timer += Time.deltaTime;
		if (next_step == STEP.NONE) {
			switch (this.step) {
			case STEP.RUN:
				if (! this.is_landed) {
				} else {
					if (Input.GetMouseButtonDown (0)) {
						this.next_step = STEP.JUNP;
					}
				}
				break;
			case STEP.JUNP:
				if (this.is_landed) {
					this.next_step = STEP.RUN;
				}
				break;
			}
		}
		
		while (this.next_step != STEP.NONE) {
			this.step = this.next_step;
			this.next_step = STEP.NONE;
			switch (this.step) {
			case STEP.JUNP:
				velocity.y = Mathf.Sqrt (
					2.0f * 9.8f * PlayerControl.JUNP_HEIGHT_MAX);
				this.is_key_released = false;
				break;
			}
			this.step_timer = 0.0f;
		}
		
		switch (this.step) {
		case STEP.RUN:
			velocity.x += PlayerControl.ACCELERATION * Time.deltaTime;
			if (Mathf.Abs (velocity.x) > PlayerControl.SPEED_MAX) {
				velocity.x *= PlayerControl.SPEED_MAX /
					Mathf.Abs (velocity.x);
			}
			break;
			
		case STEP.JUNP:
			do {
				if (! Input.GetMouseButtonUp (0)) {
					break;
				}
				
				if (this.is_key_released) {
					break;
				}
				if (velocity.y <= 0.0f) {
					break;
				}
				velocity.y *= JUNP_KEY_RELEASE_REDUCE;

				this.is_key_released = true;
			} while(false);
			break;
		}
		this.rigidbody.velocity = velocity;
	}
}
