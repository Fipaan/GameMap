using static map.Enums;
namespace map
{
    public class PatternFunctions
    {
        //Fill array by pattern
        public static void Pattern(TypesPattern type, ref Blocks[,] array)
        {
            int x = array.GetLength(0);
            int y = array.GetLength(1);
            switch (type)
            {
                case TypesPattern.FullDots:
                    for (int i = 0; i < x; i++)
                    {
                        for (int j = 0; j < y; j++)
                        {
                            if (i % 2 == 1 && j % 2 == 1)
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
            switch (TypeLayout(layout))
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
    }
}
