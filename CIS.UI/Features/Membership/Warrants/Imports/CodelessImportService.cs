using System.Collections.Generic;
using System.IO;
using System.Linq;
using CIS.Core.Domain.Common;
using CIS.Core.Domain.Membership;
using CIS.Core.Utility.Extention;
using CIS.Data.Definition;
using NHibernate;
using NHibernate.Linq;

namespace CIS.UI.Features.Membership.Warrants.Imports
{
    public class CodelessImportService(ISessionFactory sessionFactory, ImportViewModel viewModel) : IImportService
    {
        private readonly ISessionFactory _sessionFactory = sessionFactory;
        private readonly ImportViewModel _viewModel = viewModel;

        private List<CodelessWarrant> ReadFromFile()
        {
            var directoryInfo = new DirectoryInfo(this._viewModel.SourcePath);
            var warrantFiles = directoryInfo.GetFiles("*codeless*xls*", SearchOption.AllDirectories).ToList();

            // parse warrants
            var warrants = warrantFiles
                .Select(x => Importer.ImportExcel(
                    path: x.FullName, 
                    map: CodelessWarrant.OfDataRow
                ))
                .SelectMany(x => x)
                .Select(CodelessWarrant.Fix)
                .Where(CodelessWarrant.IsValid)
                .ToList();

            return warrants;
        }

        private void SaveBatch(int batchSize, IEnumerable<CodelessWarrant> batch)
        {
            using var session = _sessionFactory.OpenSession();
            using var transaction = session.BeginTransaction();
            
            session.SetBatchSize(batchSize);

            var warrants = new List<Warrant>();

            foreach (var item in batch)
            {
                var exists = session.Query<Suspect>().Any(x =>
                    x.Person.FirstName == item.FirstName &&
                    x.Person.MiddleName == item.MiddleName &&
                    x.Person.LastName == item.LastName &&
                    x.Person.Suffix == item.Suffix &&
                    x.Warrant.Crime == item.Case
                );

                if (exists)
                    continue;

                warrants.Add(new Warrant()
                {
                    Crime = item.Case,
                    Remarks = item.Disposition,
                    Suspects = new List<Suspect>()
                    {
                        new()
                        {
                            Person = new Person()
                            {
                                Prefix = item.Prefix,
                                LastName = item.LastName,
                                MiddleName = item.MiddleName,
                                FirstName = item.FirstName,
                                Suffix = item.Suffix
                            },
                            Address = new Address()
                            {
                                City = item.Address
                            },
                            ArrestDate = item.ArrestedOn,
                            Disposition = item.Disposition
                        }
                    },

                });
            }

            foreach (var item in warrants)
            {
                session.Save(item);

                this._viewModel.TotalCases++;
                this._viewModel.TotalSuspects++;
            }

            transaction.Commit();
        }

        public void Execute()
        {
            ReadFromFile()
                .Chunk(size: 1100)
                .ForEach(batch => SaveBatch(
                    batchSize: batch.Count(), 
                    batch: batch
                ));
        }
    }
}
