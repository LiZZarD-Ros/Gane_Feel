using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SoundsCollectionSO : ScriptableObject
{
    [Header("SFX")]
    public SoundSO[] GunShoot;
    public SoundSO[] Jump;
    public SoundSO[] Splat;

    [Header("Music")]
    public SoundSO[] FightMusic;
    public SoundSO[] DiscoParty;

}
