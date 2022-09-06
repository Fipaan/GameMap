using System;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace game
{
    ///#enums
    
    //Object in game
    public enum Blocks
    {
        Empty,
        Hero,
        Wall,
        Enemy,
        Death,
        Check,
        Checked,
        Win
    };
    //Types of patterns
    public enum TypesPattern
    {
        FullDots,
        FullDotsWide,
        FullTwoDotsX,
        FullTwoDotsWideX,
        FullTwoDotsY,
        FullTwoDotsWideY,
        FullSquareDots,
        FullSquareDotsWide
    };
    //Types of functions
    public enum Arguments
    {
        left,
        right,
        up,
        down,
        all,
        one,
        Blocks,
        hollow,
        wideHollow,
        rect,
        center,
        corners,
        random
    }
    //Layouts 3x3
    public enum Layout3
    {
        Down,
        Up,
        Left,
        Right
    };
    public class Map
    {
        ///#FillArray
        public Blocks[,] field = new Blocks[1, 1];
        
        //Create new array with size [x, y] filled with values
        public static void Fill<T>(int[] size, T value, ref T[,] array){
            int x = size[0];
            int y = size[1];
            array = new T[x, y];
            for(int i = 0; i < x; i++)
            {
                for(int j = 0; j < y; j++)
                {
                    array[i, j] = value;
                }
            }
        }
        //Fill array by pattern
        public static void Pattern(TypesPattern type, ref Blocks[,] array)
        {
            int x = array.GetLength(0);
            int y = array.GetLength(1);
            switch (type)
            {
                case TypesPattern.FullDots:
                    for(int i = 0; i < x; i++)
                    {
                        for(int j = 0; j < y; j++)
                        {
                            if(i % 2 == 1 && j % 2 == 1)
                            {
                                array[i, j] = Blocks.Wall;
                            }
                        }
                    }
                    break;
                case TypesPattern.FullTwoDotsX:
                    for (int i = 0; i < x; i++)
                    {
                        for (int j = 0; j < y; j++)
                        {
                            if (i % 3 == 1 && j % 2 == 1 || i % 3 == 2 && j % 2 == 1)
                            {
                                array[i, j] = Blocks.Wall;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        //Create field by layout
        public static void Layout<T1>(T1 layout, ref Blocks[,] array)
        {
            Blocks _ = Blocks.Empty;
            Blocks W = Blocks.Wall;
            Blocks H = Blocks.Hero;
            Blocks P = Blocks.Win;
            string TypeLayout<T2>(T2 layout) { return typeof(T2).Name; };
            switch(TypeLayout(layout))
            {
                case "Layout3":
                    switch (layout)
                    {
                        case Layout3.Down:
                            array = new Blocks[,]
                            {
                        {_, _, _},
                        {_, W, _},
                        {H, W, P}
                            };
                            break;
                        case Layout3.Up:
                            array = new Blocks[,]
                            {
                        {P, W, H},
                        {_, W, _},
                        {_, _, _}
                            };
                            break;
                        case Layout3.Left:
                            array = new Blocks[,]
                            {
                        {P, _, _},
                        {W, W, _},
                        {H, _, _}
                            };
                            break;
                        case Layout3.Right:
                            array = new Blocks[,]
                            {
                        {_, _, H},
                        {_, W, W},
                        {_, _, P}
                            };
                            break;
                    }
                    break;
            }
            
        }
        
        
        
        
        ///#CreateObject
        
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
        //Create *count* of objects in random positions
        public static void CreateAtRandom<T>(T value, int count, ref T[,] array, bool isEmpty)
        {
            int countEmpty;
            if (isEmpty)
            {
                countEmpty = Counter(Blocks.Empty, (Blocks[,])System.Convert.ChangeType(array, typeof(Blocks[,])));
                if(countEmpty == 0) { return; }
                if(countEmpty < count) { count = countEmpty; }
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
                    AroundAll(Blocks.Enemy, Blocks.Death, ref array);
                    break;
                default:
                    CreateAtRandom(type, count, ref array, isEmpty);
                    break;
            }
        }
        //Create object in coordinate
        public static void CreateByCoordinate<T>(int x, int y, ref T[,] array, T value)
        {
            switch (value)
            {
                default:
                    array[x, y] = value;
                    break;
            }
        }
        //Around all *value* create *valueAround*
        public static void AroundAll(Blocks value, Blocks valueAround, ref Blocks[,] array)
        {
            int x = array.GetLength(0);
            int y = array.GetLength(1);
            for(int i = 0; i < x; i++)
            {
                for(int j = 0; j < y; j++)
                {
                    if(array[i, j] == value)
                    {
                        Around(i, j, valueAround, ref array);
                    }
                }
            }
        }
        //Around position [x, y] create *value*
        public static void Around(int x, int y, Blocks value, ref Blocks[,] array)
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
                        if (array[i, j] == Blocks.Empty)
                        {
                            array[i, j] = value;
                        }
                    }
                    catch { };
                }
            }
        }
        //Top, bottom, left, right of *value* create *valueAround*
        public static void AroundAllHV(Blocks value, Blocks valueAround, ref Blocks[,] array)
        {
            int x = array.GetLength(0);
            int y = array.GetLength(1);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (array[i, j] == value)
                    {
                        AroundHV(i, j, valueAround, ref array);
                    }
                }
            }
        }
        //Top, bottom, left, right of position [x, y] create *value*
        public static void AroundHV(int x, int y, Blocks value, ref Blocks[,] array)
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
                        if (array[i, j] == Blocks.Empty)
                        {
                            array[i, j] = value;
                        }
                    }
                    catch { };
                }
            }
        }



        ///#Detect

        //Detect all values, and return array with coordinates
        public static int[,]? DetectAll<T>(T value, T[,] array)
        {
            int capX = array.GetLength(0);
            int capY = array.GetLength(1);
            int iteration = 0;
            int[,] newArray0 = new int[capX * capY, 2];
            for(int i = 0; i < capX; i++)
            {
                for(int j = 0; j < capY; j++)
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
            for(int i = 0; i < iteration; i++)
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
        //Detect NEAR *near* in specific coordinates
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
        Eight position AROUND: detect *value* in *array*
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
        Four position NEAR: detect *value* in *array*
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
        //If *array1* not equals *array2*: return true, else: return false
        public static bool ArrayEquals<T>(T[,] array1, T[,] array2)
        {
            int x1 = array1.GetLength(0);
            int y1 = array1.GetLength(1);
            if((x1 != array2.GetLength(0)) || (y1 != array2.GetLength(1))) { return false; }
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




        ///#Other
        
        //Create object(s) in hollow rectangle
        public static void Hollow(int count, int minX, int minY, int capX, int capY, int wide, Blocks value,ref Blocks[,] array, bool isAll, bool onlyEmpty)
        {
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
                    for(int i = minX; i < capX; i++)
                    {
                        for(int j = minY; j < capY; j++)
                        {
                            if (array[i, j] == Blocks.Empty)
                            {
                                CreateByCoordinate(i, j, ref array, value);
                            }
                        }
                    }
                }
                else
                {
                    for(int i = minX; i < capX; i++)
                    {
                        for(int j = minY; j < capY; j++)
                        {
                            CreateByCoordinate(i, j, ref array, value);
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
                        if ((array[i, j] == Blocks.Empty) && ((checkX(i)) || (checkY(j))))
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
                for(int i = minX; i < capX; i++)
                {
                    for(int j = minY; j < capY; j++)
                    {
                        if(checkX(i) || checkY(j))
                        {
                            coordinates[size, 0] = i;
                            coordinates[size, 1] = j;
                            size++;
                        }
                    }
                }
            }
            if(size == 0) { return; }
            if(count > size) { count = size; }
            for(int i = 0; i < count; i++)
            {
                random = RBetween(0, size - i);
                CreateByCoordinate(coordinates[random, 0], coordinates[random, 1], ref array, value);
                coordinates[random, 0] = coordinates[size - i, 0];
                coordinates[random, 1] = coordinates[size - i, 1];
            }
        }
        //Random number between x(include) and y(exclude)
        public static int RBetween(int x, int y)
        {
            Random rnd = new();
            return rnd.Next(x, y);
        }
        //Return amount of *value*
        public static int Counter<T>(T value, T[,] array)
        {
            int counter = 0;
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (Equals(array[i, j], value))
                    {
                        counter++;
                    }
                }
            }
            return counter;
        }
        //Convert all *input* in *output*
        public static void Convert<T>(T input, T output, ref T[,] array)
        {
            int x = array.GetLength(0);
            int y = array.GetLength(1);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (Equals(array[i, j], input))
                    {
                        array[i, j] = output;
                    }
                }
            }
        }
        //Returns an array that is equal to current array
        public static T[,] Assign<T>(T[,] array)
        {
            return (T[,])array.Clone();
        }
        //Return array[x]
        public static T[] ArrayOneByTwo<T>(T[,] array, int x)
        {
            int max = array.GetLength(1);
            T[] output = new T[max];
            for(int i = 0; i < max; i++)
            {
                output[i] = array[x, i];
            }
            return output;
        }
        //Custom creation field. Argument: [Blocks*editable*[0], location[1], OneOrAll[2], Block[3], coordinates[4]]
        //coordinates[4] are optional, last argument always need, to be count of repeat argument
        public static Blocks[,] Setup(int[] size, TypesPattern pattern, Object[,] arguments, ref Blocks[,] array)
        {
            //string Type<T>(T layout) { return typeof(T).Name; };
            int x = size[0] - 1;
            int y = size[1] - 1;
            Fill(size, Blocks.Empty, ref array);
            int sizeArgs = arguments.GetLength(0);
            int counterArgs = 0;
            Object[] argument;
            int blank;
            bool checkEmpty = false;
            int numI = 0;
            int numJ = 0;
            bool valueBreak = false;
            Pattern(pattern, ref array);
            while (counterArgs < sizeArgs && !valueBreak)
            {
                argument = ArrayOneByTwo(arguments, counterArgs);
                for (int countArgument = 0; countArgument < (int)argument[argument.GetLength(0) - 1]; countArgument++)
                {
                    switch (argument[0])
                    {
                        case Arguments.Blocks:
                            switch (argument[1])
                            {
                                case Arguments.left:
                                    switch (argument[2])
                                    {
                                        case Arguments.all:
                                            for (int i = 0; i < x; i++)
                                            {
                                                if (array[i, 0] == Blocks.Empty)
                                                {
                                                    array[i, 0] = (Blocks)argument[3];
                                                }
                                            }
                                            break;
                                        case Arguments.one:
                                            for (int i = 0; i < x; i++)
                                            {
                                                if (array[i, 0] == Blocks.Empty)
                                                {
                                                    checkEmpty = true;
                                                }
                                            }
                                            blank = RBetween(0, x);
                                            if ((array[blank, 0] != Blocks.Empty) && checkEmpty)
                                            {
                                                do
                                                {
                                                    blank = RBetween(0, x);
                                                } while (array[blank, 0] != Blocks.Empty);
                                            }
                                            if (checkEmpty) { array[blank, 0] = (Blocks)argument[3]; };
                                            checkEmpty = false;
                                            break;
                                        default:
                                            valueBreak = true;
                                            break;
                                    }
                                    break;
                                case Arguments.right:
                                    switch (argument[2])
                                    {
                                        case Arguments.all:
                                            for (int i = 0; i < x; i++)
                                            {
                                                if (array[i, y] == Blocks.Empty)
                                                {
                                                    array[i, y] = (Blocks)argument[3];
                                                }
                                            }
                                            break;
                                        case Arguments.one:
                                            for (int i = 0; i < x; i++)
                                            {
                                                if (array[i, y] == Blocks.Empty)
                                                {
                                                    checkEmpty = true;
                                                }
                                            }
                                            blank = RBetween(0, x);
                                            if ((array[blank, y] != Blocks.Empty) && checkEmpty)
                                            {
                                                do
                                                {
                                                    blank = RBetween(0, x);
                                                } while (array[blank, y] != Blocks.Empty);
                                            }
                                            if (checkEmpty) { array[blank, y] = (Blocks)argument[3]; };
                                            checkEmpty = false;
                                            break;
                                        default:
                                            valueBreak = true;
                                            break;
                                    }
                                    break;
                                case Arguments.up:
                                    switch (argument[2])
                                    {
                                        case Arguments.all:
                                            for (int j = 0; j < y; j++)
                                            {
                                                if (array[0, j] == Blocks.Empty)
                                                {
                                                    array[0, j] = (Blocks)argument[3];
                                                }
                                            }
                                            break;
                                        case Arguments.one:
                                            for (int j = 0; j < y; j++)
                                            {
                                                if (array[0, j] == Blocks.Empty)
                                                {
                                                    checkEmpty = true;
                                                }
                                            }
                                            blank = RBetween(0, y);
                                            if ((array[0, blank] != Blocks.Empty) && checkEmpty)
                                            {
                                                do
                                                {
                                                    blank = RBetween(0, y);
                                                } while (array[0, blank] != Blocks.Empty);
                                            }
                                            if (checkEmpty) { array[0, blank] = (Blocks)argument[3]; };
                                            checkEmpty = false;
                                            break;
                                        default:
                                            valueBreak = true;
                                            break;
                                    }
                                    break;
                                case Arguments.down:
                                    switch (argument[2])
                                    {
                                        case Arguments.all:
                                            for (int j = 0; j < y; j++)
                                            {
                                                if (array[x, j] == Blocks.Empty)
                                                {
                                                    array[x, j] = (Blocks)argument[3];
                                                }
                                            }
                                            break;
                                        case Arguments.one:
                                            for (int j = 0; j < y; j++)
                                            {
                                                if (array[x, j] == Blocks.Empty)
                                                {
                                                    checkEmpty = true;
                                                }
                                            }
                                            blank = RBetween(0, y);
                                            if ((array[x, blank] != Blocks.Empty) && checkEmpty)
                                            {
                                                do
                                                {
                                                    blank = RBetween(0, y);
                                                } while (array[x, blank] != Blocks.Empty);
                                            }
                                            if (checkEmpty) { array[x, blank] = (Blocks)argument[3]; };
                                            checkEmpty = false;
                                            break;
                                        default:
                                            valueBreak = true;
                                            break;
                                    }
                                    break;
                                case Arguments.center:
                                    switch (x % 2)
                                    {
                                        case 0:
                                            numI = x / 2;
                                            break;
                                        case 1:
                                            numI = (x + 1) / 2;
                                            break;
                                    }
                                    switch (y % 2)
                                    {
                                        case 0:
                                            numJ = y / 2;
                                            break;
                                        case 1:
                                            numJ = (y + 1) / 2;
                                            break;
                                    }
                                    if (array[numI, numJ] == Blocks.Empty)
                                    {
                                        array[numI, numJ] = (Blocks)argument[3];
                                    }
                                    break;
                                case Arguments.hollow:
                                    switch (argument[2])
                                    {
                                        case Arguments.all:
                                            Hollow((int[])argument[4], (int[])argument[5], 1, (Blocks)argument[3], ref array, true);
                                            break;
                                        case Arguments.one:
                                            Hollow((int[])argument[4], (int[])argument[5], 1, (Blocks)argument[3], ref array, false);
                                            break;
                                        default:
                                            valueBreak = true;
                                            break;
                                    }
                                    break;
                                case Arguments.wideHollow:
                                    switch (argument[2])
                                    {
                                        case Arguments.all:
                                            Hollow((int[])argument[4], (int[])argument[5], (int)argument[6], (Blocks)argument[3], ref array, true);
                                            break;
                                        case Arguments.one:
                                            Hollow((int[])argument[4], (int[])argument[5], (int)argument[6], (Blocks)argument[3], ref array, false);
                                            break;
                                        default:
                                            valueBreak = true;
                                            break;
                                    }
                                    break;
                                case Arguments.rect:
                                    break;
                                case Arguments.corners:
                                    switch (argument[2])
                                    {
                                        case Arguments.all:
                                            int cornerX = 0;
                                            int cornerY = 0;
                                            for (int i = 0; i < 4; i++)
                                            {
                                                switch (i)
                                                {
                                                    case 0:
                                                        cornerX = 0;
                                                        cornerY = 0;
                                                        break;
                                                    case 1:
                                                        cornerX = x - 1;
                                                        cornerY = 0;
                                                        break;
                                                    case 2:
                                                        cornerX = 0;
                                                        cornerY = y - 1;
                                                        break;
                                                    case 3:
                                                        cornerX = x - 1;
                                                        cornerY = y - 1;
                                                        break;
                                                }
                                                if (array[cornerX, cornerY] == Blocks.Empty)
                                                {
                                                    array[cornerX, cornerY] = (Blocks)argument[3];
                                                }
                                            }
                                            break;
                                        default:
                                            valueBreak = true;
                                            break;
                                    }
                                    break;
                                case Arguments.one:
                                    Create((Blocks)argument[3], 1, ref array);
                                    break;
                                default:
                                    valueBreak = true;
                                    break;
                            }
                            break;
                        default:
                            valueBreak = true;
                            break;
                    }

                }
                counterArgs++;
            }
            AroundAll(Blocks.Enemy, Blocks.Death, ref array);
            if (valueBreak) { Console.WriteLine($"Setup error in argument {counterArgs}"); }
            return (Blocks[,])array.Clone();
        }
        //Show *array* in console
        public static void ShowField(Blocks[,] array)
        {
            int x = array.GetLength(0);
            int y = array.GetLength(1);
            string border = new(' ', 2 * y + 4);
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(border);
            for (int i = 0; i < x; i++)
            {
                Console.Write("  ");
                Console.BackgroundColor = ConsoleColor.Black;
                for (int j = 0; j < y; j++)
                {
                    switch (array[i, j])
                    {
                        case Blocks.Empty:
                            Console.Write("  ");
                            break;
                        case Blocks.Check:
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write("Ch");
                            break;
                        case Blocks.Checked:
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("Cd");
                            break;
                        case Blocks.Enemy:
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("II");
                            break;
                        case Blocks.Death:
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("()");
                            break;
                        case Blocks.Wall:
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("[]");
                            break;
                        case Blocks.Hero:
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("\\/");
                            break;
                        case Blocks.Win:
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("|P");
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("??");
                            break;
                    }
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine("  ");
            }
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(border);
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}