using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks_Lab1
{
    class nn3S
    {
        double[,] wih, who;     //erstellt zwei zweidimensionale Arrays vom Typ double mit den Namen wih und who.
        int inodes, hnodes, onodes; // Erstell 3 integer einmal die input-Layer die hidden-Layer und die output-Layer 
        double[] hidden_inputs;    // Ein Array der die eingabewerte der einzelnen Hidden-Neuronen speichert 
        double[] hidden_outputs;   // Ein Array der die Augabewerte der einzelnen Hidden-Neuronen speichert 
        double[] final_inputs;     // Ein Array der die Eingabewerte der einzelnen Output-Neuronen speichert 
        double[] final_outputs;     // Ein Array der die Ausgabewerte  der einzelnen Output-Neuronen speichert 



        // Die dazugehörigen Properties der obigen Variablen , damit man von außen darauf zugreifen kann 
        public double[] Hidden_inputs { get { return hidden_inputs; } }
        public double[] Hidden_outputs { get { return hidden_outputs; } }
        public double[] Final_inputs { get { return final_inputs; } }
        public double[] Final_outputs { get { return final_outputs; } }

        public nn3S(int inodes, int hnodes, int onodes) // Der Konstruktor erwartet drei integer und zwar duíeANZAHL DER NEURONEN VON DEN Input-Hidden und Output- Layer 
        {

            // Initalisiert der anzahl der neuronen mit den Globalen Variablen 
            this.inodes = inodes;
            this.hnodes = hnodes;
            this.onodes = onodes;

            createWeightMatrizes();
        }

        private void createWeightMatrizes()
        {
            wih = new double[inodes, hnodes]; // Gewichtung zwischen Inputlayer und Hidden - lAyer    [Anzahl der Input Neuronen , Anzahl der Hidden ]
            who = new double[hnodes, onodes]; // Gewichtung zwischen Hidden layer und Output layer    [Anzahl der Hidden Neuronen , Anzahl der Output Neuronen]


            // Hardcoding der einzelnen Gewichtungen zwischen den einzelnen Pfaden zwischen Input und Hidden Neuronen 
            wih[0, 0] = 0.9;
            wih[1, 0] = 0.3;
            wih[2, 0] = 0.4;
            wih[0, 1] = 0.2;
            wih[1, 1] = 0.8;
            wih[2, 1] = 0.2;
            wih[0, 2] = 0.1;
            wih[1, 2] = 0.5;
            wih[2, 2] = 0.6;

            //Hardcoding der einzelnen Gewichtung zwischen einzelnen Pfaden zwischen Hidden und Outout Neuronen 
            who[0, 0] = 0.3;
            who[1, 0] = 0.7;
            who[2, 0] = 0.5;
            who[0, 1] = 0.6;
            who[1, 1] = 0.5;
            who[2, 1] = 0.2;
            who[0, 2] = 0.8;
            who[1, 2] = 0.1;
            who[2, 2] = 0.9;



            //Dynamisches Coding 
            /* for (int j = 0; j < hnodes; j++) // For schleife die solange ist wie die Neuronen Im Hidden Layer 
                 for (int i = 0; i < inodes; i++) // For Schleife die solamnge ist wie die Neuronen im Input LAyer 
                 {
                     System.Random weight_ih = new System.Random(); // erstellt eine neue Instanz der Klasse System.Random, die für die Erzeugung von Zufallszahlen verwendet wird.


                     wih[i, j] = weight_ih.NextDouble() - 0.5; //weight_ih.NextDouble() generiert eine Zufallszahl zwischen 0.0 und 1.0.
                                                               //-0.5 verschiebt die Zufallszahl in den Bereich von - 0.5 bis 0.5, da die Gewichte im neuronalen Netzwerk typischerweise kleine Werte um 0 herum haben sollen.

                     //Console.WriteLine("i: " + i + ", j: " + j + ", w: " + wih[i, j].ToString());
                 }
             for (int j = 0; j < onodes; j++)
                 for (int i = 0; i < hnodes; i++)
                 {
                     System.Random weight_ho = new System.Random();
                     who[i, j] = weight_ho.NextDouble() - 0.5;
                     //Console.WriteLine("i: " + i + ", j: " + j + ", w: " + who[i, j].ToString());
                 }*/
        }

        public void queryNN(double[] inputs)
        {
            nnMath nnMathO = new nnMath(); // Eine Instanz der KLasse nnMAth wird erstellt 

            hidden_inputs = new double[hnodes];//  es wird ein Double erstellt der länge der Hidden _ Neuronen  für die Eingabewerte 
            hidden_inputs = nnMathO.matrixMult(wih, inodes, inputs); // MNethode für die Berechnung der Eingabewerte , der die Gewichte , Anzahl der Input - Neuronen und die inputs brauch 

            hidden_outputs = new double[hnodes];   // Es wird ein Double benutzt der länge Hidden_Neuronen für die Ausgabewerte 
            hidden_outputs = nnMathO.activationFunction(hidden_inputs);

            final_inputs = new double[onodes];   // es wird ein Double benutzt der länge Outputs-Neuronen für die Eingabewerte 
            final_inputs = nnMathO.matrixMult(who, hnodes, hidden_outputs);

            final_outputs = new double[hnodes];  // es wird ein double erstellt der länge Outputs-Neuronen für die Ausgabewerte 
            final_outputs = nnMathO.activationFunction(final_inputs);
        }
    }
}



