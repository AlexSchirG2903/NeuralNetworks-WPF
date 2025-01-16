using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NeuralNetworks_Lab1
{

    
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int inodes = 3, hnodes = 3, onodes = 3;
        nn3S nn3SO;  //
        double[] inputs; //  Referenz auf ein double-Array deklariert, aber noch kein Speicherplatz dafür reserviert
        bool createButton_zu_queryButton = false; // Variable die dafür sorgt, dass zuerst create Burron gedrückt wird , bevor man queryButton druckt , um keine exeptioin zu generieren 

        public MainWindow()
        {
            InitializeComponent();
        }

        private void inputTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Methode dazu da das man nur ganzzahlen eingeben kann und keine Buchstabe 
            e.Handled = !int.TryParse(e.Text, out int inodes);
            Console.WriteLine("input : " + inodes);
        }

        private void hiddenTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {// Methode dazu da das man nur ganzzahlen eingeben kann und keine Buchstabe 
            e.Handled = !int.TryParse(e.Text, out int hnodes);
        }

        private void outputTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {// Methode dazu da das man nur ganzzahlen eingeben kann und keine Buchstabe 
            e.Handled = !int.TryParse(e.Text, out int onodes);
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        { // Methode dazu da um zu guckken , dass die 3 Layers : "input" , "hidden", "output" nicht aus 0 Neuronen bestehen 
            if ((inodes != 0) && (hnodes != 0) && (onodes != 0))
                nn3SO = new nn3S(inodes, hnodes, onodes); // Eine Instanz der Klasse nn3S wird erstellt mit dem Konstruktor der die 3 int als eingabewert erwartet 
            
            createButton_zu_queryButton = true; 
        }

        private void queryButton_Click(object sender, RoutedEventArgs e)
        {
            if (createButton_zu_queryButton)
            { // if Bedingung die schaut, ob createButton gedrückt wurde, um keine exeptioin zu generieren , da man ansonsten auf eine ungültige Instanz zugreift 
                int i, j, k; // Neu int ich weiß noch nicht wieso 
                inputs = new double[inodes]; // jetzt wird Speicherplatzt reserviert der Variable inputs  mit der länge inodes 

                // Sind die Eingangswerte die ich den Input neuronen gebe 

                inputs[0] = 0.9;
                inputs[1] = 0.1;
                inputs[2] = 0.8;

                nn3SO.queryNN(inputs);


                for (int a = 0; a < inputs.Length; a++) // Irritiert duch die länge des inputs , welches ein eindimensionales Array ist 
                {

                    // Hier wird eine Instanz vom Objekt nodeRow erstellt und die geschfeiften Klammern sagen aus das wir sofort die variablen in der instanz bearbeiten ....
                    //... können ohne zum Beispiel so zu Initalisieren : "data.inputHidden 
                    var data = new nodeRow
                    { // Wird durch ein , und nicht durch ; getrennt weil die zuweisung als Liste von Zuweisung innerhalb der geschweiften Klammern sind 

                        inputValue = inputs[a].ToString(),  // Speichert den Eingabewert de Input Neuron als String 

                        weightsIH = "0",    // Die gewichtung zwischen Inpur und Hidden 

                        inputHidden = String.Format(" {0:0.##} ", nn3SO.Hidden_inputs[a]),  // Hier werden die Hidden Eingabewerte angezeigt   -> {0:0.##} das heißt 2 nachkommerstellen werden angezeigt 

                        outputHidden = String.Format(" {0:0.##} ", nn3SO.Hidden_outputs[a]), // Hier werden die berechnetetn Ausgabewerte des Hidden layer angezeigt

                        weightsHO = "0",  // Die Gewichtung zwischen Hidden und Output 

                        errorHidden = "0", //Diese Eigenschaft ist vermutlich für den Fehler des Hidden Layers vorgesehen !!!Benutzt später den error Output , für das training 

                        inputOutput = String.Format(" {0:0.##} ", nn3SO.Final_inputs[a]), //Hier werden die Output Eingabewerte angezeigt
                                                                                          //
                        outputLayer = String.Format(" {0:0.##} ", nn3SO.Final_outputs[a]), // Hier werden die berechnetetn Ausgabewerte des Hidden layer angezeigt

                        target = "0", //Diese Eigenschaft scheint den Sollwert (Target) für das jeweilige Ausgabeneuron zu speichern

                        errorOutput = "0", //Diese Eigenschaft ist vermutlich für den Fehler des Output Layers vorgesehen wird mit dem target verrechnet um zu gucken , ob das ergebnis gut ist 
                    };


                    networkDataGrid.Items.Add(data);// wird verwendet, um die Liste im DataGrid mit den Daten zu füllen.
                }
            }
        }
    }

    public class nodeRow
    { // Diese Zeile erstellt intern automatisch eine private Variable
      //  Man muss nur extra private Variablen erstellen , wenn ich eine Logik in den Properties verwenden will . Beispiel ist darunter : 

        /*private string _inputValue;

            public string inputValue
            {
                    get { return _inputValue; }
                    set
                        {
                             if (!string.IsNullOrEmpty(value))
                                 {
                                     _inputValue = value;
                                 }
                        }
            }*/
        public string inputValue { get; set; }

        public string weightsIH { get; set; }
        public string inputHidden { get; set; }
        public string outputHidden { get; set; }
        public string weightsHO { get; set; }
        public string errorHidden { get; set; }
        public string inputOutput { get; set; }
        public string outputLayer { get; set; }
        public string target { get; set; }
        public string errorOutput { get; set; }
    }
}


