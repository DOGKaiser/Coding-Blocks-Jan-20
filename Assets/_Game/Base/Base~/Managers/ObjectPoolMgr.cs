using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolMgr {
    private static readonly ObjectPoolMgr instance = new ObjectPoolMgr();
    static ObjectPoolMgr() { }
    private ObjectPoolMgr() { }
    public static ObjectPoolMgr Instance {
        get { return instance; }
    }

    Dictionary<string, GameObject> mLoadedObject = new Dictionary<string, GameObject>();
	Dictionary<int, Queue<GameObject>> mCreatedObjects = new Dictionary<int, Queue<GameObject>>();

	public bool IsLoaded(string path) {
		GameObject loadedObject;
		mLoadedObject.TryGetValue(path, out loadedObject);

		if (loadedObject == null) {
			return false;
		}

		return true;
	}

	GameObject LoadObject(string path) {
		GameObject loadedObject;
		mLoadedObject.TryGetValue(path, out loadedObject);

		if (loadedObject == null) {
			loadedObject = Resources.Load<GameObject>(path);

			mLoadedObject.Add(path, loadedObject);
//			Debug.LogWarning("Loaded: " + loadedObject.name);
		}

		return loadedObject;
	}

	public GameObject GetObject(string path, Transform parent) {
		GameObject prefab;
		if (mLoadedObject.ContainsKey(path)) {
			prefab = mLoadedObject[path];
		}
		else {
			prefab = LoadObject(path);
		}

		return GetObject(prefab, parent);
	}

	public void ReuseObject(string path, GameObject createdObj) {
		createdObj.SetActive(false);
		GameObject prefab;
		mLoadedObject.TryGetValue(path, out prefab);

		if (prefab == null)
			return;

		Queue<GameObject> createdObjects;
		mCreatedObjects.TryGetValue(prefab.GetInstanceID(), out createdObjects);

		if (createdObjects != null) {
			createdObjects.Enqueue(createdObj);
		}
	}

	public void ReuseObject(GameObject prefab, GameObject createdObj) {
		createdObj.SetActive(false);
		Queue<GameObject> createdObjects;
		mCreatedObjects.TryGetValue(prefab.GetInstanceID(), out createdObjects);

		if (createdObjects != null) {
			createdObjects.Enqueue(createdObj);
		}
	}

	// Object already loaded
	public GameObject GetObject(GameObject prefab, Transform parent) {
		Queue<GameObject> createdObjects;
		int id = prefab.GetInstanceID();
		mCreatedObjects.TryGetValue(id, out createdObjects);

		//		Debug.LogWarning("Loaded: " + mCreatedObjects.Count + " Created: " + objCount);
		
		if (createdObjects != null) {
			while (createdObjects.Count > 0) {
				GameObject obj = createdObjects.Dequeue();
				if (obj != null) {
					obj.SetActive(true);
                    obj.transform.SetParent(parent, false);
					SetupValues(obj, prefab.transform);
					return obj;
				}
			}
		}
		else {
			createdObjects = new Queue<GameObject>();
			mCreatedObjects.Add(id, createdObjects);
		}

		GameObject poolObject = GameObject.Instantiate(prefab, parent, false);
		poolObject.name = poolObject.name.Replace("(Clone)", "");
		
		return poolObject;
	}

	void SetupValues(GameObject obj, Transform defaultT) {
		obj.transform.localPosition = defaultT.localPosition;
		obj.transform.localScale = defaultT.localScale;
		obj.transform.localRotation = defaultT.localRotation;
	}
}
