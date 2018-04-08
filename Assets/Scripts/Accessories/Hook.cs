using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : Accessory
{ 
	void Start ()
    {
        fileManager = new FileManager();
        type = TypeOfAccessories.Grapple;
        base.Start();
        vehicleController.rb.mass += float.Parse(fileManager.LoadAccessoriesValue(1, (int)type + 1));
        //TODO
    }

    void FixedUpdate ()
    {
		accessoryPressed = false;
	}
}
