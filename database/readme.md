# Generowanie importu do bazy

Do generowania plków wynikowych wymaga nodejs

- `node build -min -watch` - po prostu skleja pliki, można łączyć z dowolnymi przełącznikami

Przełączniki
- `-min` - wersja z umiuniętymi komentarzami oraz wcięciami
- `-watch` - automatyczna przebudowa po zapisaniu pliku

Po wygenerowaniu dostępne są nastepujące pliki
- `dist/db-data.sql` - dane czyli tabele + zawartość
- `dist/db-procedures.sql` - procedury + funkcje pomocnicze
- `dist/db-all.sql` - wszystko razem

# Opis procedur

<pre>
getReportsHeaders(user_id INT): {
  id INT
  name VARCHAR
  create_date DATETIME
  owner_id INT
  owner_name VARCHAR
  room_id INT
  room_name VARCHAR
  building_id INT 
  building_name VARCHAR 
}[]
</pre>

<pre>
getReportHeader(report_id INT): {
  id INT
  name VARCHAR
  create_date DATETIME
  owner_id INT
  owner_name VARCHAR
  room_id INT
  room_name VARCHAR
  building_id INT
  building_name VARCHAR 
}
</pre>

<pre>
getPositionsInReport(report_id INT): {
  present BOOLEAN
  asset_id INT
  type_id INT
  type_letter CHAR
  type_name VARCHAR
  previous_id INT|null
  previous_name VARCHAR|null
  previous_building_id INT|null
  previous_building_name VARCHAR|null
}[]
</pre>

<pre>
getAssetsInRoom(room_id INT): {
  id INT
  type INT
  asset_type_letter CHAR
  asset_type_name VARCHAR
}[]
</pre>

<pre>
getAssetInfo(asset_id INT): {
  id INT
  type INT
  letter CHAR
  asset_type_name VARCHAR
  room_id INT
  room_name VARCHAR
  building_id INT
  building_name VARCHAR
}
</pre>

<pre>
addNewAsset(type_id INT): { id INT }
</pre>

<pre>
getUser(usser_id INT): {
  id INT
  login VARCHAR
  hash VARCHAR
}
</pre>

<pre>
getUserByLogin(user_login VARCHAR): {
  id INT
  login VARCHAR
  hash VARCHAR
}
</pre>

<pre>
getLoginSession(user_token VARCHAR): {
  id INT
  user_id INT
  expiration_date DATETIME
  token VARCHAR
  expired BOOLEAN
}
</pre>

<pre>
addLoginSession(user_id INT, expiration_date DATETIME, user_token VARCHAR): { id INT } 
</pre>

<pre>
deleteLoginSession(user_token VARCHAR): VOID
</pre>

<pre>
-- report_positions - json string of array of { id INT, previous: INT|NULL, present: BOOLEAN }
addNewReport(
  report_name VARCHAR,
  report_room INT,
  report_owner INT,
  report_positions VARCHAR( JSON( { id INT, previous: INT|NULL, present: BOOLEAN } ) )
): { id INT } 
</pre>

<pre>
addRoom(room_name VARCHAR, building_id INT): { id INT }
</pre>

<pre>
addBuilding(building_name VARCHAR): { id INT }
</pre>

<pre>
getRooms(building_id INT): {
  id INT
  name VARCHAR
  building_id INT
  building_name VARCHAR
}
</pre>

<pre>
getBuildings(): { id INT, name VARCHAR }
</pre>

# Opis zawartości batel w fake bazie

## Typy środków trwałych
- 1 c komputer
- 2 k krzesło
- 3 m monitor
- 4 p projektor
- 5 s stół
- 6 t tablica

## Budynki
- 1-10
- b. 34, id = 1
- b. 33, id 10, jest pusty - nie ma sal

## Pomieszczenia
- 1-3 z budynku 34
- 4-11, dla (n = 2-9) sala z budynku name = "1/n" id = n

## Środki trwałe
- 30 zestawów
- po 6 rzeczu wg asset_types

## Użytkownicy
- user1 do user5, hasło 111 do 555

## Raporty

### Z przeniosinami w budynku b. 34
- Raport nr 1 z sali 1 zawiera assety 1-6
- Raport nr 2 z sali 2 zawiera assety 7-12
- Raport nr 3 z sali 1 dodaje assety 7-8 z sali 2, usuwa 5-6
- Raport nr 4 z sali 3 zawiera assety 13-18
- Raport nr 5 z sali 1 dodaje assety 13-14 z sali 3, usuwa 3-4 oraz zawiera assety 9, 10 ale nie zapisuje ich, zawiera wszystkie możliwe opcje

### Stan końcowy
- W sali 1 assety id: 1, 2, 7, 8, 13, 14
- W sali 2 assety id: 9, 10, 11, 12
- W sali 3 assety id: 15, 16, 17, 18
- Bez sali: 3, 4, 5, 6

### Bez zmian
- Raport nr 6 z sali 1/2 b. 2 zawiera assety 19-24

### Stan końcowy
- W sali 1/2 b. 2: 19, 20, 21, 22, 23, 24
- Bez sali: 25-180
