using map;
using static map.Enums;
using static map.Map;
using static map.Enums;

namespace game
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Map game = new();
            int[] size = { 15, 25 };
            TypesPattern pattern = TypesPattern.FullDots;
            System.Object?[,] arguments =
            {

                { Arguments.Blocks, Arguments.lineX, false, true, Blocks.Enemy, 2, 3},
                { Arguments.Blocks, Arguments.lineX, false, true, Blocks.Enemy, 6, 3 },
                { Arguments.Blocks, Arguments.lineX, false, true, Blocks.Enemy, 10, 3 },
                { Arguments.Blocks, Arguments.lineX, false, true, Blocks.Enemy, 14, 3 },
                { Arguments.Blocks, Arguments.lineX, false, true, Blocks.Enemy, 18, 3 },
                { Arguments.Blocks, Arguments.lineX, false, true, Blocks.Enemy, 22, 3 },
                { Arguments.Blocks, Arguments.left, false, true, Blocks.Hero, 1, null},
                { Arguments.Blocks, Arguments.right, false, true, Blocks.Win, 1, null}
            };
            Map.Setup(size, pattern, arguments, ref game.field);
            Map.ShowField(game.field);
        }
    }
}