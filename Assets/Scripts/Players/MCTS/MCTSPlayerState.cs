public class MCTSPlayerState
{
	MCTSPlayerState(ref MCTSPlayerState parent, MCTSPlayerAction action)
	{
		this.parent = parent;
		this.currentAction = action;
	}
	MCTSPlayerState(MCTSPlayerAction action)
	{
		this.parent = null;
		this.currentAction = action;
	}
	
	private MCTSPlayerState parent = null;
	private MCTSPlayerAction currentAction;

	public bool IsRoot() => parent == null;
}
