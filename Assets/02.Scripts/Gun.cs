using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletImpact;
    public Transform crosshair;
    
    private ParticleSystem _bulletEffect;
    private AudioSource _bulletAudio;

    private void Start()
    {
        _bulletEffect = bulletImpact.GetComponent<ParticleSystem>();
        _bulletAudio = bulletImpact.GetComponent<AudioSource>();
        
    }

    private void Update()
    {
        ARAVRInput.DrawCrosshair(crosshair);
        
        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger))
        {
            ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);
            
            _bulletAudio.Stop();
            _bulletAudio.Play();
            
            Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
            RaycastHit hitInfo;

            int playerLayer = 1 << LayerMask.NameToLayer("Player");
            int towerLayer = 1 << LayerMask.NameToLayer("Tower");
            int layerMask = playerLayer | towerLayer;

            if (Physics.Raycast(ray, out hitInfo, 200, ~layerMask))
            {
                _bulletEffect.Stop();
                _bulletEffect.Play();
                bulletImpact.position = hitInfo.point;
                bulletImpact.forward = hitInfo.normal;

                if (hitInfo.transform.TryGetComponent(out IDamagable damagable))
                {
                    damagable.DamageAction(1, hitInfo.point, hitInfo.normal);
                }
                
                // if (hitInfo.transform.name.Contains("Drone"))
                // {
                //     DroneAI drone = hitInfo.transform.GetComponent<DroneAI>();
                //
                //     if (drone)
                //     {
                //         drone.OnDamageProcess();
                //     }
                // }
            }
        }
    }
}
