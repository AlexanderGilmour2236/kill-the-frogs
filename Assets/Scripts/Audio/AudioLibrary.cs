using System;
using System.Collections.Generic;
using UnityEngine;

namespace KillTheFrogs
{
    [CreateAssetMenu(fileName = "AudioLibrary", menuName = "Audio/AudioLibrary")]
    public class AudioLibrary : ScriptableObject
    {
        [SerializeField] private MappedAudioClip[] _audioClips;
        
        private Dictionary<AudioClipKey, AudioClip> _audioClipsMap;

        public void fillDictionary()
        {
            _audioClipsMap = new Dictionary<AudioClipKey, AudioClip>();
            foreach (MappedAudioClip mappedAudioClip in _audioClips)
            {
                _audioClipsMap[mappedAudioClip.key] = mappedAudioClip.audioClip;
            }
        }

        public AudioClip getAudioClip(AudioClipKey audioClipKey)
        {
            if (_audioClipsMap.ContainsKey(audioClipKey))
            {
                return _audioClipsMap[audioClipKey];
            }
            else
            {
                throw new Exception(@$"
There is no audio clip with the key {audioClipKey.ToString()}, you should add it to the audioLibrary file");
            }
        }
    }
}