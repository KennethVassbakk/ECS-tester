using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Entities.Conversion;
using Unity.Collections;
using UnityEngine;
using Unity.Transforms;


public class TestSprites : MonoBehaviour
{
    public bool useDOTS;
    public GameObject SpriteNoDOTS;
    public GameObject SpriteDots;
    public int xWidth;
    public int yWidth;
    public Sprite sprite;
    public Transform parent;
    public Material material;
    private Entity go;
    private EntityManager manager;

    private BlobAssetStore blobAssetStore;

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
            manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            Debug.Log("This should not be firing if not dots");

            GameObjectConversionSettings settings =
                GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);

            for (int x = 0; x < xWidth; x++) {
                for (int y = 0; y < yWidth; y++) {
                    GameObject go = Instantiate(SpriteDots);
                    go.transform.position = new Vector3(x, y, 0);
                    go.transform.parent = parent;
                    //GameObjectConversionUtility.ConvertGameObjectHierarchy(SpriteNoDOTS, settings);

                }
            }
        }
    }
}
