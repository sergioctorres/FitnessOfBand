using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FitnessOfBand
{
    public sealed partial class Reports : Page
    {
        private DTO.Data data = new DTO.Data();
        private WebService.FitnessOfBandSoapClient fitnessOfBand = new WebService.FitnessOfBandSoapClient();

        public Reports()
        {
            this.InitializeComponent();
            RequestReport();
        }

        public async void RequestReport()
        {

            try
            {
                // Método do WebService para coletar os últimos 5 registros de informações
                var reports = await fitnessOfBand.GetLastInformationsAsync();

                // Lista para receber as informações requisitada no WebService
                List<DTO.Information> informations = new List<DTO.Information>();

                // Percorrendo a list obtida pelo WebService

                foreach (var item in reports.Body.GetLastInformationsResult)
                {
                    DTO.Information info = new DTO.Information
                    {
                        InitialHeartRate = item.InitialHeartRate,
                        FinalHeartRate = item.FinalHeartRate,

                        InitialDistance = item.InitialDistance,
                        FinalDistance = item.FinalDistance,

                        InitialSteps = item.InitialSteps,
                        FinalSteps = item.FinalSteps,

                        InitialCalories = item.InitialCalories,
                        FinalCalories = item.FinalCalories,

                        InitialDateTime = item.InitialDateTime,
                        FinishedDateTime = item.FinishedDateTime
                    };
                    informations.Add(info);
                }

                // Report - R1
                txtDistanceR1.Text = "Distância";
                txtStepR1.Text = "Passos";
                txtCaloriesR1.Text = "Calorias";

                txtDateR1.Text = informations[0].InitialDateTime.ToString() + " - " + (informations[0].FinishedDateTime - informations[0].InitialDateTime).ToString();
                txtInfoHeartRateR1.Text = ((informations[0].InitialHeartRate + informations[0].FinalHeartRate) / 2).ToString();
                txtInfoDistanceR1.Text = ((informations[0].FinalDistance - informations[0].InitialDistance) / 100).ToString() + "m";
                txtInfoStepR1.Text = (informations[0].FinalSteps - informations[0].InitialSteps).ToString();
                txtInfoCaloriesR1.Text = (informations[0].FinalCalories - informations[0].InitialCalories).ToString();

                // Report - R2
                txtDistanceR2.Text = "Distância";
                txtStepR2.Text = "Passos";
                txtCaloriesR2.Text = "Calorias";

                txtDateR2.Text = informations[1].InitialDateTime.ToString() + " - " + (informations[1].FinishedDateTime - informations[1].InitialDateTime).ToString();
                txtInfoHeartRateR2.Text = ((informations[1].InitialHeartRate + informations[1].FinalHeartRate) / 2).ToString();
                txtInfoDistanceR2.Text = ((informations[1].FinalDistance - informations[1].InitialDistance) / 100).ToString() + "m";
                txtInfoStepR2.Text = (informations[1].FinalSteps - informations[1].InitialSteps).ToString();
                txtInfoCaloriesR2.Text = (informations[1].FinalCalories - informations[1].InitialCalories).ToString();

                // Report - R3
                txtDistanceR3.Text = "Distância";
                txtStepR3.Text = "Passos";
                txtCaloriesR3.Text = "Calorias";

                txtDateR3.Text = informations[2].InitialDateTime.ToString() + " - " + (informations[2].FinishedDateTime - informations[2].InitialDateTime).ToString();
                txtInfoHeartRateR3.Text = ((informations[2].InitialHeartRate + informations[2].FinalHeartRate) / 2).ToString();
                txtInfoDistanceR3.Text = ((informations[2].FinalDistance - informations[2].InitialDistance) / 100).ToString() + "m";
                txtInfoStepR3.Text = (informations[2].FinalSteps - informations[2].InitialSteps).ToString();
                txtInfoCaloriesR3.Text = (informations[2].FinalCalories - informations[2].InitialCalories).ToString();

                // Report - R4
                txtDistanceR4.Text = "Distância";
                txtStepR4.Text = "Passos";
                txtCaloriesR4.Text = "Calorias";

                txtDateR4.Text = informations[3].InitialDateTime.ToString() + " - " + (informations[3].FinishedDateTime - informations[3].InitialDateTime).ToString();
                txtInfoHeartRateR4.Text = ((informations[3].InitialHeartRate + informations[3].FinalHeartRate) / 2).ToString();
                txtInfoDistanceR4.Text = ((informations[3].FinalDistance - informations[3].InitialDistance) / 100).ToString() + "m";
                txtInfoStepR4.Text = (informations[3].FinalSteps - informations[3].InitialSteps).ToString();
                txtInfoCaloriesR4.Text = (informations[3].FinalCalories - informations[3].InitialCalories).ToString();

                // Report - R5
                txtDistanceR5.Text = "Distância";
                txtStepR5.Text = "Passos";
                txtCaloriesR5.Text = "Calorias";

                txtDateR5.Text = informations[4].InitialDateTime.ToString() + " - " + (informations[4].FinishedDateTime - informations[4].InitialDateTime).ToString();
                txtInfoHeartRateR5.Text = ((informations[4].InitialHeartRate + informations[4].FinalHeartRate) / 2).ToString();
                txtInfoDistanceR5.Text = ((informations[4].FinalDistance - informations[4].InitialDistance) / 100).ToString() + "m";
                txtInfoStepR5.Text = (informations[4].FinalSteps - informations[4].InitialSteps).ToString();
                txtInfoCaloriesR5.Text = (informations[4].FinalCalories - informations[4].InitialCalories).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e) { }
    }
}
