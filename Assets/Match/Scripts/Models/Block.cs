using Assets.Match.Scripts.Enum;

namespace Assets.Match.Scripts.Models
{

    public class Block : PositionOnBoard
    {
        public BlockType Type;

        public bool IsBonus { get; set; }

    }

}
