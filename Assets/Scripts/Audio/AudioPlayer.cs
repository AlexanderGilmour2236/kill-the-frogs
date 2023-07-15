using System;
using UnityEngine;
using UnityEngine.Events;

namespace KillTheFrogs
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        
        public event Action audioClipFinished;
        
        private bool _hasPlayed;

        
        public void playAudioClip(AudioClip clip, float volume, bool loop = false)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
            _audioSource.loop = loop;
            _audioSource.volume = volume;
            _hasPlayed = true;
        }

        public void stopAudio()
        {
            _audioSource.Stop();
        }
        
        private void Update()
        {
            if (_hasPlayed && !_audioSource.isPlaying)
            {
                _hasPlayed = false;
                audioClipFinished?.Invoke();
            }
        }
        
        public AudioSource audioSource
        {
            get { return _audioSource; }
        }

        public void removeAllListeners()
        {
            audioClipFinished = null;
        }
    }
}