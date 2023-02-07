using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    //all audioClips in Unity
    public AudioClip emailSound;
    public AudioClip dishesSound;
    public AudioClip trashSound;
    public AudioClip computerSound;
    public AudioClip hungrySound;
    public AudioClip backgroundMusic;
    public AudioClip laundrySound;
    //audio source in Unity
    static AudioSource audioSrc;

    private void Start()
    {
        //get the audiosource from the audioSrc object in Unity
        audioSrc = GetComponent<AudioSource>();
    }
    public void playEmail()
    {
        //play the email ping
        audioSrc.PlayOneShot(emailSound);
    }

    public void playDishes()
    {
        //play the dish sound
        audioSrc.PlayOneShot(dishesSound);
    }

    public void playTrash()
    {
        //play the trash sound
        audioSrc.PlayOneShot(trashSound);
    }

    public void playComputer()
    {
        //play the computer (typing) sounds
        audioSrc.PlayOneShot(computerSound);
    }

    public void playHungry()
    {
        //play the hungry stomach growl
        audioSrc.PlayOneShot(hungrySound);
    }

    public void playBackgroundMusic()
    {
        //play the background music
        audioSrc.PlayOneShot(backgroundMusic);
    }

    public void playLaundry()
    {
        //play the laundry sound
        audioSrc.PlayOneShot(laundrySound);
    }

    public void stopSound()
    {
        //stop any sound the audio source is currently making
        audioSrc.Stop();
    }
}
