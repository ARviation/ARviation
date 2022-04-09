using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFXList : int
{
  Click = 0,
  Success = 1,
  Fail = 2,
  FailTwo = 3,
}

public class SoundManager : MonoBehaviour
{
  public static SoundManager Instance = null;

  [SerializeField] public AudioClip[] sfxList;
  [SerializeField] public AudioClip[] sfxMorseList;
  [SerializeField] public AudioClip[] voiceOverListIntro;
  [SerializeField] public AudioClip[] voiceOverListHunt;
  [SerializeField] public AudioClip[] voiceOverListAssembly;
  [SerializeField] public AudioClip[] voiceOverListCollectReaction;
  [SerializeField] public AudioClip[] voiceOverListFly;
  [SerializeField] private AudioSource _audioSource;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
    }
    else
    {
      Instance = this;
    }
  }

  public void StopPlay()
  {
    _audioSource.Stop();
  }

  public void PlaySFXByIndex(SFXList sfxIndex, float volume = 1.0f)
  {
    _audioSource.PlayOneShot(sfxList[(int) sfxIndex], volume);
  }

  public void PlaySFXByMorseCode(MorseCode code, float volume = 1.0f)
  {
    _audioSource.PlayOneShot(sfxMorseList[(int) code], volume);
  }
  
  public void PlaySFX(AudioClip audioClip, float volume = 1.0f)
  {
    _audioSource.PlayOneShot(audioClip, volume);
  }

  public void PlayVoiceOver(int index)
  {
    _audioSource.Stop();
    int sceneIndex = GameManager.Instance.GetCurrentSceneIndex();
    if (sceneIndex == (int) SceneIndex.Intro)
    {
      _audioSource.PlayOneShot(voiceOverListIntro[index]);
    }
    else if (sceneIndex == (int) SceneIndex.Hunt)
    {
      _audioSource.PlayOneShot(voiceOverListHunt[index]);
    }
    else if (sceneIndex == (int) SceneIndex.Assembly)
    {
      _audioSource.PlayOneShot(voiceOverListAssembly[index]);
    }
    else if (sceneIndex == (int) SceneIndex.Fly)
    {
      _audioSource.PlayOneShot(voiceOverListFly[index]);
    }
  }

  public void PlayVoiceOverCollectReaction(int index)
  {
    _audioSource.Stop();
    _audioSource.PlayOneShot(voiceOverListCollectReaction[index]);
  }

  public void PlaySuccessSFX()
  {
    PlaySFXByIndex(SFXList.Success);
  }

  public void PlayFailSFX()
  {
    PlaySFXByIndex(SFXList.FailTwo);
  }
}