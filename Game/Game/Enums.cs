namespace map
{
    public class Enums
    {
        public enum Blocks
        {
            Empty,
            Hero,
            Wall,
            MeleeEnemy,
            OneSideEnemy,
            direction,
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
            lineX,
            lineY,
            count,
            Blocks,
            hollow,
            rect,
            center,
            corners,
            random,
            _null
        }
        //Layouts 3x3
        public enum Layout3
        {
            Down,
            Up,
            Left,
            Right
        };

    }
}
