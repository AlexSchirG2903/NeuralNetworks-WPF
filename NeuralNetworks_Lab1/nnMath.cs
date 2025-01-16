using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks_Lab1
{
    class nnMath
    {

        public double[] matrixMult(double[,] gewichtung, int anzahl_neuronen, double[] Eingabewerte)
        {
            double[] Eingangsergebnis = new double[anzahl_neuronen];
            for (int i = 0; i < anzahl_neuronen; i++)
            {
                for (int j = 0; j < gewichtung.GetLength(1); j++)
                {
                    Eingangsergebnis[i] += Eingabewerte[j] * gewichtung[j, i];

                }


            }


            return Eingangsergebnis;
        }

        public double[] activationFunction(double[] inputs)
        {
            double[] ausgabewerte_sigmoid_Funktion = new double[inputs.Length];

            for (int i = 0; i < inputs.Length; i++)
            {
                ausgabewerte_sigmoid_Funktion[i] = 1 / (1 + Math.Exp(-inputs[i]));
            }

            return ausgabewerte_sigmoid_Funktion;
        }
    }
}
