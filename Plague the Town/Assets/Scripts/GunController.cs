
using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

	public Transform weaponHold;
	public Gun[] gunList;
	Gun equippedGun;

	void Start() {
		
	}

	public void EquipGun(Gun gunToEquip) {
		if (equippedGun != null) {
			Destroy(equippedGun.gameObject);
		}
		equippedGun = Instantiate (gunToEquip, weaponHold.position,transform.localRotation) as Gun;
		equippedGun.transform.parent = weaponHold;
	}

	public void EquipGunIndex(int weaponIndex){
		EquipGun(gunList[weaponIndex]);
	}

	public void OnTriggerHold() {
		if (equippedGun != null) {
			equippedGun.OnTriggerHold();
		}
	}

	public void OnTriggerRelease(){
		if(equippedGun != null){
			equippedGun.OnTriggerRelease();
		}
	}

	public float GunHeight{
		get{
			return weaponHold.position.y;
		}
	}

	public void Aim(Vector3 aimPoint){
		if(equippedGun != null){
			equippedGun.Aim(aimPoint);
		}
	}

	public void Reload(){
		if(equippedGun != null){
			equippedGun.Reload();
		}
	}
}