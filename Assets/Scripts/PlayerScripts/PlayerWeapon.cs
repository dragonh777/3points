using UnityEngine;
using System.Collections;
using Spine;
using Spine.Unity;

public class PlayerWeapon : MonoBehaviour
{

	//data
	//public new string name;
	//[SpineAnimation(startsWith: "aim")]
	//public string setupAnim;
	//[SpineAnimation(startsWith: "aim")]
	//public string idleAnim;
	//[SpineAnimation(startsWith: "aim")]
	//public string aimAnim;
	//[SpineAnimation(startsWith: "aim")]
	//public string fireAnim;
	//[SpineAnimation(startsWith: "aim")]
	//public string reloadAnim;
	//public GameObject casingPrefab;
	//public Transform casingEjectPoint;
	//public float minAngle = -40;
	//public float maxAngle = 40;
	//public float refireRate = 0.5f;
	//public int clipSize = 10;
	//public int clip = 10;
	//public int ammo = 50;

	//public Spine.Animation SetupAnim;
	//public Spine.Animation IdleAnim;
	//public Spine.Animation AimAnim;
	//public Spine.Animation FireAnim;
	//public Spine.Animation ReloadAnim;

	////states & locks
	//public bool reloadLock;
	//public float nextFireTime = 0;

	//public void CacheSpineAnimations(SkeletonData data)
	//{
	//	SetupAnim = data.FindAnimation(setupAnim);
	//	IdleAnim = data.FindAnimation(idleAnim);
	//	AimAnim = data.FindAnimation(aimAnim);
	//	FireAnim = data.FindAnimation(fireAnim);
	//	ReloadAnim = data.FindAnimation(reloadAnim);
	//}

	//public virtual void Setup()
	//{

	//}

	//public virtual void Fire()
	//{
	//	Debug.LogWarning("Not implemented!");
	//}

	//public virtual bool Reload()
	//{
	//	if (ammo == 0)
	//		return false;

	//	int refill = clipSize;
	//	if (refill > ammo)
	//		refill = clipSize - ammo;
	//	ammo -= refill;
	//	clip = refill;

	//	return true;
	//}

	//public virtual Vector2 GetRecoil()
	//{
	//	return Vector2.zero;
	//}
}