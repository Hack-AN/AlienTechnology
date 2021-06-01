using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_manage : MonoBehaviour
{
    public AudioClip[] intro_music;
    AudioSource soundSource;
    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        soundSource = GetComponent<AudioSource>();
        StartCoroutine("Playlist");
    }

    IEnumerator Playlist()
    {
        soundSource.clip = intro_music[index];
        soundSource.Play();
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if (!soundSource.isPlaying)
            {
                index = 1 - index;
                soundSource.clip = intro_music[index];
                soundSource.Play();
                soundSource.loop = true;
            }
        }
    }
}
