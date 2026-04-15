using Godot;
using System;
using System.Threading.Tasks;
public partial class chute : Area2D
{
    [Signal]
    public delegate void DropFinishedEventHandler();
    
	public Marker2D dropLocation;
    public Godot.Collections.Array<Node2D> robotsToDrop = [];



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        dropLocation = GetNode<Marker2D>("dropLocation");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}


	public void _on_drop_pressed() 
	{
        robotsToDrop = GetOverlappingBodies();
        GD.Print(robotsToDrop);
        dropRobots();
       

	}
	public async Task dropRobots() //drop the robots every second
	{
        
        
        foreach (Node2D robot in robotsToDrop)
        {
            robot.RemoveFromGroup("Held");
            await ToSignal(GetTree().CreateTimer(1f), SceneTreeTimer.SignalName.Timeout);

        }
    }



    public void on_body_exited(Node2D robot) 
	{

		robotsToDrop.Remove(robot);

	}


}
