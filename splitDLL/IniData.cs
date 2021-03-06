﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using SA_Tools;

namespace splitDLL
{
    public class IniData
    {
        [IniName("game")]
        [DefaultValue(Game.SADX)]
        public Game Game { get; set; }
		[IniName("modulename")]
		public string ModuleName { get; set; }
        [IniCollection(IniCollectionMode.IndexOnly)]
        public Dictionary<string, FileInfo> Files { get; set; }
    }

    public enum Game
    {
        SADX,
        SA2B
    }

    public class FileInfo
    {
        [IniName("type")]
        public string Type { get; set; }
		[IniName("length")]
		public int Length { get; set; }
        [IniName("filename")]
        public string Filename { get; set; }
    }

	public class DllIniData
	{
		[IniName("name")]
		public string Name { get; set; }
		[IniAlwaysInclude]
		[IniName("game")]
		public Game Game { get; set; }
		public DictionaryContainer<string> Exports { get; set; }
		public DictionaryContainer<FileTypeHash> Files { get; set; }
		public TexListContainer TexLists { get; set; }
		[IniName("Item")]
		[IniCollection(IniCollectionMode.NoSquareBrackets, StartIndex = 1)]
		public List<DllItemInfo> Items { get; set; }

		public DllIniData()
		{
			Exports = new DictionaryContainer<string>();
			Files = new DictionaryContainer<FileTypeHash>();
			Items = new List<DllItemInfo>();
		}
	}

	public class DictionaryContainer<T> : IEnumerable<KeyValuePair<string, T>>
	{
		[IniCollection(IniCollectionMode.IndexOnly)]
		public Dictionary<string, T> Items { get; set; }

		public DictionaryContainer()
		{
			Items = new Dictionary<string, T>();
		}

		public void Add(string key, T value)
		{
			Items.Add(key, value);
		}

		public bool ContainsKey(string key)
		{
			return Items.ContainsKey(key);
		}

		public bool Remove(string key)
		{
			return Items.Remove(key);
		}

		public bool TryGetValue(string key, out T value)
		{
			return Items.TryGetValue(key, out value);
		}

		public T this[string key]
		{
			get
			{
				return Items[key];
			}
			set
			{
				Items[key] = value;
			}
		}

		public void Clear()
		{
			Items.Clear();
		}

		[IniIgnore]
		public int Count
		{
			get { return Items.Count; }
		}

		public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
		{
			return Items.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	[System.ComponentModel.TypeConverter(typeof(StringConverter<FileTypeHash>))]
	public class FileTypeHash
	{
		public string Type { get; set; }
		public string Hash { get; set; }

		public FileTypeHash(string type, string hash)
		{
			Type = type;
			Hash = hash;
		}

		public FileTypeHash(string data)
		{
			string[] split = data.Split('|');
			Type = split[0];
			Hash = split[1];
		}

		public override string ToString()
		{
			return Type + "|" + Hash;
		}
	}

	public class TexListContainer : IEnumerable<KeyValuePair<uint, DllTexListInfo>>
	{
		[IniCollection(IniCollectionMode.IndexOnly, KeyConverter = typeof(UInt32HexConverter))]
		public Dictionary<uint, DllTexListInfo> Items { get; set; }

		public TexListContainer()
		{
			Items = new Dictionary<uint, DllTexListInfo>();
		}

		public void Add(uint key, DllTexListInfo value)
		{
			Items.Add(key, value);
		}

		public bool ContainsKey(uint key)
		{
			return Items.ContainsKey(key);
		}

		public bool Remove(uint key)
		{
			return Items.Remove(key);
		}

		public bool TryGetValue(uint key, out DllTexListInfo value)
		{
			return Items.TryGetValue(key, out value);
		}

		public DllTexListInfo this[uint key]
		{
			get
			{
				return Items[key];
			}
			set
			{
				Items[key] = value;
			}
		}

		public void Clear()
		{
			Items.Clear();
		}

		[IniIgnore]
		public int Count
		{
			get { return Items.Count; }
		}

		public IEnumerator<KeyValuePair<uint, DllTexListInfo>> GetEnumerator()
		{
			return Items.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	[TypeConverter(typeof(StringConverter<DllTexListInfo>))]
	public class DllTexListInfo
	{
		public string Export { get; set; }
		public int? Index { get; set; }

		public DllTexListInfo(string data)
		{
			string[] split = data.Split(',');
			Export = split[0];
			if (split.Length > 1)
				Index = int.Parse(split[1]);
		}

		public DllTexListInfo(string export, int? index)
		{
			Export = export;
			Index = index;
		}

		public override string ToString()
		{
			if (Index.HasValue)
				return $"{Export},{Index}";
			else
				return Export;
		}
	}

	public class DllItemInfo
	{
		public string Export { get; set; }
		public int? Index { get; set; }
		public string Field { get; set; }
		public string Label { get; set; }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder(Export);
			if (Index.HasValue)
				sb.Append(Index.Value);
			if (!string.IsNullOrEmpty(Field))
				sb.AppendFormat("->{0}", Field);
			return sb.ToString();
		}
	}
}