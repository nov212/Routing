namespace Routing
{
    public interface IPolygon
    {
        bool InRange(int row, int col);
        IPolygon Add(IPolygon p);
    }
}
