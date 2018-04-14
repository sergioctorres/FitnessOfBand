using Microsoft.Band;
using Microsoft.Band.Sensors;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FitnessOfBand
{
    public sealed partial class MainPage : Page
    {
        private WebService.FitnessOfBandSoapClient fitnessOfBand = new WebService.FitnessOfBandSoapClient();
        private IBandClient _bandClient = null;
        private DTO.Data data = new DTO.Data();
        private DTO.Data realtime = new DTO.Data();
        DispatcherTimer timer = new DispatcherTimer();
        int seconds = 0;
        int minute = 0;
        int hour = 0;
        bool realTime = true;

        public MainPage()
        {
            this.InitializeComponent();
            btnStop.IsEnabled = false;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;

            // Inicializando o Data Transfer Object
            data.Date = new DTO.DateInfo { };
            data.HeartRate = new DTO.IntInfo { };
            data.Speed = new DTO.DoubleInfo { };
            data.Pace = new DTO.DoubleInfo { };
            data.Distance = new DTO.LongInfo { };
            data.Step = new DTO.LongInfo { };
            data.Calories = new DTO.LongInfo { };

            realtime.Date = new DTO.DateInfo { };
            realtime.HeartRate = new DTO.IntInfo { };
            realtime.Speed = new DTO.DoubleInfo { };
            realtime.Pace = new DTO.DoubleInfo { };
            realtime.Step = new DTO.LongInfo { };
            realtime.Distance = new DTO.LongInfo { };
            realtime.Calories = new DTO.LongInfo { };
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e) { }

        private void Timer_Tick(object sender, object e)
        {
            seconds += 1;
            if (seconds == 60)
            {
                seconds = 0;
                minute += 1;
            }
            if (minute == 60)
            {
                minute = 0;
                hour += 1;
            }
            txtTimer.Text = string.Format("{0}:{1}:{2}", hour.ToString("00"), minute.ToString("00"), seconds.ToString("00"));
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            btnReports.IsEnabled = false;
            bool firstTime = true;
            seconds = 0;
            minute = 0;
            hour = 0;
            txtTimer.Text = "00:00:00";
            Sensor5Display.Text = "Iniciando...";

            // Instanciando um "client" da Microsoft Band para realizar conexão
            var bands = await BandClientManager.Instance.GetBandsAsync();

            if (_bandClient == null) _bandClient = await BandClientManager.Instance.ConnectAsync(bands.First());

            var uc = _bandClient.SensorManager.HeartRate.GetCurrentUserConsent();
            bool isConsented = false;

            if (uc == UserConsent.NotSpecified)
            {
                isConsented = await _bandClient.SensorManager.HeartRate.RequestUserConsentAsync();
            }
            if (isConsented || uc == UserConsent.Granted)
            {
                while (realTime == true)
                {
                    // Batimentos Cardíacos em tempo-real
                    int initialHeartRate = 0;
                    _bandClient.SensorManager.HeartRate.ReadingChanged += async (obj, ev) =>
                    {
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            initialHeartRate = ev.SensorReading.HeartRate;
                            txtInfoHeartRate.Text = ev.SensorReading.HeartRate.ToString();
                            realtime.HeartRate.Initial = ev.SensorReading.HeartRate;
                        });
                    };
                    await _bandClient.SensorManager.HeartRate.StartReadingsAsync();
                    await Task.Delay(TimeSpan.FromSeconds(1.5));

                    // Informações de Distancia
                    double initialSpeed = 0;
                    double initialPace = 0;
                    long initialDistance = 0;
                    _bandClient.SensorManager.Distance.ReadingChanged += async (s, args) =>
                    {
                        IBandDistanceReading readings = args.SensorReading;
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            // Velocidade atual em cm/s
                            initialSpeed = readings.Speed;

                            // Ritmo atual em ms/m
                            initialPace = readings.Pace;

                            // Distancia total desde o último reset de fábrica
                            initialDistance = readings.TotalDistance;
                            txtInfoDistance.Text = (readings.TotalDistance / 100).ToString() + "m";
                        });
                    };
                    await _bandClient.SensorManager.Distance.StartReadingsAsync();
                    await Task.Delay(TimeSpan.FromSeconds(1.5));
                    realtime.Speed.Initial = initialSpeed;
                    realtime.Pace.Initial = initialPace;
                    realtime.Distance.Initial = initialDistance;

                    // Informações de Passos Realizados
                    long initialStep = 0;
                    _bandClient.SensorManager.Pedometer.ReadingChanged += async (s, args) =>
                    {
                        IBandPedometerReading readings = args.SensorReading;
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            // Total de passos desde o último reset de fábrica
                            initialStep = readings.TotalSteps;
                            txtInfoStep.Text = readings.TotalSteps.ToString();
                        });
                    };
                    await _bandClient.SensorManager.Pedometer.StartReadingsAsync();
                    await Task.Delay(TimeSpan.FromSeconds(1.5));
                    realtime.Step.Initial = initialStep;

                    // Informações de Calorias
                    long initialCalories = 0;
                    _bandClient.SensorManager.Calories.ReadingChanged += async (s, args) =>
                    {
                        IBandCaloriesReading readings = args.SensorReading;
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            // Total de calorias desde o último reset de fábrica
                            initialCalories = readings.Calories;
                            txtInfoCalories.Text = readings.Calories.ToString();
                        });
                    };
                    await _bandClient.SensorManager.Calories.StartReadingsAsync();
                    await Task.Delay(TimeSpan.FromSeconds(1.5));
                    realtime.Calories.Initial = initialCalories;

                    // Guardando informações iniciais na DTO para gravar na tabela Information
                    while (firstTime == true)
                    {
                        data.Date.Initial = DateTime.Now;
                        data.HeartRate.Initial = initialHeartRate;
                        data.Speed.Initial = initialSpeed;
                        data.Pace.Initial = initialPace;
                        data.Distance.Initial = initialDistance;
                        data.Step.Initial = initialStep;
                        data.Calories.Initial = initialCalories;

                        Sensor5Display.Text = "Inicializado";
                        timer.Start();
                        btnStop.IsEnabled = true;
                        firstTime = false;
                    }

                    try
                    {
                        await fitnessOfBand.InsertRealTimeAsync(1, DateTime.Now, realtime.HeartRate.Initial, realtime.Speed.Initial, realtime.Pace.Initial, realtime.Distance.Initial, realtime.Step.Initial, realtime.Calories.Initial);
                    }
                    catch (Exception ex)
                    {
                        Sensor5Display.Text = "Sem conexão ao BD: " + ex.Message;
                    }
                }
            }
        }

        private async void btnStop_Click(object sender, RoutedEventArgs e)
        {
            btnStop.IsEnabled = false;
            realTime = false;
            timer.Stop();
            data.Date.Finished = DateTime.Now;
            Sensor5Display.Text = "Finalizando...";

            if (_bandClient != null)
            {
                // Batimentos Cardíacos em tempo-real
                int finalHeartRate = 0;
                _bandClient.SensorManager.HeartRate.ReadingChanged += async (obj, ev) =>
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        finalHeartRate = ev.SensorReading.HeartRate;
                    });
                };
                await _bandClient.SensorManager.HeartRate.StartReadingsAsync();
                await Task.Delay(TimeSpan.FromSeconds(1.5));
                data.HeartRate.Final = finalHeartRate;

                // Informações de Distancia
                double finalSpeed = 0;
                double finalPace = 0;
                long finalDistance = 0;
                _bandClient.SensorManager.Distance.ReadingChanged += async (s, args) =>
                {
                    IBandDistanceReading readings = args.SensorReading;
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        // Velocidade atual em cm/s
                        finalSpeed = readings.Speed;

                        // Ritmo atual em ms/m
                        finalPace = readings.Pace;

                        // Distancia total desde o último reset de fábrica
                        finalDistance = readings.TotalDistance;
                    });
                };
                await _bandClient.SensorManager.Distance.StartReadingsAsync();
                await Task.Delay(TimeSpan.FromSeconds(1.5));
                data.Speed.Final = finalSpeed;
                data.Pace.Final = finalPace;
                data.Distance.Final = finalDistance;

                // Informações de Passos Realizados
                long finalStep = 0;
                _bandClient.SensorManager.Pedometer.ReadingChanged += async (s, args) =>
                {
                    IBandPedometerReading readings = args.SensorReading;
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        // Total de passos desde o último reset de fábrica
                        finalStep = readings.TotalSteps;
                    });
                };
                await _bandClient.SensorManager.Pedometer.StartReadingsAsync();
                await Task.Delay(TimeSpan.FromSeconds(1.5));
                data.Step.Final = finalStep;

                // Informações de Calorias
                long finalCalories = 0;
                _bandClient.SensorManager.Calories.ReadingChanged += async (s, args) =>
                {
                    IBandCaloriesReading readings = args.SensorReading;
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        // Total de calorias desde o último reset de fábrica
                        finalCalories = readings.Calories;
                    });
                };
                await _bandClient.SensorManager.Calories.StartReadingsAsync();
                await Task.Delay(TimeSpan.FromSeconds(1.5));
                data.Calories.Final = finalCalories;

                try
                {
                    await fitnessOfBand.InsertInformationAsync(1, data.Date.Initial, data.Date.Finished, data.HeartRate.Initial, data.HeartRate.Final, data.Distance.Initial, data.Distance.Final, data.Step.Initial, data.Step.Final, data.Calories.Initial, data.Calories.Final);
                    Sensor5Display.Text = "Finalizado";
                }
                catch (Exception ex)
                {

                    Sensor5Display.Text = "Sem conexão ao BD: " + ex.Message;
                }

                btnReports.IsEnabled = true;
                btnStart.IsEnabled = true;
            }
        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Reports));
        }

    }
}