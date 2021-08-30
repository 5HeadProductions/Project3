using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class FeedbackManager : MonoBehaviour
{
    //class purpose is to play sound or feedback for any gameobjects that become disabled but still need to play a feedback

    
    public ParticleSystem explosionParticle;
    public void ShipExplosion(Vector3 explosionPosition){
        ParticleSystem particles = Instantiate(explosionParticle,explosionPosition, Quaternion.identity);
        Destroy(particles, 2f);
    }

}
