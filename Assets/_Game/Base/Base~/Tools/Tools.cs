using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using System.IO;
using UnityEngine;

public static class Tools {

	static Dictionary<string, Type> mAssemblies = new Dictionary<string, Type>();

	static Tools() { }

	public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class/*, IComparable<T>*/ {
		List<T> objects = new List<T>();
		foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(t => t.GetTypes()).Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)))) {
			if (!mAssemblies.ContainsKey(type.Name)) {
				mAssemblies.Add(type.Name, type);
			}
			objects.Add((T)Activator.CreateInstance(type, constructorArgs));
		}

		return objects;
	}

	public static Type GetEnumerableOfType(string className) {
		foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(t => t.GetTypes()).Where(myType => myType.IsClass && !myType.IsAbstract && myType.Name == className)) {
			if (!mAssemblies.ContainsKey(className)) {
				mAssemblies.Add(type.Name, type);
				return type;
			}
		}

		return null;
	}

	public static object GetInstance(string className) {
		if (!mAssemblies.TryGetValue(className, out Type t)) {
			t = GetEnumerableOfType(className);
		}
		return Activator.CreateInstance(t);
	}

	public static bool TimerReady(DateTime timeStarted, TimeSpan timeToWait) {
		TimeSpan timeSinceStarted = DateTime.UtcNow.Subtract(timeStarted);

		if (timeToWait.TotalMilliseconds - timeSinceStarted.TotalMilliseconds <= 0)
			return true;

		return false;
	}

    public static void WriteToFile(string file, string text, bool writeIfFileExists) {
        string path = file;
        StreamWriter writer;
        if (!File.Exists(path)) {
            writer = File.CreateText(path);
        }
        else {
            if (!writeIfFileExists)
                return;

            writer = new StreamWriter(path, true);
        }

        writer.WriteLine(text);
        writer.Close();
    }

    public static void StackDeleteMid<T>(Stack<T> st, T n) {
        // If stack is empty or all  
        // items are traversed 
        if (st.Count == 0)
            return;

        // Remove current item 
        T x = st.Peek();
        st.Pop();

        // Remove other items 
        StackDeleteMid(st, n);

        // Put all items  
        // back except middle 
        if (!x.Equals(n))
            st.Push(x);
    }

	public static string FormatMilliSecondsToString(double milliseconds, int precision = 2) {
		string num = "";

		int precise = 0;
		double time = milliseconds;

		double days = Math.Floor((time / (1000 * 60 * 60 * 24)));
		if (days > 0) {
			time -= days * 1000 * 60 * 60 * 24;
			if (precise > 0)
				num += " ";
			num += days + " day";
			if (days > 1)
				num += "s";

			precise++;
		}
		if (precise >= precision)
			return num;

		double hours = Math.Floor(time / (1000 * 60 * 60));
		if (hours > 0) {
			time -= hours * 1000 * 60 * 60;
			if (precise > 0)
				num += " ";
			num += hours + " hr";
			if (hours > 1)
				num += "s";

			precise++;
		}
		if (precise >= precision)
			return num;

		double minutes = Math.Floor(time / (1000 * 60));
		if (minutes > 0) {
			time -= minutes * 1000 * 60;
			if (precise > 0)
				num += " ";
			num += minutes + " min";
			if (minutes > 1)
				num += "s";

			precise++;
		}
		if (precise >= precision)
			return num;

		double seconds = Math.Floor(time / (1000));
		if (seconds > 0) {
			time -= seconds * 1000;
			if (precise > 0)
				num += " ";
			num += seconds + " sec";
			if (seconds > 1)
				num += "s";

			precise++;
		}
		if (precise >= precision)
			return num;


		return num;
	}
}
