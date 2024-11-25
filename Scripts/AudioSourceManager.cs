using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    public static AudioSourceManager Instance{get; private set;}
    public AudioSource soundAudio;//特效音
    public AudioSource musicAudio;//BGM
    public AudioSource dialogueAudio;//对话配音

    public void Awake()
    {
        Instance = this;
    }

    /*特效音播放*/
    public void PlaySound(string soundPath)
    {
        soundAudio.PlayOneShot(Resources.Load<AudioClip>("AudioClips/Sound/"+soundPath));
    }
    /*BGM播放*/
    public void PlayMusic(string musicPath,bool loop=true)
    {
        musicAudio.loop = loop;
        musicAudio.clip=Resources.Load<AudioClip>("AudioClips/Music/" + musicPath);
        musicAudio.Play();
    }
    /*BGM停播*/
    public void StopMusic()
    {
        musicAudio.Stop();
    }
    /*播放对话配音*/
    public void PlayDialogue(string dialoguePath)
    {
        dialogueAudio.Stop();
        dialogueAudio.PlayOneShot(Resources.Load<AudioClip>("AudioClips/Dialogue/" + dialoguePath));
    }
    /*配音停播*/
    public void StopDialogue()
    {
        dialogueAudio.Stop();
    }
}
