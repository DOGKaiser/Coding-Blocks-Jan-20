using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance {
        get {
            if (instance == null)
                Create();
            return instance; }
    }
    static AudioManager instance;

    GameObject mAudioObject;

    List<AudioSource> mAudios = new List<AudioSource>();
    AudioSource mMusic;

    bool mMuteSound;
    bool mMuteMusic;
    float mSoundVolume;
    float mMusicVolume;

    // Audio Manager doesn't exist - create it
    private static void Create() {
        GameObject obj = new GameObject();
        obj.name = "AudioManager";
        AudioManager am = obj.AddComponent<AudioManager>();
        instance = am;
        am.Init();
    }

    // Init the Audio Manager
    private void Init() {
        mMuteSound = 1 == PlayerPrefs.GetInt("MuteSound", 0);
        mMuteMusic = 1 == PlayerPrefs.GetInt("MuteMusic", 0);
        mSoundVolume = PlayerPrefs.GetFloat("SoundVolume", 1);
        mMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);

        mAudioObject = Resources.Load<GameObject>("AudioSource");
    }

    // Remove sounds if they don't exist, reuse sounds if they are done playing.
    public void Update() {
        for (int i = mAudios.Count - 1; i >= 0; i--) {
            if (mAudios[i] == null) {
                mAudios.RemoveAt(i);
                continue;
            }

            if (!mAudios[i].isPlaying) {
                ObjectPoolMgr.Instance.ReuseObject(mAudioObject, mAudios[i].gameObject);
                mAudios.RemoveAt(i);
            }
        }
    }

    // ---------------------------------------------------------------

    public void PlayMusic(AudioClip audioClip, float fadeStart, float fadeDuration) {
        if (audioClip != null) {
            if (mMusic == null) {
                GameObject obj = ObjectPoolMgr.Instance.GetObject(mAudioObject, transform);
                obj.name += "Music";
                mMusic = obj.GetComponent<AudioSource>();
                mMusic.loop = true;
            }
            if (mMusic != null) {
                mMusic.Stop();
                mMusic.clip = audioClip;

                if (mMuteMusic)
                    return;

                mMusic.DOKill();
                mMusic.volume = fadeStart * mMusicVolume;

                mMusic.volume = mMusic.volume;
                mMusic.Play();
                mMusic.DOFade(mMusicVolume, fadeDuration);
            }
        }
    }

    public void ToggleMusic() {
        mMuteMusic = !mMuteMusic;
        if (mMusic != null) {
            if (mMuteMusic)
                mMusic.Stop();
            else {
                PlayMusic(mMusic.clip, 0.05f, 8);
            }
        }

        PlayerPrefs.SetInt("MuteMusic", mMuteMusic ? 1 : 0);
    }

    public void SetMusicVolume(float volume) {
        mMusicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", mMusicVolume);

        if (mMusic != null) {
            mMusic.DOKill();
            mMusic.volume = mMusicVolume;
        }
    }

    public bool IsMusicMuted() {
        return mMuteMusic;
    }

    public float GetMusicVolume() {
        return mMusicVolume;
    }

    // ---------------------------------------------------------------

    public void PlayClip(AudioClip audioClip, float volume) {
        if (mMuteSound)
            return;

        if (audioClip != null) {
            GameObject obj = ObjectPoolMgr.Instance.GetObject(mAudioObject, transform);
            if (obj != null) {
                AudioSource newAudio = obj.GetComponent<AudioSource>();

                if (newAudio != null) {
                    newAudio.clip = audioClip;
                    newAudio.volume = mSoundVolume * volume;
                    newAudio.loop = false;
                    newAudio.Play();
                    mAudios.Add(newAudio);
                }
            }
        }
    }

    public void PlayClip(AudioClip audioClip) {
        PlayClip(audioClip, 1);
    }

    public void ToggleSound() {
        mMuteSound = !mMuteSound;

        for (int i = mAudios.Count - 1; i >= 0; i--) {
            if (mMuteSound)
                mAudios[i].mute = true;
            else
                mAudios[i].mute = false;
        }

        PlayerPrefs.SetInt("MuteSound", mMuteSound ? 1 : 0);
    }

    public bool IsSoundMuted() {
        return mMuteSound;
    }

    public void SetSoundVolume(float volume) {
        mSoundVolume = volume;
        PlayerPrefs.SetFloat("SoundVolume", mSoundVolume);

        for (int i = mAudios.Count - 1; i >= 0; i--) {
            mAudios[i].volume = mSoundVolume;
        }
    }

    public float GetSoundVolume() {
        return mSoundVolume;
    }
}
