using UnityEngine;
using System.Collections;

public class TouchControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		int fingers = Input.touchCount;
		if(fingers == 2) {
			var firstTouch = Input.touches[0];
			var secondTouch = Input.touches[1];
			
			if(firstTouch.phase == TouchPhase.Ended && secondTouch.phase == TouchPhase.Ended) {
				var firstDelta = firstTouch.deltaPosition.normalized;
				var secondDelta = secondTouch.deltaPosition.normalized;
				var z = Mathf.Abs(firstDelta.magnitude + secondDelta.magnitude) * 100f;
				if(firstDelta.magnitude < 0 && secondDelta.magnitude > 0) {
					rigidbody.AddRelativeForce(new Vector3(0, 0, -z));
				} else {
					rigidbody.AddRelativeForce(new Vector3(0, 0, z));
				}
			}
		}

		var wheel = Input.GetAxis("Mouse ScrollWheel");
		if(wheel != 0) {
			rigidbody.AddRelativeForce(new Vector3(0, 0, wheel * 1000f));
		}

		transform.LookAt(Vector3.zero);
	}
}
