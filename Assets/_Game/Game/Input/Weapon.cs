using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
	public GameObject bulletPrefab;
	public Transform firePoint;
	public float fireForce = 20f;

	// public ShotConfig shotConfig;

	private float mCooldown;

	public void Awake() {
		// mCooldown = shotConfig.Cooldown;
	}

	public void Update() {
		mCooldown -= Time.deltaTime;
	}

	public void Fire() {
		if (mCooldown <= 0) {
			GameObject bullet = ObjectPoolMgr.Instance.GetObject(bulletPrefab, firePoint.position, Quaternion.LookRotation(firePoint.forward), null);
			//bullet.transform.position = firePoint.position;
			// bullet.transform.forward = firePoint.forward;
			//bullet.transform.rotation = Quaternion.LookRotation(firePoint.forward);
            Vector3 length = bullet.transform.forward * 50;
/*
			bullet.GetComponent<Bullet>().InitBullet(bulletPrefab, shotConfig, colorConfig);

			bullet.transform.DOMove(length, length.magnitude / fireForce).onComplete += delegate {
				bullet.GetComponent<Bullet>().Recycle();
			};
*/
			// mCooldown = shotConfig.Cooldown;
		}
	}
}
