<?php


interface IRepository
{
    /**
     * Funkcja wyszukuje pojedynczy element w bazie po jego id i go zwraca
     * @param integer $id id szukanego elementu
     *
     * @return mixed - zwraca element z bazy o podanym id
     */

    function find($id);
    /**
     * Funkcja wyszukuje wszystkie elementy w bazie  i je zwraca
     *
     * @return mixed - zwraca wszystkie elementy z bazy
     */
    function findAll();
    /**
     * Funkcja usuwa pojedynczy element z bazy po jego id
     *  @param integer $id id usuwanego elementu
     */
    function deleteOne($id);
    /**
     * Funkcja dodaje pojedynczy element do bazy
     * @param $object - typ zaleznie od tego co implementuje interfejs - element dodawany
     */
    function addNew($object);
}