using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public static class UnityTools {

	static UnityTools() { }

	public static Vector3 WorldToCanvasSpace(Camera camera, RectTransform canvasRect, Vector3 worldPos) {
		Vector3 canvasPos = camera.WorldToViewportPoint(worldPos);

		canvasPos = new Vector3((canvasPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f), (canvasPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f), 0);

		return canvasPos;
	}

	public static void ShuffleArray<T>(ref List<T> array, int seed = -1) {
		for (int i = array.Count - 1; i >= 0; --i) {
			if (seed != -1)
				UnityEngine.Random.InitState(seed);
			int j = UnityEngine.Random.Range(0, i + 1);

			T tmp = array[i];
			array[i] = array[j];
			array[j] = tmp;
		}
	}

	public static List<T> UIRaycastMouse<T>() {

		PointerEventData pointerData = new PointerEventData(EventSystem.current) {
			pointerId = -1,
		};

		pointerData.position = Input.mousePosition;

		List<RaycastResult> raycasts = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerData, raycasts);

		List<T> results = new List<T>();
		for (int i = raycasts.Count - 1; i >= 0; i--) {
			T component = raycasts[i].gameObject.GetComponent<T>();

			if (component != null) {
				results.Add(component);
			}
		}

		return results;
	}

	public static T WorldRaycastMouse<T>(Vector3 mousePos, Camera camera) {
		RaycastHit hit;
		Ray ray = camera.ScreenPointToRay(mousePos);

		if (Physics.Raycast(ray, out hit)) {
			T component = hit.transform.GetComponent<T>();

			if (component != null) {
				return component;
			}
		}

		return default(T);
	}

	public static void DataLogs<T>(T[] array, string title) {
		string stringData = string.Format("{0}: {1}\n", title, array.Length);
		for (int i = 0; i < array.Length; i++) {
			if (array[i] != null) {
				stringData += "," + array[i].ToString();
			}
		}
		Debug.Log(stringData);
	}

	public static IEnumerator LoadImageFromURL(Image image, string url, Action<Sprite> result) {
		UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

		DownloadHandler handle = request.downloadHandler;

		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.ConnectionError) {
			Debug.Log(request.error);
		}
		else {
			//Load Image
			Texture2D texture2d = DownloadHandlerTexture.GetContent(request);

			Sprite sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);
			result?.Invoke(sprite);

			if (sprite != null) {
				image.sprite = sprite;
			}
		}
	}

	public static void LoadImageFromTexture2D(Image image, Texture2D tex) {
		Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
		image.overrideSprite = sprite;
	}

	public static void SetLocalScaleChildren(Transform parent, Vector3 scale) {
		for (int i = 0; i < parent.childCount; i++) {
			Transform child = parent.GetChild(i);
			child.localScale = scale;
		}
	}

	public static void SetLayer(GameObject obj, int layer, bool children = true) {
		obj.layer = layer;

		if (children) {
			for (int i = 0; i < obj.transform.childCount; i++) {
				SetLayer(obj.transform.GetChild(i).gameObject, layer);
			}
		}
	}

	public static void SetSpriteEnable(GameObject obj, bool enabled, bool children = true) {
		SpriteRenderer parent = obj.GetComponent<SpriteRenderer>();
		if (parent) {
			parent.enabled = enabled;
		}

		if (children) {
			SpriteRenderer[] spriteRenderers = obj.GetComponentsInChildren<SpriteRenderer>();
			foreach (SpriteRenderer component in spriteRenderers) {
				component.enabled = enabled;
			}
		}
	}

	public static void SetTextMeshEnable(GameObject obj, bool enabled, bool children = true) {
		TextMeshPro parent = obj.GetComponent<TextMeshPro>();
		if (parent) {
			parent.enabled = enabled;
		}

		if (children) {
			TextMeshPro[] textMeshPros = obj.GetComponentsInChildren<TextMeshPro>();
			foreach (TextMeshPro component in textMeshPros) {
				component.enabled = enabled;
			}
		}
	}

	#region Animations

	public static AnimationClip CreateAnimationClip(Transform[] transforms, float[] time) {
		Vector3[] positions = new Vector3[transforms.Length];
		Quaternion[] rotations = new Quaternion[transforms.Length];
		Vector3[] scales = new Vector3[transforms.Length];

		for (int i = 0; i < transforms.Length; i++) {
			positions[i] = transforms[i].localPosition;
			rotations[i] = transforms[i].localRotation;
			scales[i] = transforms[i].localScale;
		}

		AnimationClip clip = UnityTools.CreateAnimationClip(positions, rotations, scales, time);

		return clip;
	}

	public static AnimationClip CreateAnimationClip(Vector3[] destinations, Quaternion[] rotations, Vector3[] scales, float[] time) {
		AnimationCurve curve;

		// create a new AnimationClip
		AnimationClip clip = new AnimationClip {
			legacy = true
		};
		Keyframe[] keys = new Keyframe[time.Length];

		if (destinations != null && destinations.Length > 0) {
			// for X
			for (int i = 0; i < keys.Length; i++) {
				keys[i] = new Keyframe(time[i], destinations[i].x);
			}
			curve = new AnimationCurve(keys);
			clip.SetCurve("", typeof(Transform), "localPosition.x", curve);

			// For y
			for (int i = 0; i < keys.Length; i++) {
				keys[i] = new Keyframe(time[i], destinations[i].y);
			}
			curve = new AnimationCurve(keys);
			clip.SetCurve("", typeof(Transform), "localPosition.y", curve);

			// For z
			for (int i = 0; i < keys.Length; i++) {
				keys[i] = new Keyframe(time[i], destinations[i].z);
			}
			curve = new AnimationCurve(keys);
			clip.SetCurve("", typeof(Transform), "localPosition.z", curve);
		}

		if (rotations != null && rotations.Length > 0) {
			// Rotate x
			for (int i = 0; i < rotations.Length; i++) {
				keys[i] = new Keyframe(time[i], rotations[i].x);
			}
			curve = new AnimationCurve(keys);
			clip.SetCurve("", typeof(Transform), "localRotation.x", curve);

			// Rotate y
			for (int i = 0; i < rotations.Length; i++) {
				keys[i] = new Keyframe(time[i], rotations[i].y);
			}
			curve = new AnimationCurve(keys);
			clip.SetCurve("", typeof(Transform), "localRotation.y", curve);

			// Rotate z
			for (int i = 0; i < rotations.Length; i++) {
				keys[i] = new Keyframe(time[i], rotations[i].z);
			}
			curve = new AnimationCurve(keys);
			clip.SetCurve("", typeof(Transform), "localRotation.z", curve);

			// Rotate w
			for (int i = 0; i < rotations.Length; i++) {
				keys[i] = new Keyframe(time[i], rotations[i].w);
			}
			curve = new AnimationCurve(keys);
			clip.SetCurve("", typeof(Transform), "localRotation.w", curve);
		}

		if (scales != null && scales.Length > 0) {
			// Scale x
			for (int i = 0; i < scales.Length; i++) {
				keys[i] = new Keyframe(time[i], scales[i].x);
			}
			curve = new AnimationCurve(keys);
			clip.SetCurve("", typeof(Transform), "localScale.x", curve);

			// Scale y
			for (int i = 0; i < scales.Length; i++) {
				keys[i] = new Keyframe(time[i], scales[i].y);
			}
			curve = new AnimationCurve(keys);
			clip.SetCurve("", typeof(Transform), "localScale.y", curve);

			// Scale z
			for (int i = 0; i < scales.Length; i++) {
				keys[i] = new Keyframe(time[i], scales[i].z);
			}
			curve = new AnimationCurve(keys);
			clip.SetCurve("", typeof(Transform), "localScale.z", curve);
		}

		return clip;
	}

	public static void AddEventToAnim(ref AnimationClip clip, string eventName, float time) {
		AnimationEvent animEvent = new AnimationEvent() {
			functionName = eventName,
			time = time,
		};
		clip.AddEvent(animEvent);
	}

	public static void AddEventToAnim(ref AnimationClip clip, string eventName, float time, float param) {
		AnimationEvent animEvent = new AnimationEvent() {
			functionName = eventName,
			time = time,
			floatParameter = param
		};
		clip.AddEvent(animEvent);
	}

	public static void AddEventToAnim(ref AnimationClip clip, string eventName, float time, int param) {
		AnimationEvent animEvent = new AnimationEvent() {
			functionName = eventName,
			time = time,
			intParameter = param
		};
		clip.AddEvent(animEvent);
	}

	public static void AddEventToAnim(ref AnimationClip clip, string eventName, float time, string param) {
		AnimationEvent animEvent = new AnimationEvent() {
			functionName = eventName,
			time = time,
			stringParameter = param
		};
		clip.AddEvent(animEvent);
	}

	public static void AddEventToAnim(ref AnimationClip clip, string eventName, float time, UnityEngine.Object param) {
		AnimationEvent animEvent = new AnimationEvent() {
			functionName = eventName,
			time = time,
			objectReferenceParameter = param
		};
		clip.AddEvent(animEvent);
	}
	#endregion

