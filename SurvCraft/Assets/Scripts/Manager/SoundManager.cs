using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // 싱글톤은 객체가 생성될때 한번만 실행 되어야함
    static public SoundManager soundManager;

    public AudioSource[] audioSourcesEffects;
    public AudioSource audioSourceBGM;

    public string[] playSoundName;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    #region 싱글톤
    // 객체 생성시 최초 실행
    private void Awake() 
    {
        if(soundManager == null)
        {
            soundManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 기존에 있던것은 남고 새로 생기는 건 바로 파괴
            Destroy(gameObject);
        }
    }
    #endregion

    /* 객체 활성화시 매번 실행 / 코루틴 실행 불가 
    private void OnEnable()
    {
        
    }

    객체 활성화시 매번 실행
    private void Start()
    {
        
    }*/

    private void Start()
    {
        playSoundName = new string[audioSourcesEffects.Length];
    }

    /// <summary>
    /// effect 효과음 재생 
    /// </summary>
    public void PlaySE(string name)
    {
        for(int i = 0; i < effectSounds.Length; i++)
        {
            if (name == effectSounds[i].name)
            {
                for(int j = 0; j < audioSourcesEffects.Length; j++)
                {
                    if (!audioSourcesEffects[j].isPlaying)
                    {
                        playSoundName[j] = effectSounds[i].name;
                        audioSourcesEffects[j].clip = effectSounds[i].clip;
                        audioSourcesEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든  AudioSource가 사용중입니다.");
                return;
            }
        }
            Debug.Log(name + "는 등록된 음원파일이 아닙니다.");
    }

    public void PlayBGM(string name)
    {
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            if (name == bgmSounds[i].name)
            {
                audioSourceBGM.clip = bgmSounds[i].clip;
                audioSourceBGM.Play();
                return;
            }
        }
       
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourcesEffects.Length; i++)
        {
            audioSourcesEffects[i].Stop();
        }
    }

    public void StopSE(string name)
    {
        for (int i = 0; i < audioSourcesEffects.Length; i++)
        {
            if (playSoundName[i] == name)
            {
                audioSourcesEffects[i].Stop();
                return;
            }
        }
        Debug.Log("재생 중인 " + name + " 사운드가 없습니다.");
    }

}

[System.Serializable]
public class Sound
{
    public string name; // 사운드 클립의 이름
    public AudioClip clip; // 사운드 클립
}
