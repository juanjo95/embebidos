using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public Transform leftBound;
	public Transform rightBound;
	public float speed = 5f;
	public bool isFollowing;


	private float arrowX;
	public bool allowToMove;

	private float dragSpeed = 0.05f;
	private float timeSinceShot;
	private Vector3 velocity = Vector3.zero;
	private Vector3 startPosition;
	private Vector3 camPos;

	// Use this for initialization
	void Start () {
		startPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameplayController.instance.gameInProgress) {
			if (isFollowing) {
				if (GameObject.FindGameObjectWithTag ("Player Bullet") != null) {
					MoveCameraFollow ();
				}
			} else { 
				if (!GameplayController.instance.player.GetChild (0).transform.GetComponent<Cannon> ().readyToShoot) {
					MoveCameraBackToStart ();
					AfterShotMoveAgain ();
					allowToMove = false;
				} else {
					timeSinceShot = 0;
					allowToMove = true;
				}
				
			}
			MoveCamera ();
		}

	}

	void MoveCameraFollow(){
		if(target != null){
			transform.position = Vector3.Lerp (new Vector3 (transform.position.x, 0, -10f), new Vector3 (Mathf.Clamp (target.transform.position.x, leftBound.position.x, rightBound.position.x), 0, transform.position.z), Time.deltaTime * 10);
		}
	}

	void MoveCameraBackToStart(){
		transform.position = Vector3.MoveTowards (transform.position, startPosition, Time.deltaTime * 5f);
	}

	void AfterShotMoveAgain(){
		timeSinceShot += Time.deltaTime;
		if (timeSinceShot > 2f) {
			if(startPosition == transform.position){
				GameplayController.instance.player.GetChild (0).transform.GetComponent<Cannon> ().readyToShoot = true;
			}
		}
	}

	void MoveCamera(){
		arrowX = Input.GetAxis ("Horizontal");

		Transform cam = gameObject.transform;

		if (allowToMove) {
			if (arrowX != 0) {
				cam.position = cam.position + (new Vector3 (arrowX, 0, 0) * speed * Time.deltaTime);
				float camX = cam.position.x;
				camX = Mathf.Clamp (camX, leftBound.transform.position.x, rightBound.transform.position.x);
				cam.position = new Vector3 (camX, cam.position.y, cam.position.z);
			}
		}
	}	
		
}
