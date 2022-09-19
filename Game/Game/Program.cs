using map;
using static map.Enums;
using static map.Map;

namespace game
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Map game = new();
            int[] size = { 15, 25 };
            TypesPattern pattern = TypesPattern.FullDots;
            System.Object?[] argumentsSE = { Arguments.up };
            System.Object?[,] arguments =
            {

                { Arguments.Blocks, Arguments.down, false, true, Blocks.OneSideEnemy, 3, null, argumentsSE },
                { Arguments.Blocks, Arguments.lineX, false, true, Blocks.MeleeEnemy, 6, 3, null },
                { Arguments.Blocks, Arguments.lineX, false, true, Blocks.MeleeEnemy, 10, 3, null },
                { Arguments.Blocks, Arguments.lineX, false, true, Blocks.MeleeEnemy, 14, 3, null },
                { Arguments.Blocks, Arguments.lineX, false, true, Blocks.MeleeEnemy, 18, 3, null },
                { Arguments.Blocks, Arguments.lineX, false, true, Blocks.MeleeEnemy, 22, 3, null },
                { Arguments.Blocks, Arguments.left, false, true, Blocks.Hero, 1, null, null},
                { Arguments.Blocks, Arguments.right, false, true, Blocks.Win, 1, null, null}
            };
            Map.Setup(size, pattern, arguments, ref game.field);
            Map.ShowField(game.field);
        }
    }
}