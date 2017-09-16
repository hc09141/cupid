using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RomancerEffects : MonoBehaviour {

    public ParticleSystem angry;
    public ParticleSystem clouds;
    public ParticleSystem rain;

    public void CauseAngry() {
        angry.Play();
    }

    public void CloudsPlay() {
        clouds.Play();
        rain.Play();
    }
}
