using System;
using UnityEngine;
using Utils;

namespace KillTheFrogs
{
    public class AudioManager
    {
        private AudioLibrary _audioLibrary;
        private Pool<AudioPlayer> _audioPlayersPool;
        private AudioPlayer _audioPlayerPrefab;

        private bool _isInitialized;

        private const int DEFAULT_POOL_CAPACITY = 5;
        private const int MAX_SOUNDS_COUNT = 10;
        
        private static AudioManager _instance;

        private AudioPlayer _currentMusicAudioPlayer;
        
        public void init(AudioLibrary audioLibrary, AudioPlayer audioPlayerPrefab)
        {
            _isInitialized = true;
            _audioLibrary = audioLibrary;
            _audioLibrary.fillDictionary();
            _audioPlayerPrefab = audioPlayerPrefab;
            _audioPlayersPool = new AudioPlayersPool(_audioPlayerPrefab, MAX_SOUNDS_COUNT, DEFAULT_POOL_CAPACITY);
        }
        
        public void playSound(AudioClipKey audioClipKey, float volume)
        {
            checkInit();
            
            AudioClip audioClip = _audioLibrary.getAudioClip(audioClipKey);
            AudioPlayer audioPlayer = _audioPlayersPool.pool.Get();
            
            if (audioPlayer != null)
            {
                audioPlayer.playAudioClip(audioClip, volume);
                audioPlayer.audioClipFinished += () => _audioPlayersPool.pool.Release(audioPlayer);
            }
        }

        public void playMusic(AudioClipKey audioClipKey, float volume)
        {
            checkInit();
            AudioClip audioClip = _audioLibrary.getAudioClip(audioClipKey);
            
            if (_currentMusicAudioPlayer != null)
            {    
                if (_currentMusicAudioPlayer.audioSource.clip == audioClip)
                {
                    return;
                }
                _audioPlayersPool.pool.Release(_currentMusicAudioPlayer);
            }
            
            _currentMusicAudioPlayer = _audioPlayersPool.pool.Get();
            
            if (_currentMusicAudioPlayer != null)
            {
                _currentMusicAudioPlayer.playAudioClip(audioClip, volume, true);
            }
        }

        public void stopMusic()
        {
            if (_currentMusicAudioPlayer != null)
            {
                _audioPlayersPool.pool.Release(_currentMusicAudioPlayer);
                _currentMusicAudioPlayer = null;
            }
        }

        private void checkInit()
        {
            if (!_isInitialized)
            {
                throw new Exception("AudioManager hasn't been initialized");
            }
        }

        public void changeMusicVolume(float volumeValue)
        {
            if (_currentMusicAudioPlayer != null)
            {
                _currentMusicAudioPlayer.audioSource.volume = volumeValue;
            }
        }

        public static AudioManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AudioManager();
                }
                return _instance;
            }
        }
    }
}