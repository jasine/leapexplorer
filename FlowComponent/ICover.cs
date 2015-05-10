using System.Windows.Media.Media3D;
namespace FlowComponent
{
    public interface ICover
    {
        void Animate(int index, bool animate);
        bool Matches(MeshGeometry3D mesh);
        void Destroy();
    }
}
