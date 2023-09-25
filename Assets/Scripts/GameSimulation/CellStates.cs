using System;

public enum CellStates
{
	None = 0,
	Bomb,
	Wall,
}

public class CellStatesHelper
{
	public static CellStates GetRandomCellStates()
	{
		var values = Enum.GetValues(typeof(CellStates));
		int index = UnityEngine.Random.Range(0, values.Length);
		return (CellStates)values.GetValue(index);
	}
}