using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if CLIENT && FACEBOOK

using Facebook.Unity;


public class FacebookMgr {

	const int ChecksNeededForLogin = 2;

	static FacebookMgr instance;

	public string profileID = "";
	public string profileName = "";
	public string profilePicURL = null;
	// public Texture2D profilePic = null;

	public List<string> friendIDs = new List<string>();
	public List<string> friendNames = new List<string>();
	//public List<Texture2D> friendPics = new List<Texture2D>();
	public List<string> friendPics = new List<string>();

	int loginCheck = ChecksNeededForLogin;

	public delegate void OnFacebookLoggedIn();
	public event OnFacebookLoggedIn FacebookLoggedIn;

	public delegate void OnFacebookLoggedOut();
	public event OnFacebookLoggedOut FacebookLoggedOut;

	public static FacebookMgr self {
		get {
			if (instance == null) {
				instance = new FacebookMgr();
			}
			return instance;
		}
	}

	public bool IsInitialized() {
		return FB.IsInitialized;
	}

	public bool IsLoggedIn() {
		return FB.IsLoggedIn;
	}

	public bool IsDoneLoggingIn() {
		return loginCheck == ChecksNeededForLogin;
	}


	// ----------------------------------------------------------

	public void Init() {
		if (!FB.IsInitialized) {
			loginCheck = 0;
			FB.Init(FinishLoggingIn);
		}
	}

	public void FBLogin() {
		loginCheck = 0;
		FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, this.HandleLoginResult);
	}

	public void FBLogout() {
		friendIDs.Clear();
		friendNames.Clear();
		friendPics.Clear();

		FB.LogOut();
		FacebookLoggedOut?.Invoke();
	}

	void RequestFBProfilePic() {
		FB.API("/me/picture", HttpMethod.GET, this.ProfilePhotoCallback);
	}

	void ProfilePhotoCallback(IGraphResult result) {
		if (string.IsNullOrEmpty(result.Error) && result.Texture != null) {
			Debug.LogWarning("Get profile Pic");
			profilePicURL = "";
			// this.profilePic = result.Texture;
		}

		loginCheck++;
		CheckIfFinishedLoggingIn();

		this.HandleResult(result);
	}

	void RequestFBName() {
		FB.API("/me?fields=id,name,picture", HttpMethod.GET, NameCallback);
	}

	void NameCallback(IResult result) {
		if (result.Error == null) {
			profileID = "" + result.ResultDictionary["id"];
			profileName = "" + result.ResultDictionary["name"];
			profilePicURL = ((Dictionary<string, object>)((Dictionary<string, object>)((Dictionary<string, object>)result.ResultDictionary)["picture"])["data"])["url"].ToString();
		}
		loginCheck++;
		CheckIfFinishedLoggingIn();
	}

	void RequestFBFriends() {
		FB.API("/me/friends", HttpMethod.GET, result => {
				var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
				var friendsList = (List<object>)dictionary["data"];

				foreach (var dict in friendsList) {
					Debug.LogWarning(((Dictionary<string, object>)dict).ToJson().ToString());
					friendIDs.Add(((Dictionary<string, object>)dict)["id"].ToString());

					if (((Dictionary<string, object>)dict).ContainsKey("gaming_name")) {
						friendNames.Add(((Dictionary<string, object>)dict)["gaming_name"].ToString());
						friendPics.Add(((Dictionary<string, object>)((Dictionary<string, object>)((Dictionary<string, object>)dict)["gaming_picture"])["data"])["url"].ToString());
					}
					else {
						friendNames.Add(((Dictionary<string, object>)dict)["name"].ToString());
						friendPics.Add(((Dictionary<string, object>)((Dictionary<string, object>)((Dictionary<string, object>)dict)["picture"])["data"])["url"].ToString());
					}
				}

				//if (friendsList.Count == 0)
				loginCheck++;

				CheckIfFinishedLoggingIn();
		}
		);
	}

#if false
	private void FriendsPhotos(IGraphResult result) {
		if (string.IsNullOrEmpty(result.Error) && result.Texture != null) {
			friendPics.Add(result.Texture);
		}

		Debug.LogWarning(friendPics.Count + " " + friendNames.Count);
		if (friendPics.Count == friendNames.Count) {
			// ClientSendData.Instance.SendFBFriends();
			loginCheck++;
			CheckIfFinishedLoggingIn();
		}
		
		this.HandleResult(result);
	}
#endif
	// ----------------------------------------------------------------------------------------------

	public void FinishLoggingIn() {
		loginCheck = 0;
		if (FB.IsLoggedIn) {
			Debug.LogWarning("Get prof pic and friend pics");
			RequestFBName();
			// RequestFBProfilePic();
			RequestFBFriends();
		}
		else
			loginCheck = ChecksNeededForLogin;
	}

	void CheckIfFinishedLoggingIn() {
		if (IsDoneLoggingIn()) {
			FacebookLoggedIn?.Invoke();
		}
	}

	protected void HandleLoginResult(IResult result) {
		if (result == null) {
			Debug.LogWarning("Null Response\n");
			loginCheck = ChecksNeededForLogin;
			return;
		}

		// Some platforms return the empty string instead of null.
		if (!string.IsNullOrEmpty(result.Error)) {
			Debug.LogWarning("Error - Check log for details\n" + "Error Response:\n" + result.Error);
			loginCheck = ChecksNeededForLogin;
		}
		else if (result.Cancelled) {
			Debug.LogWarning("Cancelled - Check log for details\n" + "Cancelled Response:\n" + result.RawResult);
			loginCheck = ChecksNeededForLogin;
		}
		else if (!string.IsNullOrEmpty(result.RawResult)) {
			Debug.LogWarning("Success - Check log for details\n" + "Success Response:\n" + result.RawResult);
			if (FB.IsLoggedIn) {
				FinishLoggingIn();
			}
			else
				loginCheck = ChecksNeededForLogin;
		}
		else {
			Debug.LogWarning("Empty Response\n");
			loginCheck = ChecksNeededForLogin;
		}
	}

	protected void HandleResult(IResult result) {
		if (result == null) {
			Debug.LogWarning("Null Response\n");
			return;
		}

		// Some platforms return the empty string instead of null.
		if (!string.IsNullOrEmpty(result.Error)) {
			Debug.LogWarning("Error - Check log for details\n" + "Error Response:\n" + result.Error);
		}
		else if (result.Cancelled) {
			Debug.LogWarning("Cancelled - Check log for details\n" + "Cancelled Response:\n" + result.RawResult);
		}
		else if (!string.IsNullOrEmpty(result.RawResult)) {
			Debug.LogWarning("Success - Check log for details\n" + "Success Response:\n" + result.RawResult);
		}
		else {
			Debug.LogWarning("Empty Response\n");
		}
	}
}

#endif