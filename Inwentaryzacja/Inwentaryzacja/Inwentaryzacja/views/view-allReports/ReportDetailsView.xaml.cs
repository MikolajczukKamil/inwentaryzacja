using Inwentaryzacja.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Inwentaryzacja.Controllers.Api;

namespace Inwentaryzacja.views.view_allReports
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    /// <summary>
    /// Klasa odpowiadajaca za widok okna szczegolow raportu
    /// </summary>
    public partial class ReportDetailsView : ContentPage
    {

	    /// <summary>
	    /// Zmienne odpowiadające za pobieranie pliku
	    /// </summary>
	    private IDownloadFile File;
	    private bool isDownloading = true;
	    private int reportId;

	    /// <summary>
	    /// Konstruktor klasy
	    /// </summary>
	    public ReportDetailsView(string headerText, string roomText, string createDate, string createTime, string ownerText, string inThisRoom, string moveToRoom, string moveFromRoom, string inAnotherRoom, string scannedAll, string scannedAllDetails, string inThisRoomDetails, string movedToRoomDetails, string movedFromRoomDetails, string inAnotherRoomDetails, string scannedAllLabel, string moveFromRoomLabel, string moveToRoomLabel, string inAnotherRoomLabel, string inThisRoomLabel, int id)
        {
            InitializeComponent();
            reportId = id;
            FillViewWithText(headerText, roomText, createDate, createTime, ownerText, inThisRoom, moveToRoom, moveFromRoom, inAnotherRoom, scannedAll, scannedAllLabel, moveFromRoomLabel, moveToRoomLabel, inAnotherRoomLabel, inThisRoomLabel);
            DetailsButtonsImplementation(scannedAllDetails, inThisRoomDetails, movedToRoomDetails, movedFromRoomDetails, inAnotherRoomDetails);
            CrossDownloadManager.Current.CollectionChanged += (sender, e) =>
	            System.Diagnostics.Debug.WriteLine(
		            "[DownloadManager] " + e.Action +
                    " -> New items: " + (e.NewItems?.Count ?? 0) +
                    " at " + e.NewStartingIndex + 
                    " || Old items: " + (e.OldItems?.Count ?? 0) + 
                    " at " + e.OldStartingIndex
	            );
        }

        /// <summary>
        /// Funkcja odpowiadajaca za pobranie pliku
        /// </summary>
        public async void DownloadFile(string fileName)
        {
	        await Task.Yield();

	        await Task.Run(() =>
	        {
		        var downloadManager = CrossDownloadManager.Current;
		        var file = downloadManager.CreateDownloadFile(fileName);
                downloadManager.Start(file, true);

                while (isDownloading)
                {
	                isDownloading = IsDownloading(file);
                }
	        });

	        if (!isDownloading)
	        {
		        await DisplayAlert("Status pliku", "Raport pobrany pomyślnie", "OK");
	        }
        }

        /// <summary>
        /// Funkcja odpowiadajaca za sprawdzenie czy plik jest pobierany
        /// </summary>
        /// <returns>true jezeli tak, false jezeli nie</returns>
        public bool IsDownloading(IDownloadFile file)
        {
            if (file == null)
                return false;

            switch (file.Status)
            {
	            case DownloadFileStatus.INITIALIZED:
	            case DownloadFileStatus.PAUSED:
	            case DownloadFileStatus.PENDING:
	            case DownloadFileStatus.RUNNING:
                    return true;
                
                case DownloadFileStatus.COMPLETED:
                case DownloadFileStatus.CANCELED:
                case DownloadFileStatus.FAILED:
	                return false;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Funkcja odpowiadajaca za pobranie pliku po kliknieciu przycisku
        /// </summary>
        private void DownloadBtn_Clicked(object sender, EventArgs e)
        {
            DownloadFile($"{APIController.BaseUrl}/pdfGenerator/{reportId}/1/report-{reportId}.pdf");
        }

        /// <summary>
        /// Funkcja odpowiadajaca za wypelnienie okna widoku tekstem
        /// </summary>
        private void FillViewWithText(string headerText, string roomText, string createDate, string createTime, string ownerText, string inThisRoom, string moveToRoom, string moveFromRoom, string inAnotherRoom, string scannedAll, string scannedAllLabel, string moveFromRoomLabel, string moveToRoomLabel, string inAnotherRoomLabel, string inThisRoomLabel)
        {
            RoomText.Text = "Nazwa sali: " + roomText;
            CreateDate.Text = "Data utworzenia: " + createDate;
            CreateTime.Text = "Godzina utworzenia: " + createTime;
            OwnerText.Text = "Wykonał: " + ownerText;
            ScannedAll.Text = scannedAll;
            ScannedAllLabel.Text = scannedAllLabel;
            HeaderText.Text = headerText;
            InThisRoom.Text = inThisRoom;
            InThisRoomLabel.Text = inThisRoomLabel;
            MoveToRoom.Text = moveToRoom;
            MoveToRoomLabel.Text = moveToRoomLabel;
            MoveFromRoom.Text = moveFromRoom;
            MoveFromRoomLabel.Text = moveFromRoomLabel;
            InAnotherRoom.Text = inAnotherRoom;
            InAnotherRoomLabel.Text = inAnotherRoomLabel;

            if (ScannedAll.Text == "")
            {
                ScannedAllHeader.IsVisible = false;
                ScannedAll.IsVisible = false;
                ScannedAllLabel.IsVisible = false;
                ScannedAllBtn.IsVisible = false;
                FrameRequest.HeightRequest -= 50;
            }

            if (MoveToRoom.Text == "")
            {
                MoveToRoomHeader.IsVisible = false;
                MoveToRoom.IsVisible = false;
                MoveToRoomLabel.IsVisible = false;
                MoveToRoomBtn.IsVisible = false;
                FrameRequest.HeightRequest -= 50;
            }

            if (MoveFromRoom.Text == "")
            {
                MoveFromRoomHeader.IsVisible = false;
                MoveFromRoom.IsVisible = false;
                MoveFromRoomLabel.IsVisible = false;
                MoveFromRoomBtn.IsVisible = false;
                FrameRequest.HeightRequest -= 50;
            }

            if (InAnotherRoom.Text == "")
            {
                InAnotherRoomHeader.IsVisible = false;
                InAnotherRoom.IsVisible = false;
                InAnotherRoomLabel.IsVisible = false;
                InAnotherRoomBtn.IsVisible = false;
                FrameRequest.HeightRequest -= 50;
            }

            if (InThisRoom.Text == "")
            {
                InThisRoomHeader.IsVisible = false;
                InThisRoom.IsVisible = false;
                InThisRoomLabel.IsVisible = false;
                InThisRoomBtn.IsVisible = false;
                FrameRequest.HeightRequest -= 50;
            }
        }

        /// <summary>
        /// Funkcja odpowiadajaca za wyswietlenie przyciskow szczegolow
        /// </summary>
        private void DetailsButtonsImplementation(string scannedAllDetails, string inThisRoomDetails, string movedToRoomDetails, string movedFromRoomDetails, string inAnotherRoomDetails)
        {
            ScannedAllBtn.Clicked += (object o, EventArgs e) => DisplayAlert("Szczegóły", scannedAllDetails, "OK");
            MoveToRoomBtn.Clicked += (object o, EventArgs e) => DisplayAlert("Szczegóły", movedToRoomDetails, "OK");
            MoveFromRoomBtn.Clicked += (object o, EventArgs e) => DisplayAlert("Szczegóły", movedFromRoomDetails, "OK");
            InThisRoomBtn.Clicked += (object o, EventArgs e) => DisplayAlert("Szczegóły", inThisRoomDetails, "OK");
            InAnotherRoomBtn.Clicked += (object o, EventArgs e) => DisplayAlert("Szczegóły", inAnotherRoomDetails, "OK");
        }
       
        /// <summary>
        /// Funkcja odpowiadajaca za wybranie pokoju
        /// </summary>
        private async void return_ChooseRoom(object o, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        /// <summary>
        /// Funkcja odpowiadajaca za obsluge przycisku powrotu
        /// </summary>
        protected override bool OnBackButtonPressed()
        {
            Navigation.PopAsync();
            return true;           
        }
    }
}
