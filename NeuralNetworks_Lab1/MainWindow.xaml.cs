using System;
using System.IO;
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
using Microsoft.Win32;
using System.Drawing;

namespace NeuralNetworks_Lab1
{


    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // VAriablen 

        int inodes = 784; //784 Neuronen im Input Layer, weil wir 28 * 28 Pixel haben also für jeden Pixel ein Neuron 
        int hnodes = 200; //Anzahl der Neuronen im Hidden-Layer. Hier wurden 200 gewählt, was typisch für ein neuronales Netz ist, da es oft als ausreichend leistungsfähig für die Verarbeitung von Eingabedaten gilt.
        int onodes = 10;//Anzahl der Neuronen im Output-Layer. Der Wert 10 entspricht der Anzahl der Klassen (z. B. Ziffern von 0 bis 9), die das Netzwerk vorhersagen kann.
        int trainCount = 0; // trainCount dient als Zähler, um die Gesamtanzahl der Trainingsdurchläufe (Iterationen) zu verfolgen. Jedes Mal, wenn ein Trainingsbeispiel verarbeitet wird, wird dieser Zähler um 1 erhöht.
        int epoches = 1; //Diese Variable gibt an, wie oft das gesamte Trainingsset durchlaufen werden soll. Eine Epoche bedeutet, dass das neuronale Netz jeden Datensatz im Trainingsset genau einmal verarbeitet hat. Wenn epoches auf 5 gesetzt wird, durchläuft das Netz den gesamten Trainingsdatensatz fünfmal.
        nn3S nn3SO;  // EIn Instanz von  der Klasse nn3S wird erstellt .  Es wird verwendet, um mit dem neuronalen Netz zu arbeiten (z. B. Training, Berechnungen). 
        double[] inputs; //  Referenz auf ein double-Array deklariert, aber noch kein Speicherplatz dafür reserviert . sind die Eingabewerte 
        double[] targets;//Referenz auf ein double-Array deklariert , aber auch kein speicher deklariert . Sind die Ziele die erreicht werden sollen.
        bool createButton_zu_queryButton = false; // Variable die dafür sorgt, dass zuerst create Burron gedrückt wird , bevor man queryButton druckt , um keine exeptioin zu generieren 

        double learningRate = 0.1;                      // VAriable die global ist und später den wert von dem TextBox annimmt für die Learning Rates 

        string trainFile; // Eine CSV Datei die das Netz trainieren soll welche Zahlen welchen Pixel entsrpicht 
        string testFile; // Die Datei hat Bilder die andere Pixel hat aber den gleichen Zahlen hat , um zu gucken wie gut das Neuronale Netz die Zahlen mit unbekannten werten erkennt 
        string weightFile; // Nach jedem Training werden die Gewichte Gespeichert , um eine Remanenz zu haben , damit man nicht immer von vorne trainerene muss 
        Boolean trainOK = false;// NAch abschluss aller Epochen sagt das Netz , ob es trainiert wurde . also gibt an wann es mit de´m training fertig ist 

        //########################################################################################################################################################

        public MainWindow()
        {
            InitializeComponent();
        }


        //########################################################################################################################################################
        // DIE FOLGENDEN METHODEN SIND PREVIEWTEXTINPU METHODEN -> MAN DARF NUR GANZZAHLEN EINGEBEN IN DEN TEXT BOXEN
        //########################################################################################################################################################
        private void learningRateTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {// Methode dazu da das man nur ganzzahlen eingeben kann und keine Buchstabe 

            // Erlaubt nur Zahlen und maximal einen Dezimalpunkt
            e.Handled = !IsValidLearningRateInput(e.Text, ((TextBox)sender).Text);
        }

        // Validierungsmethode
        private bool IsValidLearningRateInput(string newText, string currentText)
        {
            // Erlaubt nur Zahlen und einen Dezimalpunkt
            string combinedText = currentText + newText;
            return double.TryParse(combinedText, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out _);
        }

