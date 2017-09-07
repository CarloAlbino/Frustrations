using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    Leaf,
    Flower
}

public class PlantNode : MonoBehaviour {
    public NodeType nodeType { get; set; }
    public List<PlantNode> children = new List<PlantNode>();
    public PlantNode parent = null;

    public List<PlantNode> GetLeafs(PlantNode node)
    {
        List<PlantNode> result = new List<PlantNode>();

        foreach(PlantNode pn in children)
        {
            if(pn != null)
            {
                result.AddRange(pn.GetLeafs(pn));
            }
        }

        if(result.Count == 0)
        {
            if(nodeType == NodeType.Leaf)
                result.Add(this);
        }

        return result;
    }
}
