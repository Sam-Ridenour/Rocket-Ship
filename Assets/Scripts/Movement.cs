using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float thrustForce = 1000f;
    [SerializeField] float thrustRotation = 100f;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem leftEngineParticle;
    [SerializeField] ParticleSystem rightEngineParticle;

    Rigidbody playerRb;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotation();
        }

    }

    void StartThrusting()
    {
        playerRb.AddRelativeForce(Vector3.up * thrustForce * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!mainEngineParticle.isPlaying)
        {
            mainEngineParticle.Play();
        }
    }
    void StopThrusting()
    {
        audioSource.Stop();
        mainEngineParticle.Stop();
    }

    void RotateLeft()
    {
        ApplyRotation(thrustRotation);
        if (!rightEngineParticle.isPlaying)
        {
            rightEngineParticle.Play();
        }
    }

    void RotateRight()
    {
        ApplyRotation(-thrustRotation);
        if (!leftEngineParticle.isPlaying)
        {
            leftEngineParticle.Play();
        }
    }

    void StopRotation()
    {
        rightEngineParticle.Stop();
        leftEngineParticle.Stop();
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        playerRb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        playerRb.freezeRotation = false;
    }
}
