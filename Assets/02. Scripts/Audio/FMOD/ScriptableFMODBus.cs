using UnityEngine;

namespace Laresistance.Audio
{
    [CreateAssetMenu(menuName = "Laresistance/Audio/FMOD Bus")]
    public class ScriptableFMODBus : ScriptableObject
    {
        [SerializeField] [Tooltip("Format is ParentBus/ThisBus")]
        private string busPath = "ParentBus/ThisBus";

        private FMOD.Studio.Bus bus;

        private void Init()
        {
            string path;
            bus.getPath(out path);
            if (!string.IsNullOrEmpty(path))
                return;
            bus = FMODUnity.RuntimeManager.GetBus(string.Format("bus:/{0}", busPath));
            UnityEngine.Assertions.Assert.IsTrue(bus.isValid());
        }

        public void ChangeVolume(float volume)
        {
            UnityEngine.Assertions.Assert.IsTrue(volume >= 0f && volume <= 1f);
            Init();
            bus.setVolume(volume);
        }

        public float GetVolume()
        {
            Init();
            float volume;
            bus.getVolume(out volume);
            return volume;
        }

        public void SetMute(bool mute)
        {
            Init();
            bus.setMute(mute);
        }

        public void StopAllEvents()
        {
            Init();
            bus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}