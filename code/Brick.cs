using Sandbox;

public sealed class Brick : Component
{
	Player player;
	bool IsUse;
	protected override void OnUpdate()
	{
		if (!IsUse)
		{
			return;
		}
		if (true)
		{
			Vector3 move = Input.AnalogMove * Transform.Rotation * Time.Delta * 50;
			Move( move );
			if ( Input.Pressed( "reload" ) )
			{
				var angles = new Angles( 0, -90, 0 );
				Rotate( Rotation.From(angles) );
			}
		}
	}
	[Broadcast]
	public void Move(Vector3 movement)
	{
		Transform.Position += movement;
	}
	[Broadcast]
	public void Rotate( Rotation rotation )
	{
		Transform.Rotation *= rotation;
	}
	public SceneTraceResult Trace( Vector3 start, Vector3 end )
	{
		return Scene.Trace.Ray( start, end )
			.UsePhysicsWorld()
			.Radius( 2f )
			.IgnoreGameObject( this.GameObject )
			.Run();
	}
	public void Use(Player player)
	{
		if (IsUse)
		{
			if ( player == this.player )
			{
				this.player = null;
				this.Components.Get<Rigidbody>().MotionEnabled = true;
				IsUse = false;
			}
			return;
		}
		if (!Trace(Transform.Position, Transform.Position + Vector3.Up * 100).Hit)
		{
			return;
		}
		this.Components.Get<Rigidbody>().MotionEnabled = false;
		this.player = player;
		IsUse = true;
		player.brick = this;
	}
}
