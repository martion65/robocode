using Godot;
using System;

public partial class robot : RigidBody2D
{



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (IsInGroup("Held"))
		{
			Freeze = true;
			
			SetCollisionMaskValue(1, false);
			SetCollisionLayerValue(1, false);

		}
		else 
		{
			Freeze = false;

            SetCollisionMaskValue(1, true);
            SetCollisionLayerValue(1, true);
        }

		


	}
}
