using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [Header("----- Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---- Audio Clip ----------")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip checkpoint;
    public AudioClip wallTouch;
    public AudioClip portalin;
    public AudioClip portalout;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
}
