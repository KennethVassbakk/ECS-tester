
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class TestinECSDraw : MonoBehaviour
{
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material material;

    private EntityManager _entityManager;

    private void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entity entity = _entityManager.CreateEntity(
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(Translation),
            typeof(RenderBounds)
        );

        _entityManager.SetSharedComponentData(entity, new RenderMesh
        {
            mesh = _mesh,
            material = material,
        });

        _entityManager.SetComponentData(entity, new Translation()
        {
            Value = new float3(1f, 1f, 1f)
        });
    }
}
