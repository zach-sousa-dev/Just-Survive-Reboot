using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//LOTSA FUNCTIONS
public class Utilities : MonoBehaviour
{
    /**
     * remaps a number between two ranges
     *
     * @author EliWood, Zach Sousa
     * @version 1
     *
     * @param s the number to map
     * @param low1 the low range
     * @param high1 the high range
     * @param low2 the new low range
     * @param high2 the new high range
     * @return s remapped to the new range
     */
    public static float map(float s, float low1, float high1, float low2, float high2) {
        return (s - low1) * (high2 - low2) / (high1 - low1) + low2;
    }

    public static List<GameObject> FindChildrenWithTag(GameObject parent, string tag) {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in parent.transform) {
            if (child.tag == tag) {
                children.Add(child.gameObject);
            }
        }

        return children;
    }
}
