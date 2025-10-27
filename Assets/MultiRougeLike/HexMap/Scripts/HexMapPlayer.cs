using System.Collections;
using UnityEngine;

public class HexMapPlayer : MonoBehaviour
{
    public int x;
    public int z;
    public bool move = false;
    public (int x, int z) Position
    {
        get
        {
            return (this.x,this.z);
        }
        set
        {
            this.x = value.x;
            this.z = value.z;
        }
    }
    public void MoveTo((int i,int j) destination)
    {
        float innerr = 1.75f;
        float outerr = innerr / 0.866025404f;
        float x = destination.i * innerr;
        x = destination.j == 1 ? x : destination.j % 2 == 0 ? x + innerr / 2 : x;
        float z = destination.j * outerr;
        z = z - (outerr - innerr) * 1.9f * destination.j;
        move = true;
        gameObject.transform.position = new Vector3(x,0.5f,z);
        move = false;
    }
    
}
