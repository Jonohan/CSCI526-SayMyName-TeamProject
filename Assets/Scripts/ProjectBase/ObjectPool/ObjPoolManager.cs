using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A container (sub-pool) class for each type (name) of object stored in the object pool. 
/// </summary>
public class ObjSubpool
{
    /// <summary>
    ///  A root node that appears in the hierarchy for organizing disabled objects. Parent node: poolRootNode. 
    /// </summary>
    public GameObject inSceneContainer;
    /// <summary>
    ///  A list of the unused objects (one value for the pool dictionary)
    /// </summary>
    public List<GameObject> objList;
    
    /// <summary>
    /// Constructor of ObjSubpool. Creates a new sub-pool for all 'obj' type objects.
    /// </summary>
    /// <param name="obj">A game object that needs to be put into its sub-pool in the object pool.</param>
    /// <param name="poolRootNode">The root node in the hierarchy for organizing all disabled game objects in the pool.</param>
    public ObjSubpool(GameObject obj, GameObject poolRootNode)
    {
        // create a new game object container in hierarchy, as a child of poolRootNode.
        inSceneContainer = new GameObject(obj.name);
        inSceneContainer.transform.parent = poolRootNode.transform;
        // create a new List to store this type of object, and put the first object in
        objList = new List<GameObject>() { };
        PushObj(obj.name, obj);
    }
    
    /// <summary>
    /// Recycle unused game object and store it into its sub-pool in the object pool.
    /// </summary>
    /// <param name="name">Name (type) of the game object (and its container). E.g. bullet, ball, etc.</param>
    /// <param name="obj">The game object that needs to be recycled and stored.</param>
    public void PushObj(string name, GameObject obj)
    {
        // disable obj in scene and organize it under the parent object 
        obj.SetActive(false);
        obj.transform.parent = inSceneContainer.transform;
        // add it into the list
        objList.Add(obj);
    }
    
    /// <summary>
    /// Get a game object from its container and put it into the scene. 
    /// </summary>
    /// <returns>An instance of the game object.</returns>
    public GameObject GetObj()
    {
        GameObject obj = null;
        // get the first object in the list and remove it from the list
        obj = objList[0];
        objList.RemoveAt(0);
        
        // enable object in scene
        obj.SetActive(true);
        
        // remove obj's parent node since it is active
        obj.transform.parent = null;
        
        return obj;
    }
}

public class ObjPoolManager : BaseManager<ObjPoolManager>
{
    /// <summary>
    /// The container for temporarily unused game objects.
    /// Objects are sorted by name (key) and stored into sub-pools (value). 
    /// </summary>
    public Dictionary<string, ObjSubpool> objPoolDict = new Dictionary<string, ObjSubpool>();
    
    /// <summary>
    ///  A root node in scene for organizing temporarily disabled game objects.
    /// </summary>
    private GameObject poolRootNode;
    
    /// <summary>
    ///  Gets a game object from object pool. 
    /// </summary>
    /// <param name="name">Name (type) of the game object. E.g. bullet, ball, etc.</param>
    /// <returns>An instance of this game object.</returns>
    public GameObject GetObj(string name)
    {
        GameObject obj = null;
        // Case 1: this object type is recognized by pool, and there are unused object(s) available in the subpool
        if (objPoolDict.ContainsKey(name) && objPoolDict[name].objList.Count > 0)
        {   
            // get the object from the subpool
            obj = objPoolDict[name].GetObj();
        }
        // Case 2: there isn't a corresponding subpool, or there isn't any available object.
        else
        {
            // Simply instantiate a new one and return it
            obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
            obj.name = name;
        }
        
        return obj;
    }

    /// <summary>
    /// Return a game object to the storage pool for later use. 
    /// </summary>
    /// <param name="name">Name (type) of the game object. E.g. bullet, ball, etc. </param>
    /// <param name="obj">The object that needs to be stored.</param>
    public void PushObj(string name, GameObject obj)
    {
        // Organize the scene hierarchy; set the root node as parent
        if (poolRootNode == null)
            poolRootNode = new GameObject("poolRootNode");
        
        // Case 1: the pool already has a subpool containing all 'name' objects.
        if (objPoolDict.ContainsKey(name))
            objPoolDict[name].PushObj(name, obj);
        // Case 2: a list called 'name' doesn't exist yet
        else
        {
            // create the list first, and put the object in (action done in ObjSubpool constructor). 
            objPoolDict.Add(name, new ObjSubpool(obj, poolRootNode));
        }
    }
    
    /// <summary>
    /// Clear everything in current object pool and remove root node.
    /// Call this method when switching between scenes.
    /// </summary>
    public void ClearPool()
    {
        objPoolDict.Clear();
        poolRootNode = null;
    }
}
