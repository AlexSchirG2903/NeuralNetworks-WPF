using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace NeuralNetworks_Lab1
{
    class nn3S
    {
        // Gewichts-Matrizen
        double[,] wih, who; // Input-to-Hidden und Hidden-to-Output Gewichtungen

        // Anzahl der Neuronen in den Schichten
        int inodes, hnodes, onodes;

        // Eingabe-, Hidden- und Output-Werte
        double[] hidden_inputs;    // Eingabewerte der Hidden Layer
        double[] hidden_outputs;   // Ausgabewerte der Hidden Layer
        double[] final_inputs;     // Eingabewerte der Output Layer
        double[] final_outputs;    // Ausgabewerte der Output Layer

        // Fehlerwerte
        double[] hidden_errors;    // Fehler der Hidden Layer
        double[] output_errors;    // Fehler der Output Layer

        // Lernrate
        double learningRate;

        // Instanz der nnMath-Klasse für mathematische Operationen
        nnMath nnMathO = new nnMath();

        // Properties für den Zugriff von außen
        public double[] Hidden_inputs { get { return hidden_inputs; } }
        public double[] Hidden_outputs { get { return hidden_outputs; } }
        public double[] Final_inputs { get { return final_inputs; } }
        public double[] Final_outputs { get { return final_outputs; } }

        public double[] Hidden_errors { get { return hidden_errors; } }
        public double[] Output_errors { get { return output_errors; } }
        public double[,] WIH { get { return wih; } }
        public double[,] WHO { get { return who; } }

        // Konstruktor der Klasse
        public nn3S(int inodes, int hnodes, int onodes, double learningRate)
        {
            this.inodes = inodes;
            this.hnodes = hnodes;
            this.onodes = onodes;
            this.learningRate = learningRate;

            // Initialisierung der Gewichtsmatrizen und Arrays
            hidden_inputs = new double[hnodes];
            hidden_outputs = new double[hnodes];
            final_inputs = new double[onodes];
            final_outputs = new double[onodes];
            hidden_errors = new double[hnodes];
            output_errors = new double[onodes];

            createWeightMatrizes();
        }

        // Initialisierung der Gewichtsmatrizen
        private void createWeightMatrizes()
        {
            wih = new double[inodes, hnodes];
            who = new double[hnodes, onodes];

            // Eine einzige Instanz von Random erzeugen
            Random random = new Random();

            // Gewichte für Input-Hidden-Schicht initialisieren
            for (int j = 0; j < hnodes; j++)
            {
                for (int i = 0; i < inodes; i++)
                {
                    wih[i, j] = random.NextDouble() * 2.0 - 1.0; // Werte im Bereich [-1.0, 1.0]
                }
            }

            // Gewichte für Hidden-Output-Schicht initialisieren
            for (int j = 0; j < onodes; j++)
            {
                for (int i = 0; i < hnodes; i++)
                {
                    who[i, j] = random.NextDouble() * 2.0 - 1.0; // Werte im Bereich [-1.0, 1.0]
                }
            }
        }

        // Berechnung der Vorwärtsausbreitung
        public void queryNN(double[] inputs)
        {
            // Berechnung der Eingabewerte der Hidden Layer
            hidden_inputs = nnMathO.matrixMult(wih, inodes, inputs);

            // Berechnung der Ausgabewerte der Hidden Layer
            hidden_outputs = nnMathO.activationFunction(hidden_inputs);

            // Berechnung der Eingabewerte der Output Layer
            final_inputs = nnMathO.matrixMult(who, hnodes, hidden_outputs);

            // Berechnung der Ausgabewerte der Output Layer
            final_outputs = nnMathO.activationFunction(final_inputs);
        }

        // Trainingsmethode
        public void train(double[] inputs, double[] targets , double learningratetemp)


        {
            learningRate = learningratetemp; 
            
            // 1. Forward Propagation
            queryNN(inputs);

            // 2. Fehler im Output Layer berechnen
            output_errors = nnMathO.CalculateOutputErrors(targets, final_outputs);

            // 3. Fehler im Hidden Layer berechnen
            hidden_errors = nnMathO.CalculateHiddenError(who, output_errors);

            // 4. Gradienten für die Ausgabeschicht berechnen
            double[] outputGradients = new double[onodes];
            for (int i = 0; i < onodes; i++)
            {
                outputGradients[i] = output_errors[i] * final_outputs[i] * (1 - final_outputs[i]);
            }

            // 5. Gradienten für die versteckte Schicht berechnen
            double[] hiddenGradients = new double[hnodes];
            for (int i = 0; i < hnodes; i++)
            {
                hiddenGradients[i] = hidden_errors[i] * hidden_outputs[i] * (1 - hidden_outputs[i]);
            }

            // 6. Gewichtsanpassung für Hidden-to-Output
            for (int i = 0; i < hnodes; i++)
            {
                for (int j = 0; j < onodes; j++)
                {
                    who[i, j] += learningRate * outputGradients[j] * hidden_outputs[i];
                }
            }

            // 7. Gewichtsanpassung für Input-to-Hidden
            for (int i = 0; i < inodes; i++)
            {
                for (int j = 0; j < hnodes; j++)
                {
                    wih[i, j] += learningRate * hiddenGradients[j] * inputs[i];
                }
            }
        }
    }
}
