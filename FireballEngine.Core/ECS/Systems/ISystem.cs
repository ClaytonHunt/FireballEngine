using FireballEngine.Core.ECS.Core;

namespace FireballEngine.Core.ECS.Systems
{
    /// <summary>
    /// Interface for all systems in the ECS architecture.
    /// </summary>
    public interface ISystem
    {
        /// <summary>
        /// Whether this system is enabled and should be updated.
        /// </summary>
        bool Enabled { get; set; }
        
        /// <summary>
        /// The priority of this system. Systems with lower priority values are updated first.
        /// </summary>
        int Priority { get; set; }
        
        /// <summary>
        /// Initializes the system with the given scene.
        /// </summary>
        void Initialize(Scene scene);
        
        /// <summary>
        /// Updates the system.
        /// </summary>
        void Update(float deltaTime);
    }
}
