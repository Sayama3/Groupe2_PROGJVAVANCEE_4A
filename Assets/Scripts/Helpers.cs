using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

public static class Helpers
{
	public static T GetRandomEnum<T>() where T : Enum
	{
		var values = Enum.GetValues(typeof(T));
		int index = UnityEngine.Random.Range(0, values.Length);
		return (T)values.GetValue(index);
	}

	public static T GetRandom<T>(this IList<T> list)
	{
        Assert.IsNotNull(list);
		int index = UnityEngine.Random.Range(0, list.Count);
		return list[index];
	}

	public static T GetRandom<T>(this IEnumerable<T> list)
	{
        Assert.IsNotNull(list);
		int index = UnityEngine.Random.Range(0, list.Count());
		return list.ElementAt(index);
	}
}