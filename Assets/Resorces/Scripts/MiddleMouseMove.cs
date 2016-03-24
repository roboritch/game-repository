using UnityEngine;
using System.Collections;

public class MiddleMouseMove : MonoBehaviour {
	public float scrollingSpeed;

	public bool invertX;
	public bool invertY;

	private float lPosX;
	private float lPosY;
	private float cPosX;
	private float cPosY;

	public Camera cam;
	public float zoomSpeed;
	public bool invertZoom;

	// Use this for initialization
	void Start() {
		cPosX = Input.mousePosition.x;
		cPosY = Input.mousePosition.y;
		lPosX = Input.mousePosition.x;
		lPosY = Input.mousePosition.y;
	}
	
	// Update is called once per frame
	void Update() {

		#region mouse scrolling
		cPosX = Input.mousePosition.x;
		cPosY = Input.mousePosition.y;

		if(Input.GetMouseButtonDown(2) || Input.GetMouseButton(2)) {

			//lots of code but best implementation
			//invertions can be changed at any time
			if(invertX && invertY) {
				transform.Translate(-(cPosX - lPosX) * scrollingSpeed, -(cPosY - lPosY) * scrollingSpeed, 0f);
			} else if(invertX) {
				transform.Translate(-(cPosX - lPosX) * scrollingSpeed, (cPosY - lPosY) * scrollingSpeed, 0f);
			} else if(invertY) {
				transform.Translate((cPosX - lPosX) * scrollingSpeed, -(cPosY - lPosY) * scrollingSpeed, 0f);
			} else {
				transform.Translate((cPosX - lPosX) * scrollingSpeed, (cPosY - lPosY) * scrollingSpeed, 0f);
			}
		}
		if(invertZoom) {
			cam.orthographicSize += zoomSpeed * Input.GetAxis("Mouse ScrollWheel");
		} else {
			cam.orthographicSize -= zoomSpeed * Input.GetAxis("Mouse ScrollWheel");
		}

		lPosX = cPosX;
		lPosY = cPosY;

		#endregion

	}
}