        //##########



        private void inputTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Methode dazu da das man nur ganzzahlen eingeben kann und keine Buchstabe 
            e.Handled = !int.TryParse(e.Text, out int inodes);
            Console.WriteLine("input : " + inodes);
        }
        //##########

        private void hiddenTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {// Methode dazu da das man nur ganzzahlen eingeben kann und keine Buchstabe 
            e.Handled = !int.TryParse(e.Text, out int hnodes);
        }
        //##########

        private void outputTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {// Methode dazu da das man nur ganzzahlen eingeben kann und keine Buchstabe 
            e.Handled = !int.TryParse(e.Text, out int onodes);
        }
        //############
        private void epochenBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Methode die nur Ganzahlen erlaubt um die Anzahl der epochen anzugeben 
            e.Handled = !int.TryParse(e.Text, out int epoches);
        }

        //############

        private void performanceBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        //#################################################################################
        // Methoden Wenn ich in den TExt Box Reinschreibe 
        //#################################################################################
        private void learningRateTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Double.TryParse(learningRateTextBox.Text, out double learingrateTemp);
            learningRate = learingrateTemp; // Übergibt die eingabe die ich getätigt habe meinen learning rate 
        }


        //########################################################################################################################################################
        //IN DEN FOLGENDEN METHODEN SIND FÜR DAS DRÜCKEN DER  BUTTONS 
        //########################################################################################################################################################

        private void createButton_Click(object sender, RoutedEventArgs e)
        { // Methode dazu da um zu guckken , dass die 3 Layers : "input" , "hidden", "output" nicht aus 0 Neuronen bestehen 
            if ((inodes != 0) && (hnodes != 0) && (onodes != 0))
                nn3SO = new nn3S(inodes, hnodes, onodes, learningRate); // Eine Instanz der Klasse nn3S wird erstellt mit dem Konstruktor der die 3 int als eingabewert erwartet 

            createButton_zu_queryButton = true; // Damit ich erstmal create Button und dann querry Button machen kann , ohne einen Exeptioin zu bekommen 
        }

        //##########

        private void trainButton_Click(object sender, RoutedEventArgs e)
        {
            /*  inputs = new double[inodes];
              targets = new double[onodes];
              inputs[0] = 0.9;
              inputs[1] = 0.1;
              inputs[2] = 0.8;
              targets[0] = 0.9;
              targets[1] = 0.9;
              targets[2] = 0.9;
              nn3SO.train(inputs, targets,learningRate);

              displayResults();*/

            int i, j, k; // Variablen die für die Schleifen verwendet werden 
            targets = new double[onodes]; // reserviert jetzt Speicherplatz für die targets mit der länge von onoeds -> also 10 weil wir zahlen von 0-9 erkennen wollen 

            for (j = 0; j < epoches; j++)  // Die schleife dient dazu , um die Netze der entsprechenden epochen zu trainieren 
                using (StreamReader sr = new StreamReader(trainFile)) // dafür da , dass ich die trainFile Datei öfnnen kann -> using-Block: Automatische Freigabe der Ressourcen (z. B. Schließen der Datei), sobald der Block verlassen wird.
                {
                    string line; // Nimmt die aktuelle zeile einer csv datei 
                    int intTarget; // ist für die erste Zeile in unserer csv datei 
                    while ((line = sr.ReadLine()) != null && (line != "")) // sr.ReadLine() liest die aktuelle Zeile aus der Datei.
                        //Die Schleife läuft, bis alle Zeilen verarbeitet wurden oder die Datei leer ist.
                    {
                        //  Console.WriteLine(line);                    
                        intTarget = readInputs(line); // trennt die Zeile durch das Kommer nimmt die Erste Zahl weil es den Wert und nicht die Pixel representiert und sklariert sie noch 
                        for (i = 0; i < onodes; i++) // läuft alle Output Neuronen (0-9) durch 
                            targets[i] = 0.01;  // setzt erstmal alle werte auf 1 % also das es nicht den Wert entspricht  den wir suchen 
                        targets[intTarget] = 0.99; // setzt unseren target Neuron an der stelle auf 99%
                        nn3SO.train(inputs, targets, learningRate); // Übergeben die inputs , die Targets und meine learningrate an die Methode der Trainings instanz 
                        trainCount++;                               // Erhöt nach jedem ablauf wie oft trainiert wird 
                        displayResults();                           // Zeig die Werte im data.Grid 
                        if (checkBoxImage.IsChecked == true)
                            MessageBox.Show("Next");
                        //MessageBox.Show("Next");                       
                    }
                }

            weightFile = string.Concat("weight-", trainCount.ToString(), "-",
                epoches.ToString(), "-", hnodes.ToString()); //  geneiret eine dateiname die so aussehen kann "weight-1000-10-200" erste präfix weight dann wie vieltrainiert wurde dann wie viele epochen und wie viel hidden neuronen es gibt 

            using (TextWriter sw = new StreamWriter(weightFile + ".txt")) //Ein TextWriter wird erstellt, um in eine Datei zu schreiben. Der Dateiname ist weightFile + ".txt", wobei weightFile vorher zusammengesetzt wurde, z. B. weight-1000-10-200.
            {
                // Console.WriteLine(nn3SO.WIH.GetLength(0).ToString(), nn3SO.WIH.GetLength(1).ToString());
                for (j = 0; j < nn3SO.WIH.GetLength(1); j++)
                {
                    for (i = 0; i < nn3SO.WIH.GetLength(0); i++)
                    {
                        sw.WriteLine(nn3SO.WIH[i, j].ToString());  // Schreibt die Gewicht zwischen  Input und Hidden Layer 
                        //  Console.WriteLine(nn3SO.WIH[i, j].ToString());
                    }
                    // sw.WriteLine();
                }
                for (j = 0; j < nn3SO.WHO.GetLength(1); j++)
                {
                    for (i = 0; i < nn3SO.WHO.GetLength(0); i++)
                        sw.WriteLine(nn3SO.WHO[i, j].ToString());  // Schreibt die Gewichte zwischen Hiden und Output layer in die datei 
                    // sw.WriteLine();
                }
            }
            trainOK = true; // Sagt das das training zuende ist 
            // displayResults();
            MessageBox.Show("Training done: " + trainCount);


        }

        //###################

        private void openTrainButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();// Neue Instant von OpenFile dialog die ermöglicht eine Datei auszuwählen 
            if (openFileDialog.ShowDialog() == true) // Durch show dialog kann man eine Datei auswählen 
                trainFile = openFileDialog.FileName; // Wird train file zugeführt 
        }
        //############
        private void loadWeightButton_Click(object sender, RoutedEventArgs e)
            //DMethode wird ausgeführt wenn ich auf Loadweight Button Klicke 
        {
            int i = 0, j = 0, counter = 0, k = 0, l = 0;
            string line;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                weightFile = openFileDialog.FileName;  // Ermöglich die Datei auszuwählen und als weightFile zu speichern 

            using (TextReader sr = new StreamReader(weightFile)) // Wird genutz um die Datei zu lesen 
            {
                while ((line = sr.ReadLine()) != null && (line != ""))
                {
                    if (j < hnodes)
                    {
                       // nn3SO.setWIHMatrix(i, j, Convert.ToDouble(line)); // Setzt die Gewichte aus der Tabelle 
                        i++; counter++;
                        if (i >= inodes) { j++; i = 0; }
                    }
                    else
                    {
                    //    nn3SO.setWHOMatrix(k, l, Convert.ToDouble(line));
                        k++;
                        if (k >= hnodes) { l++; k = 0; }
                    }
                }
            }
        }
        //####################
        private void openTestButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                testFile = openFileDialog.FileName; // Ermöglicht testfile zu öffnen 
        }







        //##########

        private void queryButton_Click(object sender, RoutedEventArgs e)
        {
            if (createButton_zu_queryButton)
            { // if Bedingung die schaut, ob createButton gedrückt wurde, um keine exeptioin zu generieren , da man ansonsten auf eine ungültige Instanz zugreift 

                int i;
                int scorecard = 0; //Diese Variable zählt, wie oft das neuronale Netz die richtige Antwort gibt.
                int testCounter = 0;//Diese Variable zählt die Anzahl der durchgeführten Tests
                targets = new double[onodes]; // Sagt das die target so lng sind wie die outputs layer , in unseren fall haben wir 10 
                using (StreamReader sr = new StreamReader(testFile)) // Liest die Test Datei 
                {
                    string line;    
                    int intTarget, indexAnswer = 10;
                    double answer = 0.0;
                    while ((line = sr.ReadLine()) != null && (line != "")) // geht solange durch bis es keine Zeilen mehr exestiert 
                    {
                        //Console.WriteLine(line);
                        answer = 0;
                        intTarget = readInputs(line);   // gibt den ersten wert der csv datei aus und skaliert sie 
                        for (i = 0; i < onodes; i++)   // Die schleife geht bis 10 also (0-9) Zahlen 
                            targets[i] = 0.01;
                        targets[intTarget] = 0.99;  // Setzt unser target nur auf 99 % 
                        nn3SO.queryNN(inputs);      // Ruft die Vorhersagen des neuronalen Netzes für die aktuellen Eingaben (inputs) ab
                        for (i = 0; i < nn3SO.Final_outputs.Length; i++) // Gibt die anzahl der final outputs 
                        {
                            if (nn3SO.Final_outputs[i] > answer) // Guckt ob Final Outputs (output nach sigmoid funktioin größer ist als answer )
                            {
                                answer = nn3SO.Final_outputs[i];  // Dann nimmt answer den wert des Final output an  um die größe wahrscheinlichkeit zu finden 
                                indexAnswer = i;                 // erhöt index answer , wenn es eine höhere antwort gibt 
                            }
                        }
                        if (intTarget == indexAnswer) // wenn 5 zumbeispiel auch den index 5 ausgibt dann weiß er das die antwort richtig ist 
                        {
                            scorecard++;  // Zaählt wie oft er die richtige antwort gegeben hat 
                            //  MessageBox.Show("Right Answer");
                        }
                        testCounter++; // Erhöht die anzahl der durchgeführten Test 
                        displayResults(); // Zeigt das Data Grid an 
                        if (checkBoxImage.IsChecked == true)
                            MessageBox.Show("Next");
                    }
                }
                performanceBox.Text = (scorecard / (double)testCounter).ToString(); // zeigt wie gut mein PROGRAMM IST IN PROZENT 
            }

        }

        private int readInputs(string line)
        {
            int i, j;
            inputs = new double[inodes]; // Sagt wie groß meine inouts sind im unseren fall 28*28 
            string[] input; // erstellt eine refernez eines Arrays ,Es wird verwendet, um die CSV-Zeile, die übergeben wurde, in einzelne Werte zu splitten.
            byte[] inputsByte = new byte[inodes]; // Ein Byte-Array inputsByte wird mit der Länge inodes erstellt. Es wird verwendet, um die Eingabewerte für die Bilddarstellung vorzubereiten.

            //Console.WriteLine(line);

            input = line.Split(','); // teilt die CSV datei bei jedem Kommer und speichert dies in ein Array 
            
            //Console.WriteLine("Size of elements in line " + input.Length);
            //Console.Write("input 0: " + input[0] + ", ");

            for (i = 1; i < input.Length; i++) // Geht die ganzen Pixel durch 
            {
                //Console.Write("input : " + input[i] + ", ");
                j = i - 1;
                inputs[j] = (Convert.ToDouble(input[i]) * 0.99 / 255.0) + 0.01; // skaliert die Bytes 0-255 zu 0.01 bis 1 um mit den daten besser zu arbeiten 
                inputsByte[j] = (byte)(Convert.ToInt32(input[i]));// Ein Byte-Array inputsByte wird mit der Länge inodes erstellt. Es wird verwendet, um die Eingabewerte für die Bilddarstellung vorzubereiten.
                //Console.Write("inputsByte : " + inputsByte[j] + ", ");
            }
            // Console.WriteLine(inputs);
            if (checkBoxImage.IsChecked == true)
            { // Erstellt ein Bild aus den Eingabedaten.
                BitmapSource img = BitmapSource.Create(28, 28, 96, 96, PixelFormats.Indexed8, BitmapPalettes.Gray256, inputsByte, 28);
                numberImage.Source = img;
                //MessageBox.Show("Next");
            }
            return (Convert.ToInt32(input[0]));
        }

        //########################################################################################################################################################
        private void displayResults()


        {

            {
                int weightIHsize = (int)(nn3SO.WIH.Length / inodes); // Gibt die Pfade der einzelnen Neuronen an zwischen Input und Hidden Neuronen
                int weightHOsize = (int)(nn3SO.WHO.Length / hnodes); // Gibt die Pfade der einzelnen Neuronen an zwischen Hidden und Output Neuronen
                networkDataGrid.Items.Clear(); // Leert das Grid vor dem Hinzufügen neuer Daten

                for (int i = 0; i < inodes; i++) // Iteriert durch die Anzahl der Input-Neuronen
                {
                    nodeRow data = new nodeRow(); // Erstellt ein neues Datenobjekt für das Grid
                    data.inputValue = inputs[i].ToString(); // Speichert den Eingabewert des Input-Neurons

                    if (i < hnodes)
                    {
                        data.inputHidden = String.Format(" {0:0.##} ", nn3SO.Hidden_inputs[i]); // Zeigt die Hidden Layer Eingabewerte mit 2 Dezimalstellen
                        data.outputHidden = String.Format(" {0:0.##} ", nn3SO.Hidden_outputs[i]); // Zeigt die Hidden Layer Ausgabewerte mit 2 Dezimalstellen
                                                                                                  // data.errorHidden = String.Format(" {0:0.##} ", nn3SO.Hidden_errors[i]); // Diese Zeile wurde deaktiviert und kann später ergänzt werden
                    }

                    if (i < onodes)
                    {
                        data.inputOutput = String.Format(" {0:0.##} ", nn3SO.Final_inputs[i]); // Zeigt die Output Layer Eingabewerte
                        data.outputLayer = String.Format(" {0:0.##} ", nn3SO.Final_outputs[i]); // Zeigt die berechneten Ausgabewerte der Output Layer
                        data.target = targets[i].ToString(); // Speichert den Sollwert (Target) für das jeweilige Output-Neuron
                                                             // data.errorOutput = String.Format(" {0:0.##} ", nn3SO.Output_errors[i]); // Diese Zeile wurde deaktiviert und kann später ergänzt werden
                    }

                    networkDataGrid.Items.Add(data); // Fügt die Datenzeile dem Grid hinzu
                }

                double maxfound = nn3SO.Final_outputs[0]; // Initialisiert die Suche nach dem höchsten Wert im Output
                int indexMax = 0;

                for (int i = 1; i < onodes; i++) // Durchläuft die Output-Werte, um das Maximum zu finden
                {
                    if (nn3SO.Final_outputs[i] > maxfound)
                    {
                        maxfound = nn3SO.Final_outputs[i];
                        indexMax = i;
                    }
                }

                recognizedBox.Text = indexMax.ToString(); // Zeigt den Index des höchsten Outputs an
            }
        }

    }


    //########################################################################################################################################################


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



