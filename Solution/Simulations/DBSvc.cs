using Interfaces;
using Models.Common;
using Models.IDBSvc;
using Models.Results;

namespace Simulations
{
    // All the code in this file is included in all platforms.
    public class DBSvc : IDBSvc
    {
        private readonly List<RnH> _rnHs;

        public DBSvc()
        {
            _rnHs = new List<RnH>
                {
                    new RnH
                    {
                        Id = 1,
                        Order = 2,
                        HorseNo = 44,
                        Status = RnHStatus.CompetitionDone,
                        Mark = 6.5,
                        IsDisqualificated = false,
                        IsRanked = false
                    },
                    new RnH
                    {
                        Id = 2,
                        Order = 1,
                        HorseNo = 255,
                        Status = RnHStatus.CompetitionDone,
                        Mark = 0.0,
                        IsDisqualificated = true,
                        IsRanked = false
                    },
                    new RnH
                    {
                        Id = 3,
                        Order = 3,
                        HorseNo = 257,
                        Status = RnHStatus.CompetitionDone,
                        Mark = 0.0,
                        IsDisqualificated = false,
                        IsRanked = false
                    },
                    new RnH
                    {
                        Id = 4,
                        Order = 4,
                        HorseNo = 125,
                        Status = RnHStatus.OnCompetitionField,
                        Mark = 0.0,
                        IsDisqualificated = false,
                        IsRanked = false
                    },
                    new RnH
                    {
                        Id = 5,
                        Order = 5,
                        HorseNo = 25,
                        Status = RnHStatus.OnPreparationField,
                        Mark = 0.0,
                        IsDisqualificated = false,
                        IsRanked = false
                    },
                    new RnH
                    {
                        Id = 6,
                        Order = 6,
                        HorseNo = 34,
                        Status = RnHStatus.OnWarmUpField,
                        Mark = 0.0,
                        IsDisqualificated = false,
                        IsRanked = false
                    },
                    new RnH
                    {
                        Id = 7,
                        Order = 7,
                        HorseNo = 134,
                        Status = RnHStatus.NotPresent,
                        Mark = 0.0,
                        IsDisqualificated = false,
                        IsRanked = false
                    }
                };
        }

        public Result<IEnumerable<RnH>> ReadAll()
        {
            return new Result<IEnumerable<RnH>>
            {
                Content = new List<RnH>(_rnHs)
            };
        }

        public Result<bool> Delete(RnH rnh)
        {
            RnH toRemove = _rnHs.FirstOrDefault(r => r.Id == rnh.Id);
            _rnHs.Remove(toRemove);
            return new Result<bool> { Content = true };
        }

        public Result<RnH> Insert(RnH rnh)
        {
            _rnHs.Add(rnh);
            return new Result<RnH> { Content = rnh };
        }

        public Result<RnH> Save(RnH rnh)
        {
            return new Result<RnH> { Content = rnh };
        }

        public Result<bool> SetDBSettings(DBConnectionSettings settings)
        {
            return new Result<bool> { Content = true };
        }

        public Result<string> TitleSave(string title)
        {
            return new Result<string> { Content = title };
        }
    }
}