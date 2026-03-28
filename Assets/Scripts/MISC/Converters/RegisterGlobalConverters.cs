using UnityEngine;
using UnityEngine.UIElements;

public static class RegisterGlobalConverters
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Register()
    {
        ConverterGroups.RegisterGlobalConverter((ref int intValue) => intValue.ToString());
    }
}
