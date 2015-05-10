using System.Windows.Media;
namespace FlowComponent
{
    public interface IThumbnailManager
    {
        ImageSource GetThumbnail(string host, string path);
    }
}
