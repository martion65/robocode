using Godot;
using System;
using System.Threading.Tasks;

public partial class door : RigidBody2D
{

	public bool winConditionMet = true;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{



	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		OpenDoor(winConditionMet);


	}

	public async Task OpenDoor(bool won) 
	{
        if (winConditionMet)
        {
            while (Rotation > -90)
			{


                
            }
        }

    }

}
