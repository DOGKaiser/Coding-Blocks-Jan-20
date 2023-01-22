using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundAnim : MonoBehaviour
{
    public void PlaySound(AudioClip clip) {
        AudioManager.Instance.PlayClip(clip);
    }
}
