using Godot;
using System;

public partial class player : CharacterBody2D
{
	public const float Speed = 150.0f;
	public const float JumpVelocity = -200.0f;
	
	private bool isInRange = false;
	private Node2D targetObject;
	private CharacterBody2D heldObject;

	private Marker2D handPosition;
	
	public override void _Ready()
	{
		handPosition = GetNode<Marker2D>("handposition");
	}

	public override void _PhysicsProcess(double delta)
	{
		PickupObject();
		DropObject();
		
		Vector2 velocity = Velocity;
		

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
	
	private void PickupObject()
	{
		if (isInRange && Input.IsActionJustPressed("pickup") && heldObject == null)
		{
			heldObject = (CharacterBody2D)targetObject;
			
			heldObject.Reparent(handPosition);
			heldObject.Position = Vector2.Zero; 
		}
	}

	private void DropObject()
	{
		if (heldObject == null) return;

		if (Input.IsActionJustPressed("drop right"))
		{
			PerformDrop(Vector2.Right * 10);
		}
		else if (Input.IsActionJustPressed("drop left"))
		{
			PerformDrop(Vector2.Left * 10);
		}
	}

	private void PerformDrop(Vector2 offset)
	{
		heldObject.Reparent(GetParent());
		heldObject.Position = Position + offset;
		heldObject = null;
	}

   
	private void _on_area_2d_body_entered(Node2D body)
	{
		if (body is Pickable)
		{
			isInRange = true;
			targetObject = body;
		}
	}

	private void _on_area_2d_body_exited(Node2D body)
	{
		if (body is Pickable)
		{
			isInRange = false;
			targetObject = null;
		}
	}
}
