﻿//using CodeMonkey.Utils;
//using Unity.Collections;
//using Unity.Jobs;
//using Unity.Mathematics;
//using Unity.Burst;
//using Unity.Entities;

//public class Pathfinding : ComponentSystem
//{
//    private const int MOVE_STRAIGHT_COST = 10;
//    private const int MOVE_DIAGONAL_COST = 14;
//    private const int MOVE_DEPTH_COST = 10;

//    protected override void OnUpdate()
//    {
//        Entities.ForEach((Entity entity, ref PathfindingParams pathfindingParams) =>
//        {
//            FindPathJob findPathJob = new FindPathJob
//            {
//                startPosition = pathfindingParams.StartPosition,
//                endPosition = pathfindingParams.EndPosition
//            };
//            findPathJob.Run();

//            // We only need to do pathfinding once, so lets remove the component once we're done!
//            PostUpdateCommands.RemoveComponent<PathfindingParams>(entity);
//        });
//    }


//    private struct FindPathJob : IJob
//    {
//        public int2 startPosition;
//        public int2 endPosition;
//        public int2 gridSize;

//        public void Execute()
//        {
//            NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(gridSize.x * gridSize.y, Allocator.Temp);

//            for (int x = 0; x < gridSize.x; x++) {
//                for (int y = 0; y < gridSize.y; y++) {
//                    PathNode pathNode = new PathNode
//                    {
//                        x = x,
//                        y = y,
//                        index = CalculateIndex(x, y, gridSize.x),
//                        gCost = int.MaxValue,
//                        hCost = CalculateDistanceCost(new int2(x, y), endPosition)
//                    };

//                    pathNode.CalculateFCost();

//                    pathNode.isWalkable = true;
//                    pathNode.cameFromNodeIndex = -1;

//                    pathNodeArray[pathNode.index] = pathNode;
//                }
//            }

//            NativeArray<int2> neighborOffsetArray = new NativeArray<int2>(8, Allocator.Temp);
//            neighborOffsetArray[0] = new int2(-1, 0); // Left
//            neighborOffsetArray[1] = new int2(+1, 0); // Right
//            neighborOffsetArray[2] = new int2(0, +1); // Up
//            neighborOffsetArray[3] = new int2(0, -1); // Down
//            neighborOffsetArray[4] = new int2(-1, -1); // Left Down
//            neighborOffsetArray[5] = new int2(-1, +1); // Left Up
//            neighborOffsetArray[6] = new int2(+1, -1); // Right Down
//            neighborOffsetArray[7] = new int2(+1, +1);  // Right Up
            

//            int endNodeIndex = CalculateIndex(endPosition.x, endPosition.y, gridSize.x);

//            PathNode startNode = pathNodeArray[CalculateIndex(startPosition.x, startPosition.y, gridSize.x)];
//            startNode.gCost = 0;
//            startNode.CalculateFCost();
//            pathNodeArray[startNode.index] = startNode;

//            NativeList<int> openList = new NativeList<int>(Allocator.Temp);
//            NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

//            openList.Add(startNode.index);

//            while (openList.Length > 0) {
//                int currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNodeArray);
//                PathNode currentNode = pathNodeArray[currentNodeIndex];

//                if (currentNodeIndex == endNodeIndex) {
//                    // Reached Destination
//                    break;
//                }

//                for (int i = 0; i < openList.Length; i++) {
//                    if (openList[i] == currentNodeIndex) {
//                        openList.RemoveAtSwapBack(i);
//                        break;
//                    }
//                }

//                closedList.Add(currentNodeIndex);

//                for (int i = 0; i < neighborOffsetArray.Length; i++) {
//                    int2 neighbourOffset = neighborOffsetArray[i];
//                    int2 neighborPosition = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);

//                    if (!IsPositionInsideGrid(neighborPosition, gridSize)) {
//                        // Neighbor is not inside the grid!
//                        continue;
//                    }

