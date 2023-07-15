
using Utils;

namespace KillTheFrogs
{
    public class AudioPlayersPool : Pool<AudioPlayer>
    {
        public AudioPlayersPool(AudioPlayer prefab, int maxPoolSize, int defaultCapacity, bool collectionChecks = true) : base(prefab, maxPoolSize, defaultCapacity, collectionChecks)
        {
        }

        protected override void onTakeFromPool(AudioPlayer pooled)
        {
            pooled.gameObject.SetActive(true);
        }

        protected override void onReturnedToPool(AudioPlayer pooled)
        {
            pooled.stopAudio();
            pooled.removeAllListeners();
            pooled.gameObject.SetActive(false);
        }
    }
}