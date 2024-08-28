using Scripts.Configs.Sounds;

namespace Scripts.Infrastructure.Providers.Configs
{
    public interface ISoundConfigProvider
    {
        VolumeConfig VolumeConfig { get; }
        AudioClipsListConfig AudioClipsListConfig { get; }
    }

    public class SoundConfigProvider : ISoundConfigProvider
    {
        public VolumeConfig VolumeConfig { get; }

        public AudioClipsListConfig AudioClipsListConfig { get; }

        public SoundConfigProvider(IGlobalConfigProvider globalConfigProvider)
        {
            VolumeConfig = globalConfigProvider.Config.VolumeConfig;
            AudioClipsListConfig = globalConfigProvider.Config.AudioClipsListConfig;
        }
    }
}