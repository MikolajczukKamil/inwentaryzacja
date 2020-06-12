using System;
using System.Runtime.CompilerServices;

namespace Inwentaryzacja.Models
{
	/// <summary>
	/// Prototyp srodka trwalego
	/// </summary>
	public class AssetPrototype
	{
		/// <summary>
		/// Typ srodka trwalego
		/// </summary>
		public int type;

		/// <summary>
		/// Konstruktor prototypu srodka trwalego
		/// </summary>
		/// <param name="type">Typ srodka trwalego</param>
		public AssetPrototype(AssetType type)
		{
			this.type = type.Id;
		}
	}
}
