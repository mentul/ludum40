using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public AudioSource fx;
	public AudioSource music;
	public AudioClip throwClip;
	public AudioClip dieClip;
	public AudioClip drawClip;
	public AudioClip[] ambientMusic;

	private int clipCount, currentClip;

	public void Start ()
	{
		clipCount = ambientMusic.Length;
		currentClip = -1;
		PlayMusic ();
	}

	public void Update ()
	{
		if (Input.GetKeyDown (KeyCode.N))
			PlayMusic ();
	}

	public void PlayMusic ()
	{
		music.Stop ();

		currentClip++;
		if (currentClip == clipCount)
			currentClip = 0;

		music.clip = ambientMusic [currentClip];

		music.Play ();
	}

	public void ThrowDzida ()
	{
		fx.PlayOneShot (throwClip);
	}

	public void PlayerDie ()
	{
		fx.PlayOneShot (dieClip);
	}

	public void DrawOnStone ()
	{
		fx.PlayOneShot (drawClip);
	}

	public void NextClip ()
	{

	}


}
