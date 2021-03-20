using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    public class PoolManager : TickBehaviour
    {
        private readonly List<IResource> _pooledResources = new List<IResource>();
        private readonly List<IResource> _wannabePooledResources = new List<IResource>();

        public override void Init()
        {
            base.Init();

            priority = TickPriority.High;
        }

        public override void Tick()
        {
            base.Tick();

            UpdatePooledResources();
        }

        public void Despawn(IResource entity)
        {
            _wannabePooledResources.Add(entity);
        }

        public IResource Spawn(IResource entity, Transform parent, Vector3 position, Quaternion rotation)
        {
            var resource = PullEntity(entity, parent);

            resource.GameObject.transform.position = position;
            resource.GameObject.transform.rotation = rotation;

            return resource;
        }

        private void UpdatePooledResources()
        {
            for (var i = _wannabePooledResources.Count - 1; i >= 0; i--)
            {
                var entity = _wannabePooledResources[i];

                entity.GameObject.transform.SetParent(transform, false);
                entity.GameObject.transform.localPosition = transform.localPosition;
                entity.GameObject.transform.localRotation = transform.localRotation;
                entity.GameObject.SetActive(false);

                _wannabePooledResources.Remove(entity);
                _pooledResources.Add(entity);
            }
        }

        private IResource PullEntity(IResource entity, Transform parent)
        {
            IResource newEntity = default;
            for (var i = 0; i < _pooledResources.Count; i++)
            {
                if (_pooledResources[i].Type != entity.Type)
                    continue;

                newEntity = _pooledResources[i].GameObject.GetComponent<IResource>();
                break;
            }

            if (newEntity != null)
            {
                newEntity.GameObject.SetActive(true);
                newEntity.GameObject.transform.SetParent(parent, false);

                _pooledResources.Remove(newEntity);
            }
            else
            {
                newEntity = Instantiate(entity.GameObject, parent).GetComponent<IResource>();
                return newEntity;
            }

            return newEntity;
        }
    }
}
