using Sirenix.OdinInspector;

namespace Scripts.Configs.Core
{
    public abstract class ScriptableConfig : UnityEngine.ScriptableObject
    {
        [TitleGroup("Info")]
        [BoxGroup("Info/In", showLabel: false)]
        public string FileName = "Config";

#if UNITY_EDITOR
        public virtual string GetConfigCategory() =>
            ConfigsCategories.NO_CATEGORY;
#endif
    }
}