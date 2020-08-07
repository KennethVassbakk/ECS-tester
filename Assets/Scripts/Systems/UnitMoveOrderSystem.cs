using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;

public class UnitMoveOrderSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Entities.ForEach((Entity entity, ref Translation translation) =>
            {
                // Adding pathfinding params!
                EntityManager.AddComponentData(entity, new PathfindingParams
                {
                    StartPosition = new int2(0, 0),
                    EndPosition = new int2(10, 10)
                });
            });
        }
    }
}
