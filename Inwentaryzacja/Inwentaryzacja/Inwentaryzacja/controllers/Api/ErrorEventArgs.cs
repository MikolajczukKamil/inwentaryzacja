using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Inwentaryzacja.Controllers.Api
{
    public class ErrorEventArgs : EventArgs
    {
        public int ErrorStatus;
        public string Message;
        public string MessageForUser;

        public bool Auth = true;

        public void SetErrorStatus(int code)
        {
            ErrorStatus = code;

            switch (code)
            {
                case 400:
                    if (Auth)
                        MessageForUser = "Niekompletne dane, nie można wykonać zapytania.";
                    else
                        MessageForUser = "Błąd autoryzacji, wymagane ponowne logowanie.";
                    break;
                case 401:
                    MessageForUser = "Błędny login lub hasło, proszę spróbować jeszcze raz."; break;
                case 402:
                    MessageForUser = "Brak połączenia z Internetem, sprawdź swoje połączenie."; break;
                case 404:
                    MessageForUser = "Nie można odczytać danych."; break;
                case 500:
                    MessageForUser = "Błąd przy tworzeniu sesji, proszę spróbować jeszcze raz."; break;
                case 502:
                    MessageForUser = "Nie odnaleziono serwera. Sprawdź połączenie z internetem, bądź zmień połączenie sieciowe."; break;
                case 503:
                    if (Auth)
                        MessageForUser = "Nie udało się edytować danych. Usługa czasowo niedostępna.";
                    else
                        MessageForUser = "Błąd autoryzacji, wymagane ponowne logowanie.";
                    break;
                default:
                    MessageForUser = "Niezidentyfikowany błąd."; break;
            }
        }
    }
}
