using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : IUpdate {
	List<ParticleInfo> mParticles = new List<ParticleInfo>();

	public void UpdateClass(float elapsedTime) {

		for (int i = mParticles.Count - 1; i >= 0; i--) {
			if (mParticles[i] == null || mParticles[i].ps == null) {
				mParticles.RemoveAt(i);
				continue;
			}

			ParticleSystem ps = mParticles[i].ps;
			// Add this to try simulate
			// ps.Simulate(elapsedTime, true);

			mParticles[i].duration -= elapsedTime;

			if (mParticles[i].duration <= 0) {
				ps.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
			}

			if (!ps.IsAlive(true)) {
				if (mParticles[i].prefabName == "")
					ObjectPoolMgr.Instance.ReuseObject(mParticles[i].prefabObj, mParticles[i].ps.gameObject);
				else
					ObjectPoolMgr.Instance.ReuseObject(mParticles[i].prefabName, mParticles[i].ps.gameObject);

				mParticles[i].actionAfterParticle?.Invoke();
				mParticles.RemoveAt(i);
			}
		}
	}

	public GameObject AddParticle(string particle, Vector3 pos, float overrideDuration = float.MaxValue, Transform parent = null, Action actionAfterParticle = null) {
		GameObject part = ObjectPoolMgr.Instance.GetObject(particle, parent);
		if (part == null)
			return null;

		ParticleSystem ps = part.GetComponent<ParticleSystem>();

		SetupParticle(ps, pos);

		ParticleInfo pInfo = new ParticleInfo {
			ps = ps,
			prefabName = particle,
			actionAfterParticle = actionAfterParticle,
			duration = overrideDuration
		};
		mParticles.Add(pInfo);

		return part;
	}

	public GameObject AddParticle(GameObject prefab, Vector3 pos, float overrideDuration = float.MaxValue, Transform parent = null, Action actionAfterParticle = null) {
		if (prefab == null)
			return null;

		GameObject part = ObjectPoolMgr.Instance.GetObject(prefab, parent);
		ParticleSystem ps = part.GetComponent<ParticleSystem>();

		SetupParticle(ps, pos);

		ParticleInfo pInfo = new ParticleInfo {
			ps = ps,
			prefabObj = prefab,
			actionAfterParticle = actionAfterParticle,
			duration = overrideDuration
		};
		mParticles.Add(pInfo);

		return part;
	}

	void SetupParticle(ParticleSystem part, Vector3 pos) {
		part.Stop();
		part.Clear(true);
		part.transform.position = pos;

		// Remove this to try simulate
		part.Play(true);
	}

	public class ParticleInfo {
		public ParticleSystem ps;
		public string prefabName = "";
		public GameObject prefabObj;
		public Action actionAfterParticle;

		public float duration;

		public ParticleSystemEvent[] particleSystemEvents;
	}
}

[Serializable]
public struct ParticleSystemEvent {
	[Range(0, 1)]
	public float Time;
	public Action Event;
	public bool Achieved;

	public ParticleSystemEvent(float Time, Action Event) {
		this.Time = Time;
		this.Event = Event;
		Achieved = false;
	}
}