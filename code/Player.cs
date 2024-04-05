using Sandbox;

public sealed class Player : Component
{
	public Brick brick;
	private GameObject last;
	public static Player Local => Game.ActiveScene.Components.GetAll<Player>( FindMode.EnabledInSelfAndDescendants ).ToList().FirstOrDefault( x => x.Network.OwnerConnection.SteamId == (ulong)Game.SteamId );
	protected override void OnUpdate()
	{
		if (IsProxy)
		{
			return;
		}
		CameraComponent Camera = Game.ActiveScene.Camera;
		CameraMove cameraMove = Camera.Components.Get<CameraMove>();
		if ( brick == null )
		{
			if ( Input.Down( "Jump" ) )
			{
				cameraMove.offset.z += 100f * Time.Delta;
			}
			if ( Input.Down( "Duck" ) )
			{
				cameraMove.offset.z -= 100f * Time.Delta;
			}
		}
		else
		{
			if ( Input.Down( "Jump" ) )
			{
				brick.Move( Vector3.Up * 50f * Time.Delta );
			}
			if ( Input.Down( "Duck" ) )
			{
				brick.Move( Vector3.Down * 50f * Time.Delta );
			}
		}
		SceneTraceResult tr = Scene.Trace.Ray(Camera.Transform.Position, Camera.Transform.Position + Camera.Transform.Rotation.Forward * 1000f).Run();
		if ( tr.GameObject != null && tr.GameObject.Components.Get<Brick>() != null && brick == null ) {
			if ( last != null )
			{
				last.Components.Get<ModelRenderer>().Tint = new Color( 1.00f, 0.94f, 0.38f, 1.00f );
			}

			tr.GameObject.Components.Get<ModelRenderer>().Tint = Color.Blue;
			last = tr.GameObject; 
		}
		if (tr.GameObject != null && Input.Pressed("use") && tr.GameObject.Components.Get<Brick>() != null && brick == null)
		{
			tr.GameObject.Components.Get<Brick>().Use(this);
			Log.Info( "Use" );
		}
		else if (Input.Pressed("use") && brick != null)
		{
			brick.Use(this);
			brick = null;
		}
	}
}
