using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip successSound;

    AudioSource myAudioSource;

    bool isTransitioning = false;

    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":  Debug.Log("That's ok"); break;
            case "Finish": Debug.Log("Nicely done!");
                StartFinishSequence();
                break;
            case "Obstacle": Debug.Log("Oh-oh...");
                StartCrashSequence();
                break;
            case "Ground": Debug.Log("You are not high enough :( ;)"); break;
            default: Debug.Log("What's this?"); break;
        }
    }

    private void ReloadScene()
    {
        LoadScene(GetCurrentSceneIndex());
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = GetCurrentSceneIndex();
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        LoadScene(nextSceneIndex);
    }

    private void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    private void StartCrashSequence()
    {
        isTransitioning = true;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(crashSound, 0.3f);
        GetComponent<Movement>().enabled = false;
        Invoke(nameof(ReloadScene), levelLoadDelay);
    }

    private void StartFinishSequence()
    {
        isTransitioning = true;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(successSound, 0.3f);
        GetComponent<Movement>().enabled = false;
        Invoke(nameof(LoadNextScene), levelLoadDelay);
    }
}
