using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFXList : int
{
  Click = 0,
  Success = 1,
  Fail = 2,
  FailTwo = 3,
  Firework = 4,
}

public class SoundManager : MonoBehaviour
{
  public static SoundManager Instance = null;

  [SerializeField] public AudioClip[] bgmList;
  [SerializeField] public AudioClip[] sfxList;
  [SerializeField] public AudioClip[] sfxMorseList;
  [SerializeField] public AudioClip[] voiceOverListIntro;
  [SerializeField] public AudioClip[] voiceOverListSortOne;
  [SerializeField] public AudioClip[] voiceOverListSortTwo;
  [SerializeField] public AudioClip[] voiceOverListFinishSelect;
  [SerializeField] public AudioClip[] voiceOverListHunt;
  [SerializeField] public AudioClip[] voiceOverListAssembly;
  [SerializeField] public AudioClip[] voiceOverListCollectReaction;
  [SerializeField] public AudioClip[] voiceOverListFlyIntro;
  [SerializeField] public AudioClip[] voiceOverListEnd;
  [SerializeField] public AudioClip[] voiceOverListWingsOne;
  [SerializeField] public AudioClip[] voiceOverListWingsTwo;
  [SerializeField] public AudioClip[] voiceOverListFuelTank;
  [SerializeField] public AudioClip[] voiceOverListPropeller;
  [SerializeField] public AudioClip[] voiceOverListEngine;
  [SerializeField] private AudioSource _audioSource;
  [SerializeField] private AudioSource _bgmSource;
  [SerializeField] private AudioSource _fireworkSFX;

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

  private void Start()
  {
    if (GameManager.Instance.GetCurrentSceneIndex() == (int) SceneIndex.Landing)
    {
      PlayBGM();
    }
  }

  public void StopPlay()
  {
    _audioSource.Stop();
  }

  public void PlayBGM()
  {
    if (_bgmSource != null)
      _audioSource.PlayOneShot(bgmList[0]);
  }

  public void PlaySFXByIndex(SFXList sfxIndex, float volume = 1.0f)
  {
    if (sfxIndex == SFXList.Firework)
    {
      _fireworkSFX.clip = sfxList[(int) SFXList.Firework];
      _fireworkSFX.Play();
    }
    else
    {
      _audioSource.PlayOneShot(sfxList[(int) sfxIndex], volume);
    }
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
    switch (sceneIndex)
    {
      case (int) SceneIndex.Intro:
        _audioSource.PlayOneShot(voiceOverListIntro[index]);
        break;
      case (int) SceneIndex.SortOne:
        _audioSource.PlayOneShot(voiceOverListSortOne[index]);
        break;
      case (int) SceneIndex.SortTwo:
        _audioSource.PlayOneShot(voiceOverListSortTwo[index]);
        break;
      case (int) SceneIndex.FinishSelect:
        _audioSource.PlayOneShot(voiceOverListFinishSelect[index]);
        break;
      case (int) SceneIndex.Hunt:
        _audioSource.PlayOneShot(voiceOverListHunt[index]);
        break;
      case (int) SceneIndex.Assembly:
        _audioSource.PlayOneShot(voiceOverListAssembly[index]);
        break;
      case (int) SceneIndex.FlyIntro:
        _audioSource.PlayOneShot(voiceOverListFlyIntro[index]);
        break;
      case (int) SceneIndex.End:
        _audioSource.PlayOneShot(voiceOverListEnd[index]);
        break;
      case (int) SceneIndex.WingsOne:
        if (index < voiceOverListWingsOne.Length)
          _audioSource.PlayOneShot(voiceOverListWingsOne[index]);
        break;
      case (int) SceneIndex.WingsTwo:
        if (index < voiceOverListWingsTwo.Length)
          _audioSource.PlayOneShot(voiceOverListWingsTwo[index]);
        break;
      case (int) SceneIndex.FuelTank:
        if (index < voiceOverListFuelTank.Length)
          _audioSource.PlayOneShot(voiceOverListFuelTank[index]);
        break;
      case (int) SceneIndex.Propeller:
        if (index < voiceOverListPropeller.Length)
          _audioSource.PlayOneShot(voiceOverListPropeller[index]);
        break;
      case (int) SceneIndex.Engine:
        if (index < voiceOverListEngine.Length)
          _audioSource.PlayOneShot(voiceOverListEngine[index]);
        break;
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