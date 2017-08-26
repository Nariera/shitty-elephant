using System;
using System.Collections.Generic;

//Basically a dumb achievemtn system
public sealed class Achievement
{
	//singleton event class ...uh
	public static Achievement instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new Achievement();
			}
			return _instance;
		}
	}
	private static Achievement _instance;

	private Dictionary<string, Action> achievementTable;
	private Dictionary<string, bool> completionTable;

	//should never be created by anybody else
	private Achievement()
	{
        achievementTable = new Dictionary<string, Action>();
        completionTable = new Dictionary<string, bool>();
	}

	//register a name and a action
	public bool Register(string name, Action evt)
	{
		if (instance.achievementTable.ContainsKey(name) || instance.completionTable.ContainsKey(name))
		{
			return false;
		}
		instance.achievementTable.Add(name, evt);
		instance.completionTable.Add(name, false);
		return true;
	}

	//return false if doesn't exist or is triggered already
	public bool Trigger(string name)
	{
		if (!instance.achievementTable.ContainsKey(name) || !instance.completionTable.ContainsKey(name) || instance.completionTable[name])
		{
			return false;
		}
		instance.achievementTable[name].Invoke();//call it
		instance.completionTable[name] = true;
		return true;
	}

	//probably should throw an error or a flag or something...I dunno. I'm super tired
	public bool IsTriggered(string name)
	{
		if (!instance.achievementTable.ContainsKey(name) || !instance.completionTable.ContainsKey(name) || instance.completionTable[name])
		{
			return false;
		}
		return instance.completionTable[name];
	}
}
