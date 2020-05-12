using System;
using Inwentaryzacja.Models;

namespace Inwentaryzacja.Models
{ 
	public class AssetType
	{
		public int Id { get; private set; }
		public char Letter { get; private set; }
		public string Name { get; private set; }

		public AssetType(int id, string name, char letter)
		{
			Id = id;
			Name = name;
			Letter = letter;
		}
	}
}
