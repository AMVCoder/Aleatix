using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RandomGenerator
{
    public static class RandomPCG
    {
        private static ulong state = 0;
        private static ulong inc = 0;

        static RandomPCG()
        {
            // Inicializar la semilla y el incremento con valores aleatorios
            var random = new Random();
            state = (ulong)random.Next() << 32 | (ulong)random.Next();
            inc = (ulong)random.Next() << 32 | (ulong)random.Next();
        }

        internal static UInt32 NextUInt32()
        {
            UInt64 oldState = state;
            state = oldState * 6364136223846793005 + inc;
            UInt32 xorshifted = (UInt32)(((oldState >> 18) ^ oldState) >> 27);
            int rot = (int)(oldState >> 59);
            return (xorshifted >> rot) | (xorshifted << ((-rot) & 31));
        }


        /// <summary>
        /// Genera un valor booleano aleatorio.
        /// </summary>
        public static bool NextBool()
        {
            return NextUInt32() < (uint.MaxValue / 2);
        }


        /// <summary>
        /// Genera un número entero aleatorio en el rango [min, max).
        /// </summary>
        public static int NextInRange(int min, int max)
        {
            if (min >= max)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "El valor mínimo debe ser menor que el valor máximo.");
            }

            int range = max - min;
            uint random = NextUInt32();
            int scaled = (int)(random % (uint)range);
            return min + scaled;
        }

        /// <summary>
        /// Genera un número aleatorio de punto flotante en el rango [minValue, maxValue).
        /// </summary>
        public static float NextFloatRange(float minValue, float maxValue)
        {
            if (minValue >= maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue), "El valor mínimo debe ser menor que el valor máximo.");
            }

            float range = maxValue - minValue;
            uint random = NextUInt32();
            float randomFloat = random / (float)uint.MaxValue;
            return minValue + randomFloat * range;
        }

        /// <summary>
        /// Devuelve una cadena aleatoria de la lista de cadenas proporcionada.
        /// </summary>
        /// <param name="stringList">Lista de cadenas a elegir.</param>
        /// <returns>Una cadena aleatoria de la lista de cadenas proporcionada.</returns>
        public static string NextString(List<string> words)
        {
            if (words == null || words.Count == 0)
            {
                throw new ArgumentException("La lista de palabras no puede ser nula o vacía.");
            }

            int randomIndex = NextInRange(0, words.Count);
            return words[randomIndex];
        }

        /// <summary>
        /// Mezcla aleatoriamente los elementos de una lista y devuelve la lista mezclada.
        /// </summary>
        public static List<T> Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            List<T> shuffledList = new List<T>(list);
            while (n > 1)
            {
                n--;
                int k = NextInRange(0, n + 1);
                T value = shuffledList[k];
                shuffledList[k] = shuffledList[n];
                shuffledList[n] = value;
            }
            return shuffledList;
        }

        /// <summary>
        /// Devuelve un elemento aleatorio de una lista.
        /// </summary>
        public static T PickRandom<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                throw new ArgumentException("La lista no puede ser nula o vacía.");
            }
            int randomIndex = NextInRange(0, list.Count);
            return list[randomIndex];
        }

        /// <summary>
        /// Simula el lanzamiento de un dado con el número de caras especificado.
        /// </summary>
        public static int RollDice(int sides)
        {
            if (sides <= 0)
            {
                throw new ArgumentOutOfRangeException("sides", "El número de lados debe ser mayor que cero.");
            }
            return NextInRange(1, sides + 1);
        }

    }
}
