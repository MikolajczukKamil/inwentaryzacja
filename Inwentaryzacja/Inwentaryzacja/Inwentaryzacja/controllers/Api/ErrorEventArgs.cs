﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Inwentaryzacja.Controllers.Api
{
    /// <summary>
    /// Klasa obslugujaca kody bledow
    /// </summary>
    public class ErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Status bledu
        /// </summary>
        public int ErrorStatus;
        
        /// <summary>
        /// Komunikat bledu
        /// </summary>
        public string Message;
        
        /// <summary>
        /// Komunikat bledu dla uzytkownika
        /// </summary>
        public string MessageForUser;

        public bool Auth = true;

        /// <summary>
        /// Ustawia kod bledu
        /// </summary>
        /// <param name="code">Kod bledy</param>
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
                    MessageForUser = "Błąd połączenia z Internetem, sprawdź swoje połączenie i spróbuj ponownie."; break;
                case 404:
                    if(!String.IsNullOrEmpty(Message) && Message[0]=='<')   //prototypowy warunek sprawdzający czy błąd pochodzi z serwera czy z odpowiedzi api
                        MessageForUser = "Błąd połączenia z serwerem, proszę spróbować ponownie wykonać zapytanie.";
                    else
                        MessageForUser = "Niekompletne dane, sprawdź czy zostały podane wszystkie niezbędne informacje."; 
                    break;
                case 410:
                    MessageForUser = "Nie udało się zinterpretować odpowiedzi serwera, proszę spróbować ponownie wykonać zapytanie."; break;
                case 411:
                    MessageForUser = "Nie udało się przetworzyć danych na zapytanie http."; break;
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
                case 504:
                    MessageForUser = "Przekroczono limit czasu oczekiwania na odpowiedź serwera, spróbuj ponownie później."; break;
                default:
                    MessageForUser = "Niezidentyfikowany błąd. Kod błędu: "+code; break;
            }
        }
    }
}
