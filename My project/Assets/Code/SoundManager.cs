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

    private void Awake()
    {
        Instance = this;
    }
}
