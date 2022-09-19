namespace map
{
    public class DetectFunctions
    {
        //Detect all values, and return array with coordinates
        public static int[,]? DetectAll<T>(T value, T[,] array)
        {
            int capX = array.GetLength(0);
            int capY = array.GetLength(1);
            int iteration = 0;
            int[,] newArray0 = new int[capX * capY, 2];
            for (int i = 0; i < capX; i++)
            {
                for (int j = 0; j < capY; j++)
                {
                    if (Equals(array[i, j], value))
                    {
                        newArray0[iteration, 0] = i;
                        newArray0[iteration, 1] = j;
                        iteration++;
                    }

                }
            }
            int[,] newArray = new int[iteration, 2];
            for (int i = 0; i < iteration; i++)
            {
                newArray[i, 0] = newArray0[i, 0];
                newArray[i, 1] = newArray0[i, 1];
            }
            if (iteration != 0) { return newArray; }
            Console.WriteLine("Something wrong in DetectAll()");
            return null;
        }
        //Detect AROUND *value* in specific coordinates
        public bool DetectAround<T>(int x, int y, T value, T[,] array)
        {
            for (int i = x - 1; i < x + 2; i++)
            {
                for (int j = y - 1; j < y + 2; j++)
                {

                    if (((i == x) && (j == y)) || (i < 0) || (j < 0))
                    {
                        continue;
                    }
                    try
                    {
                        if (Equals(array[i, j], value))
                        {
                            return true;
                        };
                    }
                    catch { };
                }
            }
            return false;
        }
        //Detect NEAR *value* in specific coordinates
        public static bool DetectNear<T>(int x, int y, T value, T[,] array)
        {
            for (int i = x - 1; i < x + 2; i++)
            {
                for (int j = y - 1; j < y + 2; j++)
                {

                    if (((i == x) && (j == y)) || ((i != x) && (j != y)) || (i < 0) || (j < 0))
                    {
                        continue;
                    }
                    try
                    {
                        if (Equals(array[i, j], value))
                        {
                            return true;
                        };
                    }
                    catch { };
                }
            }
            return false;
        }
        /*
        AROUND detect *value* in *array*
        Return true if detect, else false
        reverse: convert output
        */
        public bool DetectAroundAll<T>(bool reverse, T value, T valueAround, T[,] array)
        {
            bool detected = false;
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (Equals(array[i, j], value))
                    {
                        detected = DetectNear(i, j, valueAround, array);
                        if (detected)
                        {
                            if (reverse) { return false; }
                            return true;
                        }
                    }
                }
            }
            if (reverse) { return true; };
            return false;
        }
        /*
        NEAR detect *value* in *array*
        Return true if detect, else false
        reverse: convert output
        */
        public static bool DetectNearAll<T>(bool reverse, T value, T valueNear, T[,] array)
        {
            bool detected = false;
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (Equals(array[i, j], value))
                    {
                        detected = DetectNear(i, j, valueNear, array);
                        if (detected)
                        {
                            if (reverse) { return false; }
                            return true;
                        }
                    }
                }
            }
            if (reverse) { return true; };
            return false;
        }
        //If *array1* equals *array2*: return true, else: return false
        public static bool ArrayEquals<T>(T[,] array1, T[,] array2)
        {
            int x1 = array1.GetLength(0);
            int y1 = array1.GetLength(1);
            if ((x1 != array2.GetLength(0)) || (y1 != array2.GetLength(1))) { return false; }
            for (int i = 0; i < x1; i++)
            {
                for (int j = 0; j < y1; j++)
                {
                    if (!Equals(array1[i, j], array2[i, j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
