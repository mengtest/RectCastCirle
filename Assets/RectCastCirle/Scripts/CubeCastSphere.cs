using UnityEngine;
using UnityEngine.UI;

public class CubeCastSphere : MonoBehaviour
{
    public Transform cube;
    public Transform sphere;
    public Button button;

    public Vector3 AA;
    public Vector3 CC;
    public Vector3 O;
    public float r;

	void Start ()
    {
        button.onClick.AddListener(CheckHit);
	}

    public void CheckHit()
    {

    }
}