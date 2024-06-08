using UnityEngine;
using System;
using System.Collections;

public enum SoundType
{
    WIN_ROUND,
    LOSE_ROUND,
    WIN_MATCH,
    LOSE_MATCH,
    BUTTON_PRESS,
    ZILCH,
    SHUFFLE_AND_DELIVER
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    [SerializeField] private AudioClip[] musicList;
    public static SoundManager instance;
    public AudioSource audSource;
    [SerializeField] private AudioSource buttonAudioSource;//for pitched increased
    [SerializeField] private AudioSource musicAudioSource;

    private float originalPitch = 1f;
    private float gemPitch = 1f;
    private float pitchIncrease = 0.1f;
    private float pitchResetDelay = 2f;

    private void Awake()
    {
        instance = this;
        audSource = GetComponent<AudioSource>();

        musicAudioSource.loop = true;
        musicAudioSource.volume = 0.05f;
    }

    //Sound type and volume
    public void PlaySound(SoundType sound, float vol = 0.5f)
    {
        //instance.audSource.PlayOneShot(instance.soundList[(int)sound], vol);
        AudioClip[] clips = soundList[(int)sound].Sounds;
        if (clips.Length <= 0) return;
        AudioClip rndClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        audSource.PlayOneShot(rndClip, vol);
    }

    public void StopSound()
    {
        audSource.Stop();
    }

    public void PlaybuttonPressedSound()
    {
        PlaySoundWithIncreasedPitch(SoundType.BUTTON_PRESS, ref gemPitch);
        StartCoroutine(ResetPitchAfterDelay(SoundType.BUTTON_PRESS, pitchResetDelay));
    }

    private void PlaySoundWithIncreasedPitch(SoundType sound, ref float currentPitch)
    {
        buttonAudioSource.pitch = currentPitch;
        PlaySoundOnSource(buttonAudioSource, sound);
        currentPitch += pitchIncrease;
    }

    private void PlaySoundOnSource(AudioSource source, SoundType sound, float vol = 0.5f)
    {
        AudioClip[] clips = soundList[(int)sound].Sounds;
        if (clips.Length <= 0) return;
        AudioClip rndClip = clips[UnityEngine.Random.Range(0, clips.Length)];     
        source.PlayOneShot(rndClip, vol);
    }

    private IEnumerator ResetPitchAfterDelay(SoundType sound, float delay)
    {
        yield return new WaitForSeconds(delay);
        buttonAudioSource.pitch = originalPitch;
        if (sound == SoundType.BUTTON_PRESS)
        {
            gemPitch = originalPitch;
        }
    }

    public void PlayMusic(int index)
    {
        if (index < 0 || index >= musicList.Length) return;
        musicAudioSource.clip = musicList[index];
        musicAudioSource.Play();

    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }


#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
#endif
}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}