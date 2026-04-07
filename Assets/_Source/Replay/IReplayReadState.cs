namespace Replay
{
    public interface IReplayReadState
    {
        bool IsReplaying { get; }

        float ReplayMoveX { get; }
    }
}
