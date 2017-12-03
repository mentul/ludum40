using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMammoth : Animal {

	public override void DoInit ()
	{
		base.DoInit ();
        HP = 3;
        speed = 1.5f;
	}

	public override void DoUpdate ()
	{
		base.DoUpdate ();
	}
}
