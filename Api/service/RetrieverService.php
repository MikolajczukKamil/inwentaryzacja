<?php
include_once '../interfaces/IService.php';
include_once '../security/Security.php';

/**
 * Klasa obslugujaca zadania GET
 */
class RetrieverService
{
    /**
     * Funkcja wywoluje na usludze implementujacej interfejs IService odpowiednia metode.
     * @param $service - usluga do obiektu, okresla co chce zwrocic
     * @param integer $id id zwracanego obiektu
     */

    public static function RetrieveObject(IService $service, $id)
    {
        if(Security::performAuthorization())
        {
            $service::findOneById($id);
        }
    }
    /**
     * Funkcja wywoluje na usludze implementujacej interfejs IService odpowiednia metode.
     * @param $service - usluga do obiektu, okresla co chce zwrocic
     */
    public static function RetrieveAllObjects(IService $service)
    {
        if(Security::performAuthorization())
        {
            $service::findAll();
        }
    }
}