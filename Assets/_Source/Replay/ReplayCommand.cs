namespace Replay
{
    public struct ReplayCommand
    {
        public int Tick;
        public ReplayCommandType Type;

        public float MoveX;

        public ReplayActionKind ActionKind;

        public int SelectIndex;
    }
}
