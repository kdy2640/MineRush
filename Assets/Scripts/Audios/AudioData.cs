using System;
using UnityEngine;


[Serializable]
public class BGMClipData
{
    public BGMType type;//어떤 BGM인지 구분하는 열거형
    public AudioClip clip;//실제 재생할 오디오 파일

    [Range(0.0f, 1.0f)]
    public float volume = 1.0f;
}
[Serializable]
public class SFXClipData
{
    public SFXType type;
    public AudioClip clip;
    [Range(0.0f, 1.0f)]
    public float volume = 1.0f;
    [Range(-3.0f, 3.0f)]
    public float pitch = 1.0f;
}