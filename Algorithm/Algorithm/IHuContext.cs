

namespace MahjongAlgorithm.Algorithm {

    public interface IHuContext {
    }


    internal class TableHuContext : IHuContext {
        internal int Value { get; set; }
        internal Card[] Types { get; set; }
    }

    internal sealed class DFSHuContext : TableHuContext {
        internal Card CC { get; set; }
        internal Card[] AAA { get; set; }
        internal Card[] ABC { get; set; }
    }
}