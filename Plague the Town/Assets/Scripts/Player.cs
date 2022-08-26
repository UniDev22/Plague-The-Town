using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (GunController))]
public class Player : LivingEntity {

	public float moveSpeed = 5;

	public Crosshairs crosshairs;

	Camera viewCamera;
	PlayerController controller;
	GunController gunController;
	Material playerColor;
	Color originalColor;
	protected override void Start () {
		base.Start();
	}

	void Awake()
	{
		controller = GetComponent<PlayerController> ();
		gunController = GetComponent<GunController> ();
		viewCamera = Camera.main;

		playerColor = GetComponent<Renderer>().material;
		originalColor = playerColor.color;
		FindObjectOfType<Spawner>().OnNewWave += GunSwitcher;
	}

	void GunSwitcher(int waveNumber){
		health = startingHealth;
		gunController.EquipGunIndex(waveNumber - 1);
	}


	void Update () {
		// Movement input
		Vector3 moveInput = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
		Vector3 moveVelocity = moveInput.normalized * moveSpeed;
		controller.Move (moveVelocity);

		// Look input
		Ray ray = viewCamera.ScreenPointToRay (Input.mousePosition);
		Plane groundPlane = new Plane (Vector3.up, Vector3.up * gunController.GunHeight);
		float rayDistance;

		if (groundPlane.Raycast(ray,out rayDistance)) {
			Vector3 point = ray.GetPoint(rayDistance);
			//Debug.DrawLine(ray.origin,point,Color.red);
			controller.LookAt(point);
			crosshairs.transform.position = point;
			crosshairs.DetectTargets(ray);
			if((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude > 1){
				gunController.Aim(point);
			}
			
			
		}

		// Weapon input
		if (Input.GetMouseButton(0)) {
			gunController.OnTriggerHold();
		}
		if(Input.GetMouseButtonUp(0)){
			gunController.OnTriggerRelease();
		}

		if(health <= 100){
			StartCoroutine(EnemyFlash());
		}
		if(Input.GetKeyDown(KeyCode.R)){
			gunController.Reload();
		}
	}
	IEnumerator EnemyFlash ()
 
    {
        GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<Renderer>().material.color = originalColor;
        StopCoroutine(EnemyFlash());
    }
}