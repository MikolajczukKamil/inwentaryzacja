using System;
using Inwentaryzacja.models;

namespace Inwentaryzacja.models
{ 
	public class AssetType
	{
		public char Letter { get; private set; }
		public string Name { get; private set; }

		public AssetType(string name, char letter)
		{
			Name = name;
			Letter = letter;
		}
	}
}
