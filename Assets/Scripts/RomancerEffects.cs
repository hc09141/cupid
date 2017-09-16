using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RomancerEffects : MonoBehaviour {

    public ParticleSystem angry;
    public ParticleSystem clouds;
    public ParticleSystem rain;
    public ParticleSystem happy;

    public void CauseAngry() {
        angry.Play();
    }

    public void CloudsPlay() {
        clouds.Play();
        rain.Play();
    }

    public void HappyEffects() {
        angry.Stop();
        clouds.Stop();
        rain.Stop();
        happy.Play();
    }
}
