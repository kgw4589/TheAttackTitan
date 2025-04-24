using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Transform _explosion;
    private ParticleSystem _expEffect;
    private AudioSource _expAudio;

    private float _range = 5f;

    private void Start()
    {
        _explosion = GameObject.Find("Explosion").transform;
        _expEffect = _explosion.GetComponent<ParticleSystem>();
        _expAudio = _explosion.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        int layerMask = 1 << LayerMask.NameToLayer("Drone");
        Collider[] drones = Physics.OverlapSphere(transform.position, _range, layerMask);

        for (int i = 0; i < drones.Length; i++)
        {
            Destroy(drones[i].gameObject);
        }

        _explosion.position = transform.position;

        _expEffect.Play();
        _expAudio.Play();
        
        Destroy(gameObject);
    }
}
