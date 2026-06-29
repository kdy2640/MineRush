using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

//프로젝트 전체에서 하나만 존재하도록 관리된다.
public enum BGMType
{
    None,
    Titie,
    Upgrade,
    GameLoop
}
public enum SFXType
{
    None,
    ButtonClick,
    Upgrade,
    OreCollect,
    StoneHit,
    StoneCrush,
    MetalHit,
    FieldPlacement,
    LevelUp,
    UIHover,
    GameEnd,
    LoadingIn,
    LoadingOut,
    LaserBeam,
    Explosion,
    PickaxeEnhancing
}
public class AudioManager : MonoBehaviour
{
    [Header("AudioSource")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private int sfxSourceCount = 10;
    [SerializeField] private AudioSource[] sfxSources;
    private int currentSFXIndex = 0;

    [Header("BGM List")]
    [SerializeField] private BGMClipData[] bgmClips;//인스펙터에서 등록할 BGM
    [Header("SFX List")]
    [SerializeField] private SFXClipData[] sfxClips;//인스펙터에서 등록할 효과음 데이터


    //예 : BGMType.Stage -> Stage BGM데이터
    private Dictionary<BGMType, BGMClipData> bgmDictionary;
    //예 : SFXType.Jump -> Jump 효과음 데이터
    private Dictionary<SFXType, SFXClipData> sfxDictionary;

    private BGMClipData currentBGMData;
    private float masterVolume = 1.0f;
    private float bgmVolume = 1.0f;
    private float sfxVolume = 1.0f;
    protected void Awake()
    {  
        CreateAudioSources(); 
        InitializeDictionary();
    }
   //AudioSource가 없을경우 자동으로 만들어주는 녀석
   private void CreateAudioSources()
    {
        if(bgmSource==null)
        {
            //BGM source 라는 이름의 빈 게임오브젝트를 생성하자.
            GameObject bgmObj = new GameObject("BGM source");
            bgmObj.transform.SetParent(transform);

            //생성한 오브젝트에 AudioSource컴포넌트를 추가
            bgmSource = bgmObj.AddComponent<AudioSource>();

            //BGM은 반복재생하니까 루프를 true로 설정
            bgmSource.loop = true;
        }
        if (sfxSources == null || sfxSources.Length == 0)
        {
            sfxSources = new AudioSource[sfxSourceCount];

            for (int i = 0; i < sfxSourceCount; i++)
            {
                GameObject sfxObj = new GameObject($"SFX Source {i}");
                sfxObj.transform.SetParent(transform);

                AudioSource source = sfxObj.AddComponent<AudioSource>();
                source.loop = false;

                sfxSources[i] = source;
            }
        }
    }
    private AudioSource GetSFXSource()
    {
        // 먼저 비어있는 AudioSource를 찾는다.
        for (int i = 0; i < sfxSources.Length; i++)
        {
            if (!sfxSources[i].isPlaying)
                return sfxSources[i];
        }

        // 모두 재생 중이면 순환 사용
        AudioSource source = sfxSources[currentSFXIndex];

        currentSFXIndex++;

        if (currentSFXIndex >= sfxSources.Length)
            currentSFXIndex = 0;

        return source;
    }
    //배열로 등록한 오디 데이터를 딕셔너리에 저장하는 녀석
    private void InitializeDictionary()
    {
        bgmDictionary = new Dictionary<BGMType, BGMClipData>();
        sfxDictionary = new Dictionary<SFXType, SFXClipData>();

        for(int i = 0;i<bgmClips.Length;i++)
        {
            //배열 요소가 비어 있으면
            if (bgmClips[i] == null) continue;
            //BGM데이터 안에 AudioClip이 연결되어 있지 않으면
            if (bgmClips[i].clip == null) continue;

            //딕셔너리에 같은 BGMType이 아직 없으면
            if (!bgmDictionary.ContainsKey(bgmClips[i].type))
            {
                //BGMType을 key, BGMClipData를 Value로 저장
                bgmDictionary.Add(bgmClips[i].type, bgmClips[i]);
            }
        }
        for(int i = 0;i<sfxClips.Length;i++)
        {
            if (sfxClips[i] == null) continue;
            if (sfxClips[i].clip == null) continue;

            //딕셔너리에 같은 SFXType이 아직 없으면
            if (!sfxDictionary.ContainsKey(sfxClips[i].type))
            {
                sfxDictionary.Add(sfxClips[i].type, sfxClips[i]);
            }
        }
    }
    //BGM을 재생하는 녀석
    //AudioManager.Instance.PlayBGM(BGMType.Stage);
    public void PlayBGM(BGMType type)
    {
        //요청한 BGMType이 딕셔너리에 없으면
        if(!bgmDictionary.ContainsKey(type))
        {
            return;
        }
        //딕셔너리에서 해당 BGM데이터를 가져온다.
        BGMClipData data = bgmDictionary[type];

        //현재 재생중인 BGM과 요청한 BGM이 같다면
        if(bgmSource.clip==data.clip)
        {
            return;
        }
        //현재 재생중인 BGM데이터를 저장
        currentBGMData = data;
        //BGM AudioSource에 재생할 AudioClip을 넣는다.
        bgmSource.clip = data.clip;

        bgmSource.volume = data.volume * bgmVolume * masterVolume;
        //BGM을 재생한다.
        bgmSource.Play();
    }
    //BGM을 정지시키는 퓬
    public void StopBGM()
    {
        //현재 재쇼ㅐㅇ중인 BGM을 정지
        bgmSource.Stop();
        //오디오 소스에 연결된 오디오 클립을 제거
        bgmSource.clip = null;
        //현재 재생중인 BGM데이터도 비우자.
        currentBGMData = null;
    }
    //일시 정지
    public void PauseBGM()
    {
        bgmSource.Pause();
    }
    //일시정지된 BGM을 다시 재생
    public void ResumeBGM()
    {
        bgmSource.UnPause();
    }

    //효과음 랜덤으로 Randomratio (0~0.2) 추천
    public void PlaySFXRandomPitch(SFXType type, float randomRatio)
    {
        if (!sfxDictionary.ContainsKey(type))
            return;

        SFXClipData data = sfxDictionary[type];

        AudioSource source = GetSFXSource();

        float volume = data.volume * sfxVolume * masterVolume;

        source.pitch = data.pitch + Random.Range(-randomRatio, randomRatio);

        source.PlayOneShot(data.clip, volume);
    }
    //효과음 재생하는 녀석
    public void PlaySFX(SFXType type)
    {
        if (!sfxDictionary.ContainsKey(type)) return;

        SFXClipData data = sfxDictionary[type];

        AudioSource source = GetSFXSource();

        float volume = data.volume * sfxVolume * masterVolume;

        float pitch = data.pitch;

        source.pitch = pitch;
        source.PlayOneShot(data.clip, volume);
    }

    //전체 볼륨을 변경하는 녀석
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateBGMVolume();
    }
    //BGM볼륨을 변경하는 녀석
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        UpdateBGMVolume();
    }
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }
    //현재 재생중인 BGM의 볼륨을 계산
    private void UpdateBGMVolume()
    {
        //bgmSource가 없으면
        if (bgmSource == null) return;
        //현재 재생중인 BGM데이터가 없다면
        if(currentBGMData==null)
        {
            //기본 BGM볼륨과 마스터 볼륨만 적용
            bgmSource.volume = bgmVolume * masterVolume;
            return;
        }
        bgmSource.volume = currentBGMData.volume * bgmVolume * masterVolume;
    }
}
