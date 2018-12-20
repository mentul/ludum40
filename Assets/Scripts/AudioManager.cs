using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public AudioSource music;
	public AudioClip[] ambientMusic;

	private int clipCount, currentClip;

	public void Start()
	{
		clipCount = ambientMusic.Length;
		currentClip = -1;
		PlayMusic();
	}
    
	public void PlayMusic()
	{
		music.Stop();
        
		if (++currentClip == clipCount)
			currentClip = 0;

		music.clip = ambientMusic [currentClip];

		music.Play();
	}
    
}
