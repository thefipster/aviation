using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.BlackBox;
using TheFipster.Aviation.Modules.BlackBox.Components;

namespace TheFipster.Aviation.Modules.BlackBox
{
    public class BlackboxOperations
    {
        public ICollection<Record> Compress(ICollection<Record> records)
            => new BlackBoxCompressor().CompressRecords(records);

        public BlackBoxStats Scan(ICollection<Record> records)
            => new BlackBoxScanner().GenerateStatsFromBlackbox(records);
    }
}
