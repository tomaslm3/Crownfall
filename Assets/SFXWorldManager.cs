using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXWorldManager : MonoBehaviour
{
    public static SFXWorldManager Instance { get; private set; }
    AudioSource audioSource;

    [SerializeField] private AudioClip[] sfxClips;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(AudioClip sfx) {
        if (sfx != null) {
            audioSource.PlayOneShot(sfx);
        } else {
            Debug.LogWarning($"Sound '{sfx}' not found in SFX clips!");
        }
    }

    public void PlaySFX(string sfxName) {
        AudioClip sfx = FindClip(sfxName);
        if (sfx != null) {
            audioSource.PlayOneShot(sfx);
        } else {
            Debug.LogWarning($"Sound '{sfx}' not found in SFX clips!");
        }
    }

    private AudioClip FindClip(string clipName) {
        foreach (var clip in sfxClips) {
            if (clip.name == clipName) {
                return clip;
            }
        }
        Debug.LogWarning($"SFX Clip '{clipName}' not found!");
        return null;
    }
}
