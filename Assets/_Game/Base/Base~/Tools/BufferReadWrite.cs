using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BufferReadWrite : IDisposable {
	List<byte> Buff;
	byte[] readBuff;
	int readpos;
	bool buffUpdate = false;

	public BufferReadWrite() {
		Buff = new List<byte>();
		readpos = 0;
	}

	public int GetReadPos() {
		return readpos;
	}

	public byte[] ToArray() {
		return Buff.ToArray();
	}

	public int Count() {
		return Buff.Count;
	}

	public int BufferLeft() {
		return Count() - readpos;
	}

	public void Clear() {
		Buff.Clear();
		ResetReadPos();
	}

	public void ResetReadPos() {
		readpos = 0;
	}

	// ------------------- Write -------------------------------

	public void WriteByte(byte data) {
		Buff.Add(data);
		buffUpdate = true;
	}

	public void WriteBytes(byte[] data) {
		if (data != null) {
			Buff.AddRange(data);
			buffUpdate = true;
		}
	}

	public void WriteInteger(int data) {
		Buff.AddRange(BitConverter.GetBytes(data));
		buffUpdate = true;
	}

	public void WriteFloat(float data) {
		Buff.AddRange(BitConverter.GetBytes(data));
		buffUpdate = true;
	}

	public void WriteDouble(double data) {
		Buff.AddRange(BitConverter.GetBytes(data));
		buffUpdate = true;
	}

	public void WriteString(string data) {
        Buff.AddRange(BitConverter.GetBytes(data.Length));
        Buff.AddRange(Encoding.ASCII.GetBytes(data));
        buffUpdate = true;
	}

	public void WriteDateTime(System.DateTime dateTime) {
		WriteInteger(dateTime.Year);
		WriteByte((byte)dateTime.Month);
		WriteInteger(dateTime.Day);
		WriteByte((byte)dateTime.Hour);
		WriteByte((byte)dateTime.Minute);
		WriteByte((byte)dateTime.Second);
		/*
		Buff.AddRange(BitConverter.GetBytes());
		Buff.Add((byte)dateTime.Month);
		Buff.Add((byte)dateTime.Day);
		Buff.Add((byte)dateTime.Hour);
		Buff.Add((byte)dateTime.Minute);
		Buff.Add((byte)dateTime.Second);*/
		buffUpdate = true;
	}

	// ------------------- READ -------------------------------

	public byte ReadByte(bool peek = true) {
		if (Buff.Count > readpos) {
			UpdateReadBuff();

			byte value = readBuff[readpos];
			if (peek && Buff.Count > readpos) {
				readpos += 1;
			}
			return value;
		}
		else {
			throw new Exception("Byte Buffer is past its Limit!");
		}
	}

	public byte[] ReadBytes(int length, bool peek = true) {
		UpdateReadBuff();

		byte[] value = Buff.GetRange(readpos, length).ToArray();
		if (peek) {
			readpos += length;
		}
		return value;
	}

	public int ReadInteger(bool peek = true) {
		if (Buff.Count > readpos) {
			UpdateReadBuff();

			int value = BitConverter.ToInt32(readBuff, readpos);
			if (peek && Buff.Count > readpos) {
				readpos += 4;
			}
			return value;
		}
		else {
			throw new Exception("Byte Buffer is past its Limit!");
		}
	}

	public float ReadFloat(bool peek = true) {
		if (Buff.Count > readpos) {
			UpdateReadBuff();

			float value = BitConverter.ToSingle(readBuff, readpos);
			if (peek && Buff.Count > readpos) {
				readpos += 4;
			}
			return value;
		}
		else {
			throw new Exception("Byte Buffer is past its Limit!");
		}
	}

	public double ReadDouble(bool peek = true) {
		if (Buff.Count > readpos) {
			UpdateReadBuff();

			double value = BitConverter.ToDouble(readBuff, readpos);
			if (peek && Buff.Count > readpos) {
				readpos += 8;
			}
			return value;
		}
		else {
			throw new Exception("Byte Buffer is past its Limit!");
		}
	}

	public string ReadString(bool peek = true) {
		int len = ReadInteger(true);
		UpdateReadBuff();

        string value = Encoding.ASCII.GetString(readBuff, readpos, len);
        if (peek && Buff.Count > readpos) {
			readpos += len;
		}
		return value;
	}

	public System.DateTime ReadDateTime() {
		System.DateTime value;

		UpdateReadBuff();

		int yearD = ReadInteger();
		byte monthD = ReadByte();
		int dayD = ReadInteger();
		byte hourD = ReadByte();
		byte minuteD = ReadByte();
		byte secondD = ReadByte();

		value = new System.DateTime(yearD, monthD, dayD, hourD, minuteD, secondD);

		return value;
	}

	// ---------------------------------------------------------------

	void UpdateReadBuff() {
		if (buffUpdate) {
			readBuff = Buff.ToArray();
			buffUpdate = false;
		}
	}

	// IDisposable
	private bool disposedValue = false;
	protected virtual void Dispose(bool disposing) {
		if (!this.disposedValue) {
			if (disposing) {
				Buff.Clear();
			}

			readpos = 0;
		}
		this.disposedValue = true;
	}

	public void Dispose() {
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
