using System;
using VRC.SDKBase;

public class UdonTaskRandom
{
	public static UdonTaskRandom New(int seed)
	{
		var state = InitState(seed);
		return (UdonTaskRandom)(object)new object[] { seed, 0, state };
	}

	public static UdonTaskRandom New()
	{
		var seed = (int)(((DateTimeOffset)DateTime.SpecifyKind(Networking.GetNetworkDateTime(), DateTimeKind.Utc)).ToUnixTimeMilliseconds() & int.MaxValue);
		var state = InitState(seed);
		return (UdonTaskRandom)(object)new object[] { seed, 0, state };
	}

	private static uint[] InitState(int seed)
	{
		var state = new uint[4];
		for (int i = 0; i < 4; ++i)
		{
			state[i] = Convert.ToUInt32(Convert.ToInt64(seed) & 0xFFFFFFFF);
			seed = seed * 1812433253 + 1;
		}
		return state;
	}
}