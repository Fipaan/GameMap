using static map.Map;
using static map.Enums;

namespace map
{
    public class CreateFunctions
    {
        //Create one object in random position
        public static void CreateOneAtRandom<T>(T input, ref T[,] array, bool isEmpty)
        {
            Random rnd = new();
            int capX = array.GetLength(0);
            int capY = array.GetLength(1);
            int x = rnd.Next(capX);
            int y = rnd.Next(capY);
            if (isEmpty)
            {
                while (!Equals(array[x, y], Blocks.Empty))
                {
                    x = rnd.Next(capX);
                    y = rnd.Next(capY);
                }
            }
            array[x, y] = input;
        }
        //Create *count* of objects at random position
        public static void CreateAtRandom<T>(T value, int count, ref T[,] array, bool isEmpty)
        {
            int countEmpty;
            if (isEmpty)
            {
                countEmpty = Counter(Blocks.Empty, (Blocks[,])System.Convert.ChangeType(array, typeof(Blocks[,])));
                if (countEmpty == 0) { return; }
                if (countEmpty < count) { count = countEmpty; }
            }
            for (int i = 0; i < count; i++)
            {
                CreateOneAtRandom(value, ref array, isEmpty);
            }
        }
        //Value-Dependent Object Creation Function (random)
        public static void FlexibleCreateAtRandom(Blocks type, int count, ref Blocks[,] array, bool isEmpty)
        {
            switch (type)
            {
                case Blocks.Enemy:
                    CreateAtRandom(Blocks.Enemy, count, ref array, isEmpty);
                    CreateAroundAll(Blocks.Enemy, Blocks.Death, ref array, isEmpty);
                    break;
                default:
                    CreateAtRandom(type, count, ref array, isEmpty);
                    break;
            }
        }
        //Create object in coordinate
        public static void FlexibleCreateByCoordinate(int x, int y, ref Blocks[,] array, Blocks value)
        {
            switch (value)
            {
                case Blocks.Enemy:
                    array[x, y] = value;
                    CreateAround(x, y, Blocks.Death, ref array, true);
                    break;
                default:
                    array[x, y] = value;
                    break;
            }
        }
        //AROUND all *value* create *valueAround*
        public static void CreateAroundAll<T>(T value, T valueAround, ref T[,] array, bool isEmpty)
        {
            int x = array.GetLength(0);
            int y = array.GetLength(1);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (Equals(array[i, j], value))
                    {
                        CreateAround(i, j, valueAround, ref array, isEmpty);
                    }
                }
            }
        }
        //AROUND position [x, y] create *value*
        public static void CreateAround<T>(int x, int y, T value, ref T[,] array, bool isEmpty)
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
                        if (Equals(array[i, j], Blocks.Empty) || !isEmpty)
                        {
                            array[i, j] = value;
                        }
                    }
                    catch { };
                }
            }
        }
        //NEAR of *value* create *valueAround*
        public static void CreateNearAll<T>(T value, T valueAround, ref T[,] array, bool isEmpty)
        {
            int x = array.GetLength(0);
            int y = array.GetLength(1);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (Equals(array[i, j], value) || !isEmpty)
                    {
                        CreateNear(i, j, valueAround, ref array, isEmpty);
                    }
                }
            }
        }
        //NEAR of position [x, y] create *value*
        public static void CreateNear<T>(int x, int y, T value, ref T[,] array, bool isEmpty)
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
                        if (Equals(array[i, j], Blocks.Empty) || !isEmpty)
                        {
                            array[i, j] = value;
                        }
                    }
                    catch { };
                }
            }
        }
        //Create object(s) at hollow rectangle at random position
        public static void CreateHollow(bool isAll, bool onlyEmpty, int[] inputCoordinates, Blocks value, ref Blocks[,] array, int wide = 1, int count = 0)
        {
            int minX = inputCoordinates[0];
            int minY = inputCoordinates[1];
            int capX = inputCoordinates[2];
            int capY = inputCoordinates[3];
            int XtoX = capX - minX;
            int YtoY = capY - minY;
            int[,] coordinates = new int[XtoX * YtoY, 2];
            int size = 0;
            int random;
            bool checkX(int i)
            {
                return ((wide > i) || ((XtoX - i) < (wide + 1)));
            }
            bool checkY(int j)
            {
                return ((wide > j) || ((YtoY - j) < (wide + 1)));
            }
            if (isAll)
            {
                if (onlyEmpty)
                {
                    for (int i = minX; i < capX; i++)
                    {
                        for (int j = minY; j < capY; j++)
                        {
                            if (Equals(array[i, j], Blocks.Empty))
                            {
                                FlexibleCreateByCoordinate(i, j, ref array, value);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = minX; i < capX; i++)
                    {
                        for (int j = minY; j < capY; j++)
                        {
                            FlexibleCreateByCoordinate(i, j, ref array, value);
                        }
                    }
                }
                return;
            }
            if (onlyEmpty)
            {
                for (int i = minX; i < capX; i++)
                {
                    for (int j = minY; j < capY; j++)
                    {
                        if (Equals(array[i, j], Blocks.Empty) && ((checkX(i)) || (checkY(j))))
                        {
                            coordinates[size, 0] = i;
                            coordinates[size, 1] = j;
                            size++;
                        }
                    }
                }
            }
            else
            {
                for (int i = minX; i < capX; i++)
                {
                    for (int j = minY; j < capY; j++)
                    {
                        if (checkX(i) || checkY(j))
                        {
                            coordinates[size, 0] = i;
                            coordinates[size, 1] = j;
                            size++;
                        }
                    }
                }
            }
            if (size == 0) { return; }
            if (count > size) { count = size; }
            for (int i = 0; i < count; i++)
            {
                random = RBetween(0, size - i);
                FlexibleCreateByCoordinate(coordinates[random, 0], coordinates[random, 1], ref array, value);
                coordinates[random, 0] = coordinates[size - i, 0];
                coordinates[random, 1] = coordinates[size - i, 1];
            }
        }
        //Create object(s) at line at random position
        public static void CreateLineY(bool isAll, bool isEmpty, int x, Blocks value, ref Blocks[,] array, int count = 0)
        {
            int capY = array.GetLength(1);
            int[] coordinates = new int[capY];
            int size = 0;
            if (isAll)
            {
                if (isEmpty)
                {
                    for (int j = 0; j < capY; j++)
                    {
                        if (Equals(array[x, j], Blocks.Empty))
                        {
                            FlexibleCreateByCoordinate(x, j, ref array, value);
                        }
                    }
                } else
                {
                    for (int j = 0; j < capY; j++)
                    {
                        FlexibleCreateByCoordinate(x, j, ref array, value);
                    }
                }
                return;
            }
            else
            {
                if (isEmpty)
                {
                    for(int j = 0; j < capY; j++)
                    {
                        if (array[x, j] == Blocks.Empty)
                        {
                            coordinates[size] = j;
                            size++;
                        }
                    }
                    if(count > size) 
                    {
                        for (int j = 0; j < size; j++)
                        {
                            FlexibleCreateByCoordinate(x, coordinates[j], ref array, value);
                        }
                        return;
                    }
                    for(int j = 0; j < count; j++)
                    {
                        int randY = RBetween(0, size - j);
                        FlexibleCreateByCoordinate(x, coordinates[randY], ref array, value);
                        coordinates[randY] = coordinates[size - (j + 1)];
                    }
                }
                else
                {
                    for (int j = 0; j < capY; j++)
                    {
                        FlexibleCreateByCoordinate(x, coordinates[j], ref array, value);
                    }
                    return;
                }
            }
        }
        public static void CreateLineX(bool isAll, bool isEmpty, int y, Blocks value, ref Blocks[,] array, int count = 0)
        {
            int capX = array.GetLength(0);
            int[] coordinates = new int[capX];
            int size = 0;
            if (isAll)
            {
                if (isEmpty)
                {
                    for (int i = 0; i < capX; i++)
                    {
                        if (Equals(array[i, y], Blocks.Empty))
                        {
                            FlexibleCreateByCoordinate(i, y, ref array, value);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < capX; i++)
                    {
                        FlexibleCreateByCoordinate(i, y, ref array, value);
                    }
                }
                return;
            }
            else
            {
                if (isEmpty)
                {
                    for (int i = 0; i < capX; i++)
                    {
                        if (array[i, y] == Blocks.Empty)
                        {
                            coordinates[size] = i;
                            size++;
                        }
                    }
                    if(count > size)
                    {
                        for(int i = 0; i < size; i++)
                        {
                            FlexibleCreateByCoordinate(coordinates[i], y, ref array, value);
                        }
                        return;
                    }
                    for(int i = 0; i < count; i++)
                    {
                        int randX = RBetween(0, size - i);
                        FlexibleCreateByCoordinate(coordinates[randX], y, ref array, value);
                        coordinates[randX] = coordinates[size - (i + 1)];
                    }
                }
                else
                {
                    for (int i = 0; i < capX; i++)
                    {
                        FlexibleCreateByCoordinate(coordinates[i], y, ref array, value);
                    }
                }
            }
        }
        //Create object at center
        public static void CreateAtCenter(Blocks value, ref Blocks[,] array)
        {
            int x = array.GetLength(0);
            int y = array.GetLength(1);
            int centerX = (x + x % 2) / 2;
            int centerY = (y + y % 2) / 2;
            FlexibleCreateByCoordinate(centerX, centerY, ref array, value);
        }
    }
}
