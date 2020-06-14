<?php


interface IService
{
    /**
     * Funkcja wyszukuje pojedynczy element w bazie i go zwraca
     * @param integer $id id szukanego elementu
     * @return mixed - zwraca element z bazy o podanym id
     */
    static function findOneById($id);

    /**
     * Funkcja wyszukuje wszystkie elementy w bazie i je zwraca
     * @return mixed - zwraca wszystkie elementy z bazy
     */
    static function findAll();

    /**
     * Funkcja dodaje pojedynczy element do bazy
     * @param array $data dane elementu dodawanego
     */
    static function addNew($data);

    /**
     * Funkcja usuwa pojedynczy element z bazy
     *  @param integer $id id usuwanego elementu
     */
    static function deleteOneById($id);
}