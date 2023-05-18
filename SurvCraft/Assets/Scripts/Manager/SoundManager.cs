using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // �̱����� ��ü�� �����ɶ� �ѹ��� ���� �Ǿ����
    static public SoundManager soundManager;

    public AudioSource[] audioSourcesEffects;
    public AudioSource audioSourceBGM;

    public string[] playSoundName;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    #region �̱���
    // ��ü ������ ���� ����
    private void Awake() 
    {
        if(soundManager == null)
        {
            soundManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // ������ �ִ����� ���� ���� ����� �� �ٷ� �ı�
            Destroy(gameObject);
        }
    }
    #endregion

    /* ��ü Ȱ��ȭ�� �Ź� ���� / �ڷ�ƾ ���� �Ұ� 
    private void OnEnable()
    {
        
    }

    ��ü Ȱ��ȭ�� �Ź� ����
    private void Start()
    {
        
    }*/

    private void Start()
    {
        playSoundName = new string[audioSourcesEffects.Length];
    }

    /// <summary>
    /// effect ȿ���� ��� 
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
                Debug.Log("���  AudioSource�� ������Դϴ�.");
                return;
            }
        }
            Debug.Log(name + "�� ��ϵ� ���������� �ƴմϴ�.");
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
        Debug.Log("��� ���� " + name + " ���尡 �����ϴ�.");
    }

}

[System.Serializable]
public class Sound
{
    public string name; // ���� Ŭ���� �̸�
    public AudioClip clip; // ���� Ŭ��
}
