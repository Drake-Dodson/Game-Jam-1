using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    //public AudioSource MKEMelody;
    //public AudioSource MKEBass;
    //public AudioSource MKEPercussion;
    //public AudioSource MKELead;

    //public AudioSource MKEPercussion2;
    //public AudioSource MKEBass2;
    //public AudioSource MKEPhazer1;
    //public AudioSource MKEPhazer2;
    //public AudioSource MKEPhazer3;

    public static MusicPlayer instance;

    [SerializeField] private AudioSource[] sources;

    [SerializeField] private List<Song> songs = new List<Song>();
    private int currentSongIndex = 0;
    private Song currentSong;
    private int beatOffset = 0;

    private List<Song> activeSongs = new List<Song>();

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < songs.Count; i++)
        {
            if (songs[i].difficultyNumber == Data.difficulty)
                activeSongs.Add(songs[i]);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //PlaySong(0);
    }

    private void PlaySong(int index)
    {
        StopAll();

        if(index < 0 || index >= activeSongs.Count)
        {
            ClearSong();
            return;
        }

        currentSongIndex = index;
        Song song = activeSongs[index];
        currentSong = song;

        SetSong(song);

        PlayAll();
    }

    private void PlayNextSong()
    {
        if (currentSongIndex + 1 < activeSongs.Count)
            PlaySong(currentSongIndex + 1 % activeSongs.Count);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVolumes(GameManager.instance.currentLevel);

        if (GameManager.instance.currentBeat - beatOffset == currentSong.beatCount)
        {
            beatOffset += currentSong.beatCount;
            StopAll();
            PlayNextSong();
        }
    }

    public void Restart()
    {
        StopAll();
        PlaySong(0);
    }

    private void StopAll()
    {
        foreach(AudioSource source in sources)
        {
            source.Stop();
        }
    }

    private void PlayAll()
    {
        foreach (AudioSource source in sources)
        {
            if(source.clip != null)
            {
                source.Play();
            }
        }
    }

    private void SetSong(Song song)
    {
        currentSong = song;
        for (int i = 0; i < sources.Length; i++)
        {
            if (i < song.parts.Count)
            {
                sources[i].clip = song.parts[i].audioClip;
                sources[i].volume = song.parts[i].volumes[0];
            }
            else
            {
                sources[i].clip = null;
                sources[i].volume = 0f;
            }
        }
    }

    private void UpdateVolumes(int level)
    {
        for (int i = 0; i < sources.Length; i++)
        {
            if (i < currentSong.parts.Count)
            {
                int l = Mathf.Min(level, currentSong.parts[i].volumes.Length - 1);

                sources[i].clip = currentSong.parts[i].audioClip;
                sources[i].volume = currentSong.parts[i].volumes[l];
            }
        }
    }

    private void ClearSong()
    {
        currentSongIndex = -1;
        SetSong(new Song());
    }
}

[System.Serializable]
public class Song
{
    public string songName;
    public int beatCount = 128;
    public int difficultyNumber = 1;
    public List<SongPart> parts = new List<SongPart>();
}

[System.Serializable]
public class SongPart
{
    public string partName;
    public AudioClip audioClip;
    public float[] volumes;
}