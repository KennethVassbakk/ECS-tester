using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Entities.Conversion;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;
using Unity.Transforms;
using UnityEngine.Rendering;


public class TestSprites : MonoBehaviour
{
    public bool useDOTS;
    public GameObject SpriteNoDOTS;
    public int xWidth;
    public int yWidth;
    public Sprite sprite;
    public Transform parent;
    

    // ECS STUFF
    private EntityManager _entityManager;
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;



    private void Start()
    {
        
    }

    public void Awake()
    {
        Debug.Log(useDOTS);
        // Lets create objects!
        if (useDOTS == false)
        {
            for (int x = 0; x < xWidth; x++)
            {
                for (int y = 0; y < yWidth; y++)
                {
                    GameObject go = Instantiate(SpriteNoDOTS);
                    go.transform.position = new Vector3(x, y, 0);
                    go.transform.parent = parent;
                }
            }

        } else {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            for (int x = 0; x < xWidth; x++)
            {
                for (int y = 0; y < yWidth; y++)
                {
                    Entity entity = _entityManager.CreateEntity(
                        typeof(RenderMesh),
                        typeof(LocalToWorld),
                        typeof(Translation),
                        typeof(RenderBounds)
                    );

                    _entityManager.SetSharedComponentData(entity, new RenderMesh {
                        mesh = _mesh,
                        material = _material,
                    });

                    _entityManager.SetComponentData(entity, new Translation() {
                        Value = new float3((float)x, (float)y, 0f)
                    });

                    _entityManager.SetComponentData(entity, new RenderBounds()
                    {
                        Value = _mesh.bounds.ToAABB(),
                    });
                }
            }

        }
    }
}
