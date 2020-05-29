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
        public ReportDetailsView(string headerText, string roomText, string createDate, string createTime, string ownerText, string inThisRoom, string moveToRoom, string moveFromRoom, string inAnotherRoom, string scannedAll)
        {
            InitializeComponent();
            FillViewWithText(headerText, roomText, createDate, createTime, ownerText, inThisRoom, moveToRoom, moveFromRoom, inAnotherRoom, scannedAll);
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
                FrameRequest.HeightRequest -= 65;
            }
            if (MoveToRoom.Text == "")
            {
                MoveToRoomHeader.IsVisible = false;
                MoveToRoom.IsVisible = false;
                MoveToRoomBtn.IsVisible = false;
                FrameRequest.HeightRequest -= 65;
            }
            if (MoveFromRoom.Text == "")
            {
                MoveFromRoomHeader.IsVisible = false;
                MoveFromRoom.IsVisible = false;
                MoveFromRoomBtn.IsVisible = false;
                FrameRequest.HeightRequest -= 65;
            }
            if (InAnotherRoom.Text == "")
            {
                InAnotherRoomHeader.IsVisible = false;
                InAnotherRoom.IsVisible = false;
                InAnotherRoomBtn.IsVisible = false;
                FrameRequest.HeightRequest -= 65;
            }
            if (InThisRoom.Text == "")
            {
                InThisRoomHeader.IsVisible = false;
                InThisRoom.IsVisible = false;
                InThisRoomBtn.IsVisible = false;
                FrameRequest.HeightRequest -= 60;
            }
        }

        private void return_ChooseRoom(object o, EventArgs e)
        {
            App.Current.MainPage = new AllReportsPage();
        }
    }
}