using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using CIS.Core.Utilities.Extentions;
using CIS.Store.Services;
using CIS.Store.Services.Warrants;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Utilities.Extentions;
using Common.Logging;
using NHibernate;
using NHibernate.Linq;
using ServiceStack;

namespace CIS.UI.Features.Polices.Warrants
{
    public class WarrantSyncronizeTaskScheduler : ITaskScheduler
    {
        private static ILog _log = LogManager.GetCurrentClassLogger();

        private readonly ISessionFactory _sessionFactory;
        private readonly BackgroundWorker _worker;
        private readonly Timer _timer;

        private IRestClient _service;

        public WarrantSyncronizeTaskScheduler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;

            _worker = new BackgroundWorker();
            _worker.DoWork += (sender, e) => SyncronizeData();
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;

            _timer = new Timer();
            _timer.Interval = App.Data.DataStore.SyncronizeInterval * 1000;
            _timer.Tick += (sender, e) =>
            {
                if (!this.IsWorkInProgress)
                    _worker.RunWorkerAsync();
            };
        }

        private void SyncronizeData()
        {
            try
            {
                _service = IoC.Container.Resolve<IRestClient>();

                this.IsWorkInProgress = true;

                this.PullData();
                this.PushData();
                this.PullData();
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.Message, ex);
            }
            finally
            {
                this.IsWorkInProgress = false;
            }
        }

        private IEnumerable<Store.Domain.Entities.Warrant> GetStoreData()
        {
            /// steps
            /// 1. get missing sequences(ids) from local storage
            /// 2. get last sequence from local storage
            /// 3. pull data from data store

            var request = new PullWarrantRequest();

            using (var session = _sessionFactory.OpenStatelessSession())
            using (var transaction = session.BeginTransaction())
            {
                var sql = string.Format(@"
                        select distinct top {0} cast(#Temp.Sequence as BIGINT)
                        from (select distinct Sequence = number from master.dbo.spt_values) as #Temp
                        where 
	                        #Temp.Sequence between 1 and (select max(DataStoreId) from Polices.Suspects) and
	                        #Temp.Sequence not in (select DataStoreId from Polices.Suspects where DataStoreId is not null)",
                    App.Data.DataStore.FetchSize
                );

                request.FetchSize = App.Data.DataStore.FetchSize;
                request.MissingIds = session.CreateSQLQuery(sql).List<long>().ToArray();
                request.LatestId = session.Query<Suspect>().Max(o => o.DataStoreId) ?? 0;
                request.Client = App.Data.GetClientInfo();

                transaction.Commit();
            }

            var response = _service.Patch(request);
            return response.Warrants;
        }

        private IEnumerable<Store.Domain.Entities.Warrant> GetLocalData()
        {
            var localData = (IEnumerable<Store.Domain.Entities.Warrant>)null;

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var query = session.Query<Suspect>()
                    .Where(x => x.DataStoreId == null)
                    .Take(App.Data.DataStore.FetchSize)
                    .Fetch(x => x.Warrant)
                    .Fetch(x => x.Aliases)
                    .ToFuture();

                session.Query<Suspect>()
                    .Where(x => x.DataStoreId == null)
                    .Take(App.Data.DataStore.FetchSize)
                    .Fetch(x => x.Occupations)
                    .ToFuture();

                localData = query
                    .Select(x => new Store.Domain.Entities.Warrant()
                    {
                        Id = 0,
                        ParentKey = x.Warrant.Id,
                        ChildKey = x.Id,
                        WarrantCode = x.Warrant.WarrantCode,
                        CaseNumber = x.Warrant.CaseNumber,
                        Crime = x.Warrant.Crime,
                        Description = x.Warrant.Description,
                        Remarks = x.Warrant.Remarks,
                        BailAmount = x.Warrant.BailAmount,
                        IssuedOn = x.Warrant.IssuedOn,
                        IssuedBy = x.Warrant.IssuedBy,
                        IssuedAt = x.Warrant.IssuedAt != null
                            ? new Store.Domain.Entities.Address()
                            {
                                Address1 = x.Warrant.IssuedAt.Address1,
                                Address2 = x.Warrant.IssuedAt.Address2,
                                Barangay = x.Warrant.IssuedAt.Barangay,
                                City = x.Warrant.IssuedAt.City,
                                Province = x.Warrant.IssuedAt.Province
                            }
                            : null,
                        ArrestStatus = x.ArrestStatus != null
                            ? x.ArrestStatus.ToString()
                            : null,
                        ArrestDate = x.ArrestDate,
                        Disposition = x.Disposition,
                        SuspectFirstName = x.Person.FirstName,
                        SuspectMiddleName = x.Person.MiddleName,
                        SuspectLastName = x.Person.LastName,
                        SuspectSuffix = x.Person.Suffix,
                        SuspectGender = x.Person.Gender != null
                            ? x.Person.Gender.ToString()
                            : null,
                        SuspectBirthDate = x.Person.BirthDate,
                        SuspectAddress = x.Address != null
                            ? new Store.Domain.Entities.Address()
                            {
                                Address1 = x.Address.Address1,
                                Address2 = x.Address.Address2,
                                Barangay = x.Address.Barangay,
                                City = x.Address.City,
                                Province = x.Address.Province
                            }
                            : null,
                        SuspectPhysicalAttributes = x.PhysicalAttributes != null
                            ? new Store.Domain.Entities.PhysicalAttributes()
                            {
                                Hair = x.PhysicalAttributes.Hair,
                                Eyes = x.PhysicalAttributes.Eyes,
                                Build = x.PhysicalAttributes.Build,
                                Complexion = x.PhysicalAttributes.Complexion,
                                ScarsAndMarks = x.PhysicalAttributes.ScarsAndMarks,
                                Race = x.PhysicalAttributes.Race,
                                Nationality = x.PhysicalAttributes.Nationality,
                            }
                            : null,
                        SuspectAliases = x.Aliases.ToList(),
                        SuspectOccupations = x.Occupations.ToList(),
                    })
                    .ToList();

                transaction.Commit();
            }

            return localData;
        }

        private void SaveStoreData(IEnumerable<Store.Domain.Entities.Warrant> items)
        {
            foreach (var item in items)
            {
                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Warrant>()
                    .Where(x =>
                        x.DataStoreParentKey == item.ParentKey ||
                        x.Suspects.Any(o =>
                            x.Crime == item.Crime &&
                            o.Person.FirstName == item.SuspectFirstName &&
                            o.Person.MiddleName == item.SuspectMiddleName &&
                            o.Person.LastName == item.SuspectLastName &&
                            o.Person.Suffix == item.SuspectSuffix
                        )
                    )
                    .FetchMany(x => x.Suspects)
                    .ThenFetchMany(x => x.Aliases)
                    .ToFuture();

                    session.Query<Warrant>()
                        .Where(x =>
                            x.DataStoreParentKey == item.ParentKey ||
                            x.Suspects.Any(o =>
                                x.Crime == item.Crime &&
                                o.Person.FirstName == item.SuspectFirstName &&
                                o.Person.MiddleName == item.SuspectMiddleName &&
                                o.Person.LastName == item.SuspectLastName &&
                                o.Person.Suffix == item.SuspectSuffix
                            )
                        )
                        .FetchMany(x => x.Suspects)
                        .ThenFetchMany(x => x.Occupations)
                        .ToFuture();

                    var warrants = new List<Warrant>();
                    if (query.Count() != 0)
                        warrants = query.ToList();
                    else
                        warrants.Add(new Warrant());

                    foreach (var warrant in warrants)
                    {
                        warrant.DataStoreParentKey = item.ParentKey;
                        warrant.WarrantCode = item.WarrantCode;
                        warrant.CaseNumber = item.CaseNumber;
                        warrant.Crime = item.Crime;
                        warrant.Description = item.Description;
                        warrant.Remarks = item.Remarks;
                        warrant.BailAmount = item.BailAmount;
                        warrant.IssuedOn = item.IssuedOn;
                        warrant.IssuedBy = item.IssuedBy;
                        warrant.IssuedAt = item.IssuedAt != null
                            ? new Address()
                            {
                                Address1 = item.IssuedAt.Address1,
                                Address2 = item.IssuedAt.Address2,
                                Barangay = item.IssuedAt.Barangay,
                                City = item.IssuedAt.City,
                                Province = item.IssuedAt.Province
                            }
                            : null;

                        var suspect = warrant.Suspects
                            .FirstOrDefault(x =>
                                x.DataStoreChildKey == item.ChildKey ||
                                (
                                    x.Person.FirstName.IsEqualTo(item.SuspectFirstName) &&
                                    x.Person.MiddleName.IsEqualTo(item.SuspectMiddleName) &&
                                    x.Person.LastName.IsEqualTo(item.SuspectLastName) &&
                                    x.Person.Suffix.IsEqualTo(item.SuspectSuffix)
                                )
                            );

                        if (suspect == null)
                        {
                            suspect = new Suspect();
                            warrant.AddSuspect(suspect);
                        }

                        suspect.DataStoreId = item.Id;
                        suspect.DataStoreChildKey = item.ChildKey;
                        suspect.ArrestStatus = item.ArrestStatus.AsNullableEnum<ArrestStatus>();
                        suspect.ArrestDate = item.ArrestDate;
                        suspect.Disposition = item.Disposition;
                        suspect.Person = new Person()
                        {
                            FirstName = item.SuspectFirstName,
                            MiddleName = item.SuspectMiddleName,
                            LastName = item.SuspectLastName,
                            Suffix = item.SuspectSuffix,
                            Gender = item.SuspectGender.AsNullableEnum<Gender>(),
                            BirthDate = item.SuspectBirthDate
                        };
                        suspect.Address = item.SuspectAddress != null
                            ? new Address()
                            {
                                Address1 = item.SuspectAddress.Address1,
                                Address2 = item.SuspectAddress.Address2,
                                Barangay = item.SuspectAddress.Barangay,
                                City = item.SuspectAddress.City,
                                Province = item.SuspectAddress.Province
                            }
                            : null;
                        suspect.PhysicalAttributes = item.SuspectPhysicalAttributes != null
                            ? new PhysicalAttributes()
                            {
                                Hair = item.SuspectPhysicalAttributes.Hair,
                                Eyes = item.SuspectPhysicalAttributes.Eyes,
                                Build = item.SuspectPhysicalAttributes.Build,
                                Complexion = item.SuspectPhysicalAttributes.Complexion,
                                ScarsAndMarks = item.SuspectPhysicalAttributes.ScarsAndMarks,
                                Race = item.SuspectPhysicalAttributes.Race,
                                Nationality = item.SuspectPhysicalAttributes.Nationality,
                            }
                            : null;
                        suspect.Aliases = item.SuspectAliases;
                        suspect.Occupations = item.SuspectOccupations;

                        var isNew = warrant.Id == Guid.Empty;
                        if (isNew)
                        {
                            session.Save(warrant);
                        }
                    }
                    transaction.Commit();
                }
            }

            /// steps
            //var groups = items.GroupBy(x => x.ParentKey);


            //using (var session = _sessionFactory.OpenSession())
            //using (var transaction = session.BeginTransaction())
            //{

            //    foreach (var group in groups)
            //    {
            //        var storeWarrant = group.First();

            //        var query = session.Query<Warrant>()
            //            .Where(x => x.Crime == storeWarrant.Crime);

            //        foreach (var item in group)
            //        {
            //            query = query.Where(x =>
            //                x.Suspects.Any(o =>
            //                    o.Person.FirstName == item.SuspectFirstName &&
            //                    o.Person.LastName == item.SuspectLastName
            //                )
            //            );
            //        }

            //        query = query.Fetch(x => x.Suspects);

            //        var isNew = false;
            //        var warrant = query.FirstOrDefault();
            //        if (warrant == null)
            //        {
            //            warrant = new Warrant();
            //            isNew = true;
            //        }

            //        warrant.DataStoreParentKey = storeWarrant.ParentKey;
            //        warrant.WarrantCode = storeWarrant.WarrantCode;
            //        warrant.CaseNumber = storeWarrant.CaseNumber;
            //        warrant.Crime = storeWarrant.Crime;
            //        warrant.Description = storeWarrant.Description;
            //        warrant.Remarks = storeWarrant.Remarks;
            //        warrant.BailAmount = storeWarrant.BailAmount;
            //        warrant.IssuedOn = storeWarrant.IssuedOn;
            //        warrant.IssuedBy = storeWarrant.IssuedBy;
            //        warrant.IssuedAt = new Address()
            //        {
            //            Address1 = storeWarrant.IssuedAt.Address1,
            //            Address2 = storeWarrant.IssuedAt.Address2,
            //            Barangay = storeWarrant.IssuedAt.Barangay,
            //            City = storeWarrant.IssuedAt.City,
            //            Province = storeWarrant.IssuedAt.Province
            //        };

            //        foreach (var storeSuspect in group)
            //        {
            //            var suspect = warrant.Suspects
            //                .FirstOrDefault(x =>
            //                    x.Person.FirstName.IsEqualTo(storeSuspect.SuspectFirstName) &&
            //                    x.Person.MiddleName.IsEqualTo(storeSuspect.SuspectMiddleName) &&
            //                    x.Person.LastName.IsEqualTo(storeSuspect.SuspectLastName) &&
            //                    x.Person.Suffix.IsEqualTo(storeSuspect.SuspectSuffix)
            //                );

            //            if (suspect == null)
            //            {
            //                suspect = new Suspect();
            //                warrant.AddSuspect(suspect);
            //            }

            //            suspect.DataStoreId = storeSuspect.Id;
            //            suspect.DataStoreChildKey = storeSuspect.ChildKey;
            //            suspect.ArrestStatus = storeSuspect.ArrestStatus.AsEnum<ArrestStatus>();
            //            suspect.ArrestDate = storeSuspect.ArrestDate;
            //            suspect.Disposition = storeSuspect.Disposition;
            //            suspect.Person = new Person()
            //            {
            //                FirstName = storeSuspect.SuspectFirstName,
            //                MiddleName = storeSuspect.SuspectMiddleName,
            //                LastName = storeSuspect.SuspectLastName,
            //                Suffix = storeSuspect.SuspectSuffix,
            //                Gender = storeSuspect.SuspectGender.AsEnum<Gender>(),
            //                BirthDate = storeSuspect.SuspectBirthDate
            //            };
            //            suspect.Address = new Address()
            //            {
            //                Address1 = storeSuspect.SuspectAddress.Address1,
            //                Address2 = storeSuspect.SuspectAddress.Address2,
            //                Barangay = storeSuspect.SuspectAddress.Barangay,
            //                City = storeSuspect.SuspectAddress.City,
            //                Province = storeSuspect.SuspectAddress.Province
            //            };
            //            suspect.PhysicalAttributes = new PhysicalAttributes()
            //            {
            //                Hair = storeSuspect.SuspectPhysicalAttributes.Hair,
            //                Eyes = storeSuspect.SuspectPhysicalAttributes.Eyes,
            //                Build = storeSuspect.SuspectPhysicalAttributes.Build,
            //                Complexion = storeSuspect.SuspectPhysicalAttributes.Complexion,
            //                ScarsAndMarks = storeSuspect.SuspectPhysicalAttributes.ScarsAndMarks,
            //                Race = storeSuspect.SuspectPhysicalAttributes.Race,
            //                Nationality = storeSuspect.SuspectPhysicalAttributes.Nationality,
            //            };
            //            suspect.Aliases = storeSuspect.SuspectAliases;
            //            suspect.Occupations = storeSuspect.SuspectOccupations;
            //        }

            //        if (isNew)
            //        {
            //            session.Save(warrant);
            //        }
            //    }

            //    transaction.Commit();
            //}
        }

        private void PullData()
        {
            var warrants = (IEnumerable<Store.Domain.Entities.Warrant>)null;
            while ((warrants = GetStoreData()).Count() > 0)
            {
                SaveStoreData(warrants);
            }
        }

        private void PushData()
        {
            var warrants = (IEnumerable<Store.Domain.Entities.Warrant>)null;
            while ((warrants = GetLocalData()).Count() > 0)
            {

                var request = new PushWarrantRequest()
                {
                    FetchSize = App.Data.DataStore.FetchSize,
                    Warrants = warrants.ToArray(),
                    Client = App.Data.GetClientInfo()
                };

                var response = _service.Patch(request);

                SaveStoreData(response.Warrants);
            }
        }

        public virtual bool IsWorkInProgress { get; private set; }

        public virtual void StartWork()
        {
            _timer.Start();
        }
    }
}
