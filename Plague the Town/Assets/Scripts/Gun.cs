using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public enum FireMode {Auto, Burst, Single};
	public FireMode fireMode;

	[Header("Gun Mechanics")]
	public Transform[] projectileSpawn;
	public ProjectileController projectile;
	public float msBetweenShots = 100;
	public float muzzleVelocity = 35;
	public int burstCount;
	public int magazineSize;
	public float reloadTime = .2f;
	int bulletsRemaining;
	bool isReloading = false;

	public Transform shell;
	public Transform shellEjection;
	//MuzzleFlash muzzleflash;
	float nextShotTime;

	bool triggerReleasedSinceLastShot;
	int shotsRemainingInBurst;

	[Header("Recoil")]
	Vector3 recoilDamp;
	public Vector2 kickMinMax = new Vector2(.1f, .3f);
	public Vector2 recoilMinMax = new Vector2(3, 5);
	float recoilAngleDamp;
	float recoilAngle;
	public float maxRecoilAngle = 30;
	public float smoothDampTime = 0.1f;

	public AudioClip shootAudio;
	public AudioClip reloadAudio;

	void Start() {
		//muzzleflash = GetComponent<MuzzleFlash> ();
		shotsRemainingInBurst = burstCount;
		bulletsRemaining = magazineSize;
	}

	void LateUpdate()
	{
		transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilDamp, smoothDampTime);
		recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilAngleDamp, smoothDampTime);
		transform.localEulerAngles = transform.localEulerAngles + Vector3.left * recoilAngle;

		if(!isReloading && bulletsRemaining == 0){
			Reload();
		}
	}

	void Shoot() {

		if (!isReloading && Time.time > nextShotTime && bulletsRemaining > 0) {
			if (fireMode == FireMode.Burst) {
				if (shotsRemainingInBurst == 0) {
					return;
				}
				shotsRemainingInBurst --;
			}
			else if (fireMode == FireMode.Single) {
				if (!triggerReleasedSinceLastShot) {
					return;
				}
			}

			for (int i =0; i < projectileSpawn.Length; i ++) {
				if(bulletsRemaining == 0){
					break;
				}
				bulletsRemaining--;
				nextShotTime = Time.time + msBetweenShots / 1000;
				ProjectileController newProjectile = Instantiate (projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as ProjectileController;
				newProjectile.SetSpeed (muzzleVelocity);
			}

			Instantiate(shell,shellEjection.position, shellEjection.rotation);
			//muzzleflash.Activate();
			transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y);
			recoilAngle += Random.Range(recoilMinMax.x, recoilMinMax.y);
			recoilAngle = Mathf.Clamp(recoilAngle, 0, maxRecoilAngle);

			AudioManager.instance.PlaySound(shootAudio, transform.position);
		}
	}

	public void OnTriggerHold() {
		Shoot ();
		triggerReleasedSinceLastShot = false;
	}

	public void OnTriggerRelease() {
		triggerReleasedSinceLastShot = true;
		shotsRemainingInBurst = burstCount;
	}

	public void Aim(Vector3 aimPoint){
		transform.LookAt(aimPoint);
		
	}

	public void Reload(){
		if(!isReloading && bulletsRemaining != magazineSize){
			StartCoroutine(AnimateReload());
			AudioManager.instance.PlaySound(reloadAudio, transform.position);
		}
		
	}

	IEnumerator AnimateReload(){
		isReloading = true;
		yield return new WaitForSeconds(reloadTime);
		float reloadSpeed = 1f / reloadTime;

		float percent = 0;
		Vector3 initialRot = transform.localEulerAngles;
		float maxReloadAngle = 30f;
		while(percent < 1){
			percent += Time.deltaTime * reloadSpeed;
			float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
			float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
			transform.localEulerAngles = initialRot + Vector3.left * reloadAngle;

			yield return null;
		}
		isReloading = false;
		bulletsRemaining = magazineSize;
	}

}