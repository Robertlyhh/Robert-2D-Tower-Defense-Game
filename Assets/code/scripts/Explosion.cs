using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip[] audioClips;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioClips[0]);
        StartCoroutine(explosion());
        
    }

    private IEnumerator explosion()
    {
        yield return new WaitForSeconds(audioClips[0].length);
        Destroy(gameObject);
    }
}
