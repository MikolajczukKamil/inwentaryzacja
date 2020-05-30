using Inwentaryzacja.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inwentaryzacja.views.view_allReports
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportDetailsView : ContentPage
    {
        public ReportDetailsView(string headerText, string roomText, string createDate, string createTime, string ownerText, string inThisRoom, string moveToRoom, string moveFromRoom, string inAnotherRoom, string scannedAll, string scannedAllDetails, string inThisRoomDetails, string movedToRoomDetails, string movedFromRoomDetails, string inAnotherRoomDetails)
        {
            InitializeComponent();
            FillViewWithText(headerText, roomText, createDate, createTime, ownerText, inThisRoom, moveToRoom, moveFromRoom, inAnotherRoom, scannedAll);
            DetailsButtonsImplementation(scannedAllDetails, inThisRoomDetails, movedToRoomDetails, movedFromRoomDetails, inAnotherRoomDetails);            
        }

        private void FillViewWithText(string headerText, string roomText, string createDate, string createTime, string ownerText, string inThisRoom, string moveToRoom, string moveFromRoom, string inAnotherRoom, string scannedAll)
        {
            RoomText.Text = "Nazwa sali: " + roomText;
            CreateDate.Text = "Data utworzenia: " + createDate;
            CreateTime.Text = "Godzina utworzenia: " + createTime;
            OwnerText.Text = "Wykonał: " + ownerText;
            ScannedAll.Text = scannedAll;
            HeaderText.Text = headerText;
            InThisRoom.Text = inThisRoom;
            MoveToRoom.Text = moveToRoom;
            MoveFromRoom.Text = moveFromRoom;
            InAnotherRoom.Text = inAnotherRoom;

            if (ScannedAll.Text == "")
            {
                ScannedAllHeader.IsVisible = false;
                ScannedAll.IsVisible = false;
                ScannedAllBtn.IsVisible = false;
                FrameRequest.HeightRequest -= 50;
            }
            if (MoveToRoom.Text == "")
            {
                MoveToRoomHeader.IsVisible = false;
                MoveToRoom.IsVisible = false;
                MoveToRoomBtn.IsVisible = false;
                FrameRequest.HeightRequest -= 50;
            }
            if (MoveFromRoom.Text == "")
            {
                MoveFromRoomHeader.IsVisible = false;
                MoveFromRoom.IsVisible = false;
                MoveFromRoomBtn.IsVisible = false;
                FrameRequest.HeightRequest -= 50;
            }
            if (InAnotherRoom.Text == "")
            {
                InAnotherRoomHeader.IsVisible = false;
                InAnotherRoom.IsVisible = false;
                InAnotherRoomBtn.IsVisible = false;
                FrameRequest.HeightRequest -= 50;
            }
            if (InThisRoom.Text == "")
            {
                InThisRoomHeader.IsVisible = false;
                InThisRoom.IsVisible = false;
                InThisRoomBtn.IsVisible = false;
                FrameRequest.HeightRequest -= 50;
            }
        }

        private void DetailsButtonsImplementation(string scannedAllDetails, string inThisRoomDetails, string movedToRoomDetails, string movedFromRoomDetails, string inAnotherRoomDetails)
        {
            ScannedAllBtn.Clicked += (object o, EventArgs e) => DisplayAlert("Szczegóły", scannedAllDetails, "OK");
            MoveToRoomBtn.Clicked += (object o, EventArgs e) => DisplayAlert("Szczegóły", movedToRoomDetails, "OK");
            MoveFromRoomBtn.Clicked += (object o, EventArgs e) => DisplayAlert("Szczegóły", movedFromRoomDetails, "OK");
            InThisRoomBtn.Clicked += (object o, EventArgs e) => DisplayAlert("Szczegóły", inThisRoomDetails, "OK");
            InAnotherRoomBtn.Clicked += (object o, EventArgs e) => DisplayAlert("Szczegóły", inAnotherRoomDetails, "OK");
        }

        private void return_ChooseRoom(object o, EventArgs e)
        {
            App.Current.MainPage = new AllReportsPage();
        }


    }
}