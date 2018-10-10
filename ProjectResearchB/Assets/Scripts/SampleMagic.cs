using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleMagic : Magic {
    [SerializeField]
    private MagicObject Ball;
    private int damage = 200;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Use()
    {
        var pos = Player.instance.transform.position + Player.instance.transform.forward * 2 + new Vector3(0, 1, 0);
        var rot = Player.instance.transform.rotation;
        var prefab = Instantiate(Ball, pos, rot);
        prefab.Init(damage);
        prefab.GetComponent<Rigidbody>().velocity = Player.instance.transform.forward;
    }
}
