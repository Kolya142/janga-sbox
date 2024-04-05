using Sandbox;

public sealed class GameC : Component
{
	public static GameC Gamec => Game.ActiveScene.Components.Get<GameC>( FindMode.EnabledInSelfAndDescendants );
	[Sync] public Player current_player { get; set; }
	[Sync] public int player_number { get; set; }

	protected override void OnAwake()
	{
		base.OnAwake();
	}

	protected override void OnUpdate()
	{
		if ( current_player == null )
		{
			if ( Game.ActiveScene.Components.GetAll<Player>( FindMode.EnabledInSelfAndDescendants ).Count() > 0)
			{

				Next_Player();
			}
		}
	}
	[Broadcast]
	public void Next_Player()
	{
		current_player = Game.ActiveScene.Components.GetAll<Player>( FindMode.EnabledInSelfAndDescendants ).ToList()[player_number];
		player_number++;
		player_number = player_number % Game.ActiveScene.Components.GetAll<Player>( FindMode.EnabledInSelfAndDescendants ).Count();
	}
}
