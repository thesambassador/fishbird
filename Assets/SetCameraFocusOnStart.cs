using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Com.LuisPedroFonseca.ProCamera2D;

public class SetCameraFocusOnStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ProCamera2D cam = Camera.main.GetComponent<ProCamera2D>();
		cam.AddCameraTarget(this.transform);
	}

}