#if UNITY_EDITOR
	public static List<T> TryGetAssets<T>(string folderPath) where T : UnityEngine.Object {
		// Gets all files with the Directory System.IO class
		string[] files = Directory.GetFiles(Application.dataPath + folderPath, "*.*", SearchOption.AllDirectories);
		List<T> assets = new List<T>();
		T asset = null;
		// move through all files
		foreach (var file in files) {
			// use the GetRightPartOfPath utility method to cut the path so it looks like this: Assets/folderblah
			string path = GetLastPartOfPath(file);

			// Then I try and load the asset at the current path.
			asset = AssetDatabase.LoadAssetAtPath<T>("Assets" + folderPath + path);

			// check the asset to see if it's not null
			if (asset) {
				// if the optional name is nothing then we skip this step
				assets.Add(asset);
			}
		}

		return assets;
	}

	private static string GetLastPartOfPath(string path) {
		string[] parts = path.Split('/');

		if (parts.Length > 0)
			return parts[parts.Length - 1];

		return "";
	}

	public static void DrawUILine(Color color, int thickness = 2, int padding = 10) {
		Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
		r.height = thickness;
		r.y += padding / 2;
		r.x -= 2;
		r.width += 6;
		EditorGUI.DrawRect(r, color);
	}

	public static string MakeSelectableComboBoxFromClass<T>(List<T> classes, string curClass, bool withBlank = false) {
		int addBlank = withBlank ? 1 : 0;
		string[] templateNames = new string[classes.Count + addBlank];
		int curSelected = 0;

		if (withBlank) {
			templateNames[0] = "";
		}

		for (int i = 0; i < classes.Count; i++) {
			templateNames[i + addBlank] = classes[i].GetType().ToString();
			if (templateNames[i + addBlank] == curClass) {
				curSelected = i + addBlank;
			}
		}

		curSelected = EditorGUILayout.Popup("Template Type", curSelected, templateNames);

		return templateNames[curSelected];
	}


	public static void ShowSpriteInInspector(Sprite sprite, Rect rect) {
		if (sprite != null) {
			GUI.enabled = false;
			EditorGUI.ObjectField(rect, sprite.texture, typeof(Texture2D), false);
			GUI.enabled = true;
		}
	}
   

#endif
}
