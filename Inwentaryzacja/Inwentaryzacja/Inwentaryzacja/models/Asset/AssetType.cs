using System;
using Inwentaryzacja.Models;

namespace Inwentaryzacja.Models
{ 
	/// <summary>
	/// Typ srodka trwalego
	/// </summary>
	public class AssetType
	{
		/// <summary>
		/// Id typu
		/// </summary>
		public int Id { get; private set; }
		
		/// <summary>
		/// Litera symblizujaca typ
		/// </summary>
		public char Letter { get; private set; }
		
		/// <summary>
		/// Nazwa typu
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Konstruktor typu
		/// </summary>
		/// <param name="id">Id typu</param>
		/// <param name="name">Nazwa typu</param>
		/// <param name="letter">Litera symbolizujaca typ</param>
		public AssetType(int id, string name, char letter)
		{
			Id = id;
			Name = name;
			Letter = letter;
		}
	}
}