//                    int neighborNodeIndex = CalculateIndex(neighborPosition.x, neighborPosition.y, gridSize.x);

//                    if (closedList.Contains((neighborNodeIndex))) {
//                        // Already searched this node
//                        continue;
//                    }

//                    PathNode neighborNode = pathNodeArray[neighborNodeIndex];
//                    if (!neighborNode.isWalkable) {
//                        // Neighbor is not walkable
//                        continue;
//                    }

//                    int2 currentNodePosition = new int2(currentNode.x, currentNode.y);
//                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neighborPosition);
//                    if (tentativeGCost < neighborNode.gCost) {
//                        neighborNode.cameFromNodeIndex = currentNodeIndex;
//                        neighborNode.gCost = tentativeGCost;
//                        neighborNode.CalculateFCost();
//                        pathNodeArray[neighborNodeIndex] = neighborNode;

//                        if (!openList.Contains(neighborNode.index)) {
//                            openList.Add(neighborNode.index);
//                        }
//                    }
//                }
//            }

//            PathNode endNode = pathNodeArray[endNodeIndex];
//            if (endNode.cameFromNodeIndex == -1) {
//                // We did not find a path
//            } else {
//                // Found a path
//                NativeList<int2> path = CalculatePath(pathNodeArray, endNode);

//                path.Dispose();
//            }

//            pathNodeArray.Dispose();
//            openList.Dispose();
//            closedList.Dispose();
//            neighborOffsetArray.Dispose();
//        }
//    }
//    private static NativeList<int2> CalculatePath(NativeArray<PathNode> pathNodeArray, PathNode endNode)
//    {
//        if (endNode.cameFromNodeIndex == -1)
//        {
//            // Could not find a path
//            return new NativeList<int2>(Allocator.Temp);
//        }
//        else
//        {
//            // Found a path!
//            NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
//            path.Add(new int2(endNode.x, endNode.y));

//            PathNode currentNode = endNode;
//            while (currentNode.cameFromNodeIndex != -1) {
//                PathNode cameFromNode = pathNodeArray[currentNode.cameFromNodeIndex];
//                path.Add(new int2(cameFromNode.x, cameFromNode.y));
//                currentNode = cameFromNode;
//            }

//            return path;
//        }
//    }

//    private static bool IsPositionInsideGrid(int2 gridPosition, int2 gridSize)
//    {
//        return
//            gridPosition.x >= 0 &&
//            gridPosition.y >= 0 &&
//            gridPosition.x < gridSize.x &&
//            gridPosition.y < gridSize.y;

//    }

//    private static int CalculateIndex(int x, int y, int gridWidth)
//    {
//        return x + y * gridWidth;
//    }

//    private static int CalculateDistanceCost(int2 aPos, int2 bPos)
//    {
//        int xDistance = math.abs(aPos.x - bPos.x);
//        int yDistance = math.abs(aPos.y - bPos.y);
//        int remaining = math.abs(xDistance - yDistance);
//        return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
//    }

//    private static int GetLowestCostFNodeIndex(NativeList<int> openList, NativeArray<PathNode> pathNodeArray)
//    {
//        PathNode lowestCostPathNode = pathNodeArray[openList[0]];
//        for (int i = 0; i < openList.Length; i++)
//        {
//            PathNode testPathNode = pathNodeArray[openList[i]];
//            if (testPathNode.fCost < lowestCostPathNode.fCost)
//            {
//                lowestCostPathNode = testPathNode;
//            }
//        }

//        return lowestCostPathNode.index;
//    }

//    private struct PathNode
//    {
//        public int x;
//        public int y;

//        public int index;

//        public int gCost,
//            hCost,
//            fCost;

//        public bool isWalkable;

//        public int cameFromNodeIndex;

//        public void CalculateFCost()
//        {
//            fCost = gCost + hCost;
//        }
//    }
//}
