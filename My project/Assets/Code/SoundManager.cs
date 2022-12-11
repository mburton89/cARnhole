using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource boardPlacedSound;
    public AudioSource bagHitSound;
    public AudioSource onePointSound;
    public AudioSource threePointsSound;
    public AudioSource newRoundSound;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayBagHitSound()
    {
        float rand = Random.Range(0.9f, 1.1f);
        bagHitSound.pitch = rand;
        bagHitSound.Play();
    }
}
