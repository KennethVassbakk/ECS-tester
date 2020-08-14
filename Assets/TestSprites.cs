using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;
using Unity.Transforms;
using UnityEngine.UI;


public class TestSprites : MonoBehaviour
{
    public bool useDOTS;
    public GameObject SpriteNoDOTS;
    public int xWidth;
    public int yWidth;
    public int zDepth = 5;
    public Sprite sprite;
    public Transform parent;
    public int currentZ = 0;
    public Text zText;
    public Text countText;

    public Color[] colors = new[]
    {
        Color.black,
        Color.blue,
        Color.cyan,
        Color.gray,
        Color.green,
        Color.magenta,
        Color.red,
        Color.white,
        Color.yellow
    };

    private Material[] _mats;

    // ECS STUFF
    private EntityManager _entityManager;
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;

    public bool init = false;
    
    private void Start()
    {
        xWidth = TestingGrid.Instance.gridWidth;
        yWidth = TestingGrid.Instance.gridHeight;
        zDepth = TestingGrid.Instance.gridDepth;

        countText.text = "Entities: " + (xWidth * yWidth * zDepth);
        
        if (TestingGrid.Instance.pfGrid == null)
        {
            Debug.Log("World doesn't exist!");
            return;
        }

        _mats = new Material[colors.Length];

        for (var i = 0; i < colors.Length; i++)
        {
            var temp = new Material(_material);
            temp.color = colors[i];
            temp.enableInstancing = true;
            _mats[i] = temp;
        }
        
        // Lets create objects!
        if (useDOTS == false)
        {
            for (var x = 0; x < xWidth; x++)
            for (var y = 0; y < yWidth; y++)
            {
                var go = Instantiate(SpriteNoDOTS, parent, true);
                go.transform.position = new Vector3(x, y, 0);
            }
        }
        else
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            for (var x = 0; x < xWidth; x++)
            {
                for (var y = 0; y < yWidth; y++)
                {
                    for (var z = 0; z < zDepth; z++)
                    {
                        var entity = _entityManager.CreateEntity(
                            typeof(RenderMesh),
                            typeof(LocalToWorld),
                            typeof(Translation),
                            typeof(RenderBounds),
                            typeof(DisableRendering)
                        );

                    /*var entity = _entityManager.CreateEntity(   
                        typeof(RenderMe),
                        typeof(Translation),
                        typeof(DontRender)
                    );*/

                        var random = Mathf.FloorToInt(UnityEngine.Random.Range(0, colors.Length));

                        var mat = _material;
                        
                        _entityManager.SetSharedComponentData(entity, new RenderMesh()
                        {
                            mesh = _mesh,
                            material = _mats[random]
                        });

                        _entityManager.SetComponentData(entity, new Translation()
                        {
                            Value = new float3((float) x, (float) y, z)
                        });

                        _entityManager.SetComponentData(entity, new RenderBounds()
                        {
                            Value = _mesh.bounds.ToAABB()
                        });
                        
                       
                        TestingGrid.Instance.pfGrid.GetGridObject(x, y, z).SetEntity(entity);
//                        Debug.Log("Entity is: " + entity);
//                        Debug.Log("Grid is: " + TestingGrid.Instance.pfGrid.GetGridObject(x,y,z).IsWalkable());
                    }
                }

            }

        }

        DrawSprites(currentZ);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q) && currentZ >= 0 && currentZ < zDepth - 1)
        {
            RemoveDraw(currentZ);
            currentZ++;
            DrawSprites(currentZ);
        }
        
        if (Input.GetKeyUp(KeyCode.E) && currentZ > 0 && currentZ <= zDepth - 1)
        {
            RemoveDraw(currentZ);
            currentZ--;
            DrawSprites(currentZ);
        }
    }
    
    // TODO: Jobify these.
    public void DrawSprites(int z)
    {
        for (var x = 0; x < xWidth; x++)
        {
            for (var y = 0; y < yWidth; y++)
            {
                //_entityManager.RemoveComponent(TestingGrid.Instance.pfGrid.GetGridObject(x, y, z).GetEntity(), typeof(DisableRendering));
                _entityManager.RemoveComponent(TestingGrid.Instance.pfGrid.GetGridObject(x, y, z).GetEntity(),
                    typeof(DisableRendering));
                //TestingGrid.Instance.pfGrid.GetGridObject(x,y,currentZ).GetEntity()
            }
        }

        zText.text = "Current Z: " + (currentZ + 1) + "/" + zDepth;
    }

    public void RemoveDraw(int z)
    {
        for (int x = 0; x < xWidth; x++)
        {
            for (int y = 0; y < yWidth; y++)
            {
                //_entityManager.AddComponent(TestingGrid.Instance.pfGrid.GetGridObject(x, y, z).GetEntity(), typeof(DisableRendering));
                _entityManager.AddComponent(TestingGrid.Instance.pfGrid.GetGridObject(x, y, z).GetEntity(),
                    typeof(DisableRendering));
                //TestingGrid.Instance.pfGrid.GetGridObject(x,y,currentZ).GetEntity()
            }
        }
    }
}