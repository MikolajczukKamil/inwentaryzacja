using Inwentaryzacja.Models;
using System;
using System.Collections.Generic;

namespace Inwentaryzacja.Controllers.Api
{
    /// <summary>
    /// Encja srodka trwalego
    /// </summary>
    public class AssetEntity
    {
        /// <summary>
        /// Numer id srodka trwalego
        /// </summary>
        public int id;
        
        /// <summary>
        /// Typ srodka trwalego
        /// </summary>
        public AssetTypeEntity type;
        
        /// <summary>
        /// Porownanie czy podany obiekt jest taki sam
        /// </summary>
        /// <param name="obj">Obiekt do porownania</param>
        /// <returns>Czy podany obiekt jest taki sam</returns>
        public override bool Equals(object obj)
        {
            if (id == ((AssetEntity)obj).id && type.Equals(((AssetEntity)obj).type))
                return true;
            return false;
        }

        /// <summary>
        /// Zwraca kod hash obiektu
        /// </summary>
        /// <returns>Kod hash obiektu</returns>
        public override int GetHashCode() => id.GetHashCode();
    }

    /// <summary>
    /// Encja informacji o srodku trwalym
    /// </summary>
    public class AssetInfoEntity : AssetEntity
    {
        /// <summary>
        /// Pokoj w ktorym znajduje sie srodek trwaly
        /// </summary>
        public RoomEntity room;
    }

    /// <summary>
    /// Encja typu srodka trwalego
    /// </summary>
    public class AssetTypeEntity
    {
        /// <summary>
        /// Id typu
        /// </summary>
        public int id;
        
        /// <summary>
        /// Litera typu
        /// </summary>
        public char letter;
        
        /// <summary>
        /// Nazwa typu
        /// </summary>
        public string name;
        
        /// <summary>
        /// Porownanie czy podany obiekt jest taki sam
        /// </summary>
        /// <param name="obj">Obiekt do porownania</param>
        /// <returns>Czy podany obiekt jest taki sam</returns>
        public override bool Equals(object obj)
        {
            if (id == ((AssetTypeEntity)obj).id && letter == ((AssetTypeEntity)obj).letter && name == ((AssetTypeEntity)obj).name)
                return true;
            return false;
        }
        
        /// <summary>
        /// Zwraca kod hash obiektu
        /// </summary>
        /// <returns>Kod hash obiektu</returns>
        public override int GetHashCode() => id.GetHashCode();
    }

    /// <summary>
    /// Encja pokoju
    /// </summary>
    public class RoomEntity
    {
        /// <summary>
        /// Numer id pokoju
        /// </summary>
        public int id;
        
        /// <summary>
        /// Nazwa pokoju
        /// </summary>
        public string name;
        
        /// <summary>
        /// Budynek w ktorym znajduje sie pokoj
        /// </summary>
        public BuildingEntity building;
        
        /// <summary>
        /// Porownanie czy podany obiekt jest taki sam
        /// </summary>
        /// <param name="obj">Obiekt do porownania</param>
        /// <returns>Czy podany obiekt jest taki sam</returns>
        public override bool Equals(object obj)
        {
            if (building.Equals(((RoomEntity)obj).building))
            {
                if (id == ((RoomEntity)obj).id && name == ((RoomEntity)obj).name)
                    return true;
            }
            return false;
        }
        
        /// <summary>
        /// Zwraca kod hash obiektu
        /// </summary>
        /// <returns>Kod hash obiektu</returns>
        public override int GetHashCode() => id.GetHashCode();
    }

    /// <summary>
    /// Encja budynku
    /// </summary>
    public class BuildingEntity
    {
        /// <summary>
        /// Id budynku
        /// </summary>
        public int id;
        
        /// <summary>
        /// Nazwa budynku
        /// </summary>
        public string name;
        
        /// <summary>
        /// Porownanie czy podany obiekt jest taki sam
        /// </summary>
        /// <param name="obj">Obiekt do porownania</param>
        /// <returns>Czy podany obiekt jest taki sam</returns>
        public override bool Equals(object obj)
        {
            if (id == ((BuildingEntity)obj).id && name == ((BuildingEntity)obj).name)
                return true;
            return false;
        }
        
        /// <summary>
        /// Zwraca kod hash obiektu
        /// </summary>
        /// <returns>Kod hash obiektu</returns>
        public override int GetHashCode() => id.GetHashCode();
    }

    /// <summary>
    /// Encja naglowka raportu
    /// </summary>
    public class ReportHeaderEntity
    {
        /// <summary>
        /// Numer id raportu
        /// </summary>
        public int id;
        
        /// <summary>
        /// Nazwa raportu
        /// </summary>
        public string name;
        
        /// <summary>
        /// Data wygenerowania raportu
        /// </summary>
        public DateTime create_date;
        
        /// <summary>
        /// Wlasciciel raportu
        /// </summary>
        public UserEntity owner;
        
        /// <summary>
        /// Pokoj w ktorym zostal wygenerowany raport
        /// </summary>
        public RoomEntity room;
        
        /// <summary>
        /// Porownanie czy podany obiekt jest taki sam
        /// </summary>
        /// <param name="obj">Obiekt do porownania</param>
        /// <returns>Czy podany obiekt jest taki sam</returns>
        public override bool Equals(object obj)
        {
            if (id == ((ReportHeaderEntity)obj).id && name == ((ReportHeaderEntity)obj).name && owner.Equals(((ReportHeaderEntity)obj).owner) && room.Equals(((ReportHeaderEntity)obj).room) && create_date.Equals(((ReportHeaderEntity)obj).create_date))
                return true;
            return false;
        }
        
        /// <summary>
        /// Zwraca kod hash obiektu
        /// </summary>
        /// <returns>Kod hash obiektu</returns>
        public override int GetHashCode() => id.GetHashCode();
    }

    /// <summary>
    /// Encja srodka trwalego w raporcie
    /// </summary>
    public class ReportPositionEntity
    {
        /// <summary>
        /// Srodek trwaly
        /// </summary>
        public AssetEntity asset;
        
        /// <summary>
        /// Pokoj w ktorym poprzednio znajdowal sie srodek trwaly
        /// </summary>
        public RoomEntity previous_room;
        
        /// <summary>
        /// Czy srodek trwaly obecnie znajduje sie w tym pokoju
        /// </summary>
        public bool present;
        
        /// <summary>
        /// Porownanie czy podany obiekt jest taki sam
        /// </summary>
        /// <param name="obj">Obiekt do porownania</param>
        /// <returns>Czy podany obiekt jest taki sam</returns>
        public override bool Equals(object obj)
        {
            if (asset.Equals(((ReportPositionEntity)obj).asset) && previous_room.Equals(((ReportPositionEntity)obj).previous_room) && present == ((ReportPositionEntity)obj).present)
                return true;
            return false;
        }
        
        /// <summary>
        /// Zwraca kod hash obiektu
        /// </summary>
        /// <returns>Kod hash obiektu</returns>
        public override int GetHashCode() => asset.GetHashCode() * asset.GetHashCode();
    }

    /// <summary>
    /// Encja uzytkownika
    /// </summary>
    public class UserEntity
    {
        /// <summary>
        /// Numer id uzytkownika
        /// </summary>
        public int id;
        
        /// <summary>
        /// Login uzytkownika
        /// </summary>
        public string login;
        
        /// <summary>
        /// Porownanie czy podany obiekt jest taki sam
        /// </summary>
        /// <param name="obj">Obiekt do porownania</param>
        /// <returns>Czy podany obiekt jest taki sam</returns>
        public override bool Equals(object obj)
        {
            if (id == ((UserEntity)obj).id && login == ((UserEntity)obj).login)
                return true;
            return false;
        }
        
        /// <summary>
        /// Zwraca kod hash obiektu
        /// </summary>
        /// <returns>Kod hash obiektu</returns>
        public override int GetHashCode() => id.GetHashCode();

    }

    /// <summary>
    /// Encja odpowiedzi
    /// </summary>
    public class AnswerEntity
    {
        /// <summary>
        /// Numer id odpowiedzi
        /// </summary>
        public int id;
        
        /// <summary>
        /// Wiadomosc
        /// </summary>
        public string message;
    }

    /// <summary>
    /// Encja skanu
    /// </summary>
    public class ScanEntity
    {
        /// <summary>
        /// Numer id skanu
        /// </summary>
        public int id;
        
        /// <summary>
        /// Pokoj w dokonanu skanowania
        /// </summary>
        public RoomEntity room;
        
        /// <summary>
        /// Wlasciciel skanu
        /// </summary>
        public UserEntity owner;
        public DateTime create_date;
        
        /// <summary>
        /// Porownanie czy podany obiekt jest taki sam
        /// </summary>
        /// <param name="obj">Obiekt do porownania</param>
        /// <returns>Czy podany obiekt jest taki sam</returns>
        public override bool Equals(object obj)
        {
            if (id == ((ScanEntity)obj).id && room.Equals(((ScanEntity)obj).room) && owner.Equals(((ScanEntity)obj).owner) && create_date.Equals(((ScanEntity)obj).create_date))
                return true;
            return false;
        }
        
        /// <summary>
        /// Zwraca kod hash obiektu
        /// </summary>
        /// <returns>Kod hash obiektu</returns>
        public override int GetHashCode() => id.GetHashCode();
    }

    /// <summary>
    /// Encja srodka trwalego w skanowaniu
    /// </summary>
    public class ScanPositionEntity
    {
        /// <summary>
        /// Aktualny stan srodka trwalego
        /// </summary>
        public int state;
        
        /// <summary>
        /// Srodek trwaly
        /// </summary>
        public AssetInfoEntity asset;
        
        /// <summary>
        /// Porownanie czy podany obiekt jest taki sam
        /// </summary>
        /// <param name="obj">Obiekt do porownania</param>
        /// <returns>Czy podany obiekt jest taki sam</returns>
        public override bool Equals(object obj)
        {
            if (state == ((ScanPositionEntity)obj).state && asset.Equals(((ScanPositionEntity)obj).asset))
                return true;
            return false;
        }

        /// <summary>
        /// Zwraca kod hash obiektu
        /// </summary>
        /// <returns>Kod hash obiektu</returns>
        public override int GetHashCode() => state.GetHashCode()*asset.id.GetHashCode();
    }
}
