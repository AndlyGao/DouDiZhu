using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAudio : AudioBase
{
    private AudioSource audioSource;
    private void Awake()
    {
        Bind(AudioEvent.EFFECTAUDIO);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case AudioEvent.EFFECTAUDIO:
                PlayAudio((int)message);
                break;
            default:
                break;
        }
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void Start()
    {
        AudioSource[] ass = GetComponentsInChildren<AudioSource>(true);
        for (int i = 0; i < ass.Length; i++)
        {
            Destroy(ass[i]);
        }
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    private void PlayAudio(int audioId)
    {
        AudioClip audio = Resources.Load<AudioClip>("Sound/Chat/Chat_" + audioId);
        audioSource.PlayOneShot(audio);
    }
}
