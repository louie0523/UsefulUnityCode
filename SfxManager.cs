using UnityEngine;
using System.Collections.Generic;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance;
    public AudioSource SfxAudio;
    public AudioSource BgmAudio;

    private Dictionary<string, AudioClip> sfxClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SfxAudio = GetComponent<AudioSource>();
            BgmAudio = transform.GetChild(0).GetComponent<AudioSource>();
            LoadAllClips();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadAllClips()
    {
        sfxClips = new Dictionary<string, AudioClip>();
        AudioClip[] loadedClips = Resources.LoadAll<AudioClip>("GameData/Sound");

        foreach (var clip in loadedClips)
        {
            if (!sfxClips.ContainsKey(clip.name))
                sfxClips.Add(clip.name, clip);
        }

        Debug.Log($"총 {sfxClips.Count}개의 사운드 클립이 로드되었습니다.");
    }

    public void PlaySound(string soundName)
    {
        if (sfxClips.TryGetValue(soundName, out var clip))
        {
            SfxAudio.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"사운드를 찾을 수 없습니다: {soundName}");
        }
    }

    public void PlayBgm(string soundName)
    {
        if (sfxClips.TryGetValue(soundName, out var clip))
        {
            BgmAudio.Stop();
            BgmAudio.loop = true;
            BgmAudio.clip = sfxClips[soundName];
            BgmAudio.Play();
        }
        else
        {
            Debug.LogWarning($"사운드를 찾을 수 없습니다: {soundName}");
        }
    }

    public void StopBgms()
    {
        BgmAudio.Stop();
    }
}
