using static map.CreateFunctions;
using static map.Enums;
using static map.PatternFunctions;
//AROUND: means objects in diagonal axis closest relatively object
//NEAR: means objects in horizontal and vertical axis closest relatively object

namespace map
{
    ///#enums
    
    //Object in game
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
        
        ///#Other
        
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
        public static Blocks[,] Setup(int[] size, TypesPattern pattern, Object?[,] arguments, ref Blocks[,] array)
        {
            //string Type<T>(T layout) { return typeof(T).Name; };
            int x = size[0] - 1;
            int y = size[1] - 1;
            Fill(size, Blocks.Empty, ref array);
            Pattern(pattern, ref array);
            int sizeArgs = arguments.GetLength(0);
            Object?[] argument;
            for (int numArgument = 0; numArgument < sizeArgs; numArgument++)
            {
                argument = ArrayOneByTwo(arguments, numArgument);
                switch (argument[0])
                {
                    case Arguments.Blocks:
                        switch (argument[1])
                        {
                            case Arguments.left:
                                CreateLineX((bool)argument[2], (bool)argument[3], 0, (Blocks)argument[4], ref array, (int)argument[5]);
                                break;
                            case Arguments.right:
                                Console.WriteLine(argument[2]);
                                Console.WriteLine(argument[3]);
                                Console.WriteLine(argument[4]);
                                Console.WriteLine(argument[5]);
                                Console.WriteLine(x);
                                CreateLineX((bool)argument[2], (bool)argument[3], y, (Blocks)argument[4], ref array, (int)argument[5]);
                                break;
                            case Arguments.up:
                                CreateLineY((bool)argument[2], (bool)argument[3], 0, (Blocks)argument[4], ref array, (int)argument[5]);
                                break;
                            case Arguments.down:
                                CreateLineY((bool)argument[2], (bool)argument[3], x, (Blocks)argument[4], ref array, (int)argument[5]);
                                break;
                            case Arguments.lineX:
                                CreateLineX((bool)argument[2], (bool)argument[3], (int)argument[5], (Blocks)argument[4], ref array, (int)argument[6]);
                                break;
                            case Arguments.lineY:
                                CreateLineY((bool)argument[2], (bool)argument[3], (int)argument[5], (Blocks)argument[4], ref array, (int)argument[6]);
                                break;
                            case Arguments.center:
                                CreateAtCenter((Blocks)argument[2], ref array);
                                break;
                        }
                        break;
                }
            }
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