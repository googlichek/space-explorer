namespace Game.Scripts
{
    /// <summary>
    /// Enum for assigning update priority.
    /// </summary>
    public enum TickPriority
    {
        None = 0,
        Low = 1,
        Normal = 2,
        High = 3
    }

    /// <summary>
    /// Interface for sub-components of the game object.
    /// Should be handled from the game object with <see cref="ITick"/> interface.>
    /// </summary>
    public interface ITickComponent
    {
        /// <summary>
        /// Unique ID for current game session.
        /// </summary>
        int Id { get; }

        void SetId(int tickId);

        /// <summary>
        /// Init component here.
        /// </summary>
        void Init();

        /// <summary>
        /// Subscribe to events here.
        /// </summary>
        void Enable();

        /// <summary>
        /// Handle fixed update code here.
        /// </summary>
        void PhysicsTick();

        /// <summary>
        /// Handle update code here.
        /// </summary>
        void Tick();

        /// <summary>
        /// Handle late update code here.
        /// </summary>
        void CameraTick();

        /// <summary>
        /// Unsubscribe from events here.
        /// </summary>
        void Disable();

        /// <summary>
        /// Destroy component here here.
        /// </summary>
        void Dispose();
    }

    /// <summary>
    /// Interface for main component of the game object.
    /// </summary>
    public interface ITick : ITickComponent
    {
        /// <summary>
        /// Update priority.
        /// </summary>
        TickPriority Priority { get; }
    }
}
