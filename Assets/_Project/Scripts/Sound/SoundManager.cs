using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class SoundManager : MonoBehaviour
{
    [Space(10)]
    [Header("SoundButton")]
    public Sprite[] SFX;
    public Sprite[] BGM;
    public UnityEngine.UI.Button Btn_SFX;
    public UnityEngine.UI.Button Btn_BGM;
    public List<ObjectSound> listObject= new List<ObjectSound>();
    public static SoundManager instance;
   
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);    
        }
        AddAudioSourceForEachSound();
    }
    private void Start()
    {
        PlayBGM(TypeSFX.BGM, "BGM");
        //Btn_BGM.GetComponent<UnityEngine.UI.Image>().sprite = BGM[GameData.isBGM];
        //Btn_SFX.GetComponent<UnityEngine.UI.Image>().sprite = SFX[GameData.isSFX];
    }
    
    public void PlayBGM(TypeSFX _type, string _name)
    {
        if (GameData.isBGM == 1)
        {
            //Debug.Log("PLAY SFX");
            ObjectSound _objectSound= listObject.Find(x => (x.type == _type));
            if (_objectSound != null)
            {
                Sound _sound = _objectSound.track.Find(y => (y.name == _name));
                if (_sound != null && !_sound.source.isPlaying)
                {
                    _sound.source.Play();
                }
            }
        }
    }
    public void PlayRandom(TypeSFX _type)
    {
        if (GameData.isSFX == 1)
        {
            ObjectSound obj = listObject.Find(x => (x.type == _type));
            if (obj != null)
            {
                int randomIndex = Random.Range(0, obj.track.Count);
                obj.track[randomIndex].source.Play();
            }
        }
    }  
    public void PlaySFX(TypeSFX _type, string _name)
    {
        if (GameData.isSFX == 1)
        {
            ObjectSound _objectSound = listObject.Find(x => (x.type == _type));
            if (_objectSound != null)
            {
                Sound _sound = _objectSound.track.Find(y => (y.name == _name));
                if (_sound != null && !_sound.source.isPlaying)
                {
                    _sound.source.Play();
                }
            }
        }
    }
    public void playSequential(TypeSFX _type)
    {
        if (GameData.isSFX == 1)
        {
            ObjectSound _objectSound = listObject.Find(x => (x.type == _type));
            int _index = PlayerPrefs.GetInt(_type.ToString(), 0);
            //Debug.LogError(_index);
            if (_objectSound != null)
            {
                if (_index >= _objectSound.track.Count) PlayerPrefs.SetInt(_type.ToString(), _index = 0);
                Sound _sound = _objectSound.track[_index];
                if (_sound != null && !_sound.source.isPlaying)
                {
                    _sound.source.PlayOneShot(_sound.source.clip);
                    //Debug.LogError(_sound.name);
                    PlayerPrefs.SetInt(_type.ToString(), ++_index);
                }
            }
        }
    }
    public void ClearIndexSquential(TypeSFX _type)
    {
         PlayerPrefs.SetInt(_type.ToString(), 0);
    }
    public void Stop(TypeSFX _type, string _name)
    {
        //if (GameData.isSFX == 1)
        //{
            ObjectSound _objectSound= listObject.Find(x => (x.type == _type));
            Sound _sound = _objectSound.track.Find(y => (y.name == _name));
            if(_sound!=null)
                _sound.source.Stop();
        //}
    }
    public void MuteAll()
    {
        foreach (ObjectSound obj in listObject)
        {
            foreach (Sound s in obj.track)
            {
                s.source.mute=true;
            }
        }
    }  
    public void UnMuteAll()
    {
        foreach (ObjectSound obj in listObject)
        {
            foreach (Sound s in obj.track)
            {
                s.source.mute=false;
            }
        }
    }


    void AddAudioSourceForEachSound()
    {
        if (listObject != null)
        {

            foreach(ObjectSound obj in listObject)
            {
                foreach(Sound sound in obj.track)
                {
                    sound.source = gameObject.AddComponent<AudioSource>();
                    sound.source.clip = sound.clip;
                    sound.source.volume = sound.volume;
                    sound.source.pitch = sound.pitch;
                    sound.source.loop = sound.loop;
                }
            }
         }
     }



}

[System.Serializable]
public class ObjectSound
{
    public TypeSFX type;
    public List<Sound> track;

    public ObjectSound(TypeSFX _type)
    {
        type = _type;

    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0, 1)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}


public enum TypeSFX
{
    Mix,
    True,
    Wrong,
    Win,
    BGM,
    Lose,
    NUM_OF_TYPE
}


