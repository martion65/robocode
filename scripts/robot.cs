using Godot;
using System;
using System.ComponentModel.Design;

public partial class robot : RigidBody2D
{

    public Godot.Collections.Array<Node2D> containedRobots = [];
	public Marker2D nestSlot;
	public Marker2D nestSlot2;
	public Vector2 offset = new Vector2(15, 0);
	public int counter = 0;
	public Label counterDisplay;
	public LineEdit counterInput;
	public int desiredCounter;
    public void traitExecute()
	{

		if (IsInGroup("Print"))
		{

		}
		else if (IsInGroup("While")) 
		{

			while (counter < desiredCounter)
			{
				foreach (robot nestedRobot in containedRobots)
				{

					nestedRobot.traitExecute();

				}
			}
		}
		else if (IsInGroup("IfElse")) { }
		else if (IsInGroup("Add")) 
		{
			counter++;
		}


		
		



		
	}

	
	



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (IsInGroup("While"))
		{
			nestSlot = GetNode<Marker2D>("nestSlot");
			nestSlot2 = GetNode<Marker2D>("nestSlot2");
			
		}
		if (IsInGroup("Add")) 
		{
			counterInput = GetNode<LineEdit>("LineEdit");
			counterDisplay = GetNode<Label>("Label");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (counter > 0) 
		{
			counterDisplay.Text = counter.ToString();
		}


		if (IsInGroup("Held") || (IsInGroup("Nested")))
		{
			Freeze = true;
			SetCollisionLayerValue(4, false);
			SetCollisionLayerValue(3, false);
			SetCollisionMaskValue(1, false);
			SetCollisionLayerValue(1, false);

		}

		else if (!IsInGroup("Held"))
		{

			
			SetCollisionLayerValue(3, true);
			SetCollisionMaskValue(1, true);
			SetCollisionLayerValue(1, true);



			if (!IsInGroup("Submitted"))
			{

				Freeze = false;
			}
			else
			{
				Freeze = true;

			}
			if (!IsInGroup("While") && (!IsInGroup("Nested"))) 
			{
                SetCollisionLayerValue(4, true);
            }

		}




	}

	
	public void _on_area_2d_body_entered(Node2D insideRobot) 
	{
		if (!IsInGroup("Held"))
		{
			if (containedRobots.Count < 4)
			{
				if (insideRobot.IsInGroup("Pickable") && !containedRobots.Contains(insideRobot)) // making sure that the robot being nested hasn't already been nested before by checking if it has been made not pickable and if it already appears in the array.
				containedRobots.Add(insideRobot);
				insideRobot.RemoveFromGroup("Pickable");
				
				
				if (containedRobots.Count<= 2)
				{
                    insideRobot.CallDeferred(Node.MethodName.Reparent, nestSlot, false);

                    insideRobot.SetDeferred("position", Vector2.Zero + ((containedRobots.Count-1) * offset)) ;
					
					
				}
				else 
				{
                    insideRobot.CallDeferred(Node.MethodName.Reparent, nestSlot2, false);
                    insideRobot.SetDeferred("position", Vector2.Zero + ((containedRobots.Count-3) * offset)); 
				}
                insideRobot.AddToGroup("Nested");

            }
		}
	

	}

	public void _on_eject_button_pressed() 
	{
		foreach (robot nestedRobot in containedRobots) 
		{
			nestedRobot.Position += Vector2.Up * 50;
			nestedRobot.AddToGroup("Pickable");
			nestedRobot.Reparent(GetParent());
			nestedRobot.RemoveFromGroup("Nested");
			
		}
		containedRobots.Clear();
	}

	public void _on_line_edit_text_submitted() 
	{
		desiredCounter = Convert.ToInt32(counterInput.Text);
	}
}
