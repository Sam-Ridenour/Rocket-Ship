using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delay = 1f;
    [SerializeField] AudioClip levelComplete;
    [SerializeField] AudioClip crashing;

    [SerializeField] ParticleSystem levelCompleteParticles;
    [SerializeField] ParticleSystem crashingParticles;

    AudioSource audioSource;

    bool isTransitioning = false;
    bool isColliding = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        DebugKeys();
    }

    void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            isColliding = !isColliding; // toggle collision
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || isColliding)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("That was a friendly");
                break;
            case "Finish":
                StartNextLevel();                
                break;
            default:
                StartCrashSequence();                
                break;
        }
    }
    void StartNextLevel()
    {       
            isTransitioning = true;
            audioSource.Stop();
            audioSource.PlayOneShot(levelComplete);
            levelCompleteParticles.Play();
            GetComponent<Movement>().enabled = false;            
            Invoke("LoadNextLevel", delay);          
    }
    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crashing);
        crashingParticles.Play();
        GetComponent<Movement>().enabled = false;

        Invoke("ReloadLevel", delay);
    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

}
