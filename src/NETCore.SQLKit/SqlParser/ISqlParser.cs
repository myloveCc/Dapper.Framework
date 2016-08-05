
namespace NETCore.SQLKit
{
    public interface ISqlParser
    {
        string ParamPrefix { get; }

        string ElementLeftPrefix { get; }

        string ElementRightPrefix { get; }
    }
}
