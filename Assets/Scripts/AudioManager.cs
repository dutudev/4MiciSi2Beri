using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] 
    private AudioSource oneShotSource;
    [SerializeField] 
    private AudioSource loopSource;

    [SerializeField] private List<AudioClip> clips = new List<AudioClip>();
    private bool _playedLoop = false;
    public static AudioManager instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlaySoundOnce(int clip)
    {
        oneShotSource.PlayOneShot(clips[clip]);
    }
    
    public void StartLoop(bool set)
    {
        if (set)
        {
            if (!_playedLoop)
            {
                _playedLoop = true;
                loopSource.Play();
            }
            else
            {
                loopSource.UnPause();
            }

        }
        else
        {
            loopSource.Pause();
        }
    }
}
