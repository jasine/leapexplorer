namespace FlowComponent
{
    public interface ICoverFactory
    {
        ICover NewCover(string host, string path, int coverPos, int currentPos);
    }
}
