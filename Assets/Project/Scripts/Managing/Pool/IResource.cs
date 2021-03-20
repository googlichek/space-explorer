using UnityEngine;

namespace Game.Scripts
{
    public interface IResource
    {
        GameObject GameObject { get; }
        ResourceType Type { get; }
        bool IsValid { get; }
    }
}
