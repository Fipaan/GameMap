
namespace game
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Map game = new();
            int arofl = 1122212;
            int[] size = { 15, 15 };
            TypesPattern pattern = TypesPattern.FullDots;
            System.Object[,] arguments =
            {
                { Arguments.Blocks, Arguments.left, Arguments.one, Blocks.Hero, null, null, null, 1 },
                { Arguments.Blocks, Arguments.right, Arguments.one, Blocks.Win, null, null, null, 1 },
                { Arguments.Blocks, Arguments.hollow, Arguments.one, Blocks.Enemy, new int[]{3, 3}, new int[]{size[0] - 3, size[1] - 3}, null, 7 }
            };
            Map.Setup(size, pattern, arguments, ref game.field);
            Map.ShowField(game.field);
        }
    }
}