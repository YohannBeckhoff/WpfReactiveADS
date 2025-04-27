using System.Net.Sockets;
using System.Reactive;
using System.Reactive.Linq;        // Pour .Take(), .Subscribe(), etc.
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwinCAT.Ads;
using TwinCAT.Ads.Reactive; 

namespace WpfReactiveADS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AdsClient client;
        private IDisposable subscription;
        private bool isClosing = false;



        public MainWindow()
        {
            InitializeComponent();
            StartObserver();
        }
        private void StartObserver()
        {
            try
            {
                client = new AdsClient();
                client.Connect(AmsNetId.Local, 851);

                // Création de l'observer
                var valueObserver = Observer.Create<ushort>(val =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (ValuesListBox.Items.Count >= 12)
                        {
                            ValuesListBox.Items.RemoveAt(0); // Supprime le plus ancien
                        }
                        ValuesListBox.Items.Add($"Requete reçue : {val}");
                        Write1024String(val);
                    });
                });

                // Souscription à la variable
                var fastSettings = new NotificationSettings(
                    AdsTransMode.Cyclic, // mode de transmission
                    10,                  // cycleTime en millisecondes
                    0                    // maxDelay en millisecondes
                );


                subscription = client
                    .WhenNotification<ushort>("Main.NumRequete", fastSettings)
                   // .Take(20)
                    .Subscribe(valueObserver);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
        }
        private void Write1024String(ushort val)
        {
            try
            {
                // Créer une chaîne de 1024 caractères basée sur la valeur reçue
                string baseText = $"Valeur reçue : {val}";
                string paddedText = baseText.PadRight(1024, '.').Substring(0, 1024);

                // Écriture dans la variable PLC
                uint handle = client.CreateVariableHandle("Main.s1024String");
                client.WriteAnyString(handle, paddedText,1024, Encoding.ASCII);
            }
            catch (Exception ex)
            {
                // Évite le crash si la variable n'est pas trouvée
                Console.WriteLine($"Erreur Write1024String : {ex.Message}");
            }
        }
        private void Quitter_Click(object sender, RoutedEventArgs e)
        {
             Close();
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            isClosing = true; 
            try
            {
                subscription?.Dispose();
                System.Threading.Thread.Sleep(1000); // Pause synchrone Laisse finir les éventuelles callbacks
                client?.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur à la fermeture : {ex.Message}");
            }
            base.OnClosing(e);
        }

    }
}