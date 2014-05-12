using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Barangays;
using CIS.Core.Entities.Commons;
using CIS.UI.Utilities.Extentions;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Exceptions;
using NHibernate.Linq;
using NHibernate.Transform;
using ReactiveUI;

namespace CIS.UI.Features.Barangays.Blotters.MasterList
{
    public class BlotterListController : ControllerBase<BlotterListViewModel>
    {
        public BlotterListController(BlotterListViewModel viewModel)
            : base(viewModel)
        {
            this.ViewModel.Search = new ReactiveCommand();
            this.ViewModel.Search.Subscribe(x => Search());
            this.ViewModel.Search.ThrownExceptions.Handle(this);

            this.ViewModel.Create = new ReactiveCommand();
            this.ViewModel.Create.Subscribe(x => Create());
            this.ViewModel.Create.ThrownExceptions.Handle(this);

            this.ViewModel.Edit = new ReactiveCommand();
            this.ViewModel.Edit.Subscribe(x => Edit((BlotterListItemViewModel)x));
            this.ViewModel.Edit.ThrownExceptions.Handle(this);

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.Subscribe(x => Delete((BlotterListItemViewModel)x));
            this.ViewModel.Delete.ThrownExceptions.Handle(this);

            //this.CreateDummyIncumbent();
            //this.CreateDummyBlotter();
            this.Search();
        }

        private void CreateDummyIncumbent()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var list = new List<Incumbent>()
                {
                    new Incumbent()
                    {
                        Date = DateTime.Today.AddYears(-5),
                        Officials = new List<Official>()
                        {
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "Engr.",
                                    FirstName = "Lingoy",
                                    MiddleName = "",
                                    LastName= "Canlas",
                                    Gender = Gender.Male,
                                },
                                Position = Position.BarangayCaptain,
                                IsActive = true,
                            },
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "Hon.",
                                    FirstName = "Myrna",
                                    MiddleName = "L.",
                                    LastName= "Cybilla",
                                    Gender = Gender.Female,
                                },
                                Position = Position.BarangayCouncilor,
                                Committee = Committee.CommitteeOnEducationAndInformation,
                                IsActive = true,
                            },
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "Hon.",
                                    FirstName = "Melca",
                                    MiddleName = "V.",
                                    LastName= "Chavez",
                                    Gender = Gender.Male,
                                },
                                Position = Position.BarangayCouncilor,
                                Committee = Committee.CommitteeOnFinanceAndAppropriation,
                                IsActive = true,
                            },
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "Hon.",
                                    FirstName = "Imelda",
                                    MiddleName = "S.",
                                    LastName= "Roca",
                                    Gender = Gender.Female,
                                },
                                Position = Position.BarangayCouncilor,
                                Committee = Committee.CommitteeOnHealthAndSanitation,
                                IsActive = true,
                            },
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "Hon.",
                                    FirstName = "Fe",
                                    MiddleName = "M.",
                                    LastName= "Quijano",
                                    Gender = Gender.Female,
                                },
                                Position = Position.BarangayCouncilor,
                                Committee = Committee.CommitteeOnInfrastractures,
                                IsActive = true,
                            },
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "Hon.",
                                    FirstName = "Sabina",
                                    MiddleName = "A.",
                                    LastName= "Teraza",
                                    Gender = Gender.Female,
                                },
                                Position = Position.BarangayCouncilor,
                                Committee = Committee.CommitteeOnLawsRulesAndRegulations,
                                IsActive = true,
                            },
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "Hon.",
                                    FirstName = "Rusita",
                                    MiddleName = "A.",
                                    LastName= "Luyten",
                                    Gender = Gender.Female,
                                },
                                Position = Position.BarangayCouncilor,
                                Committee = Committee.CommitteeOnLivelihodAndAgriculture,
                                IsActive = true,
                            },
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "Hon.",
                                    FirstName = "Wardiolita",
                                    MiddleName = "O.",
                                    LastName= "Chong",
                                    Gender = Gender.Female,
                                },
                                Position = Position.BarangayCouncilor,
                                Committee = Committee.CommitteeOnPeaceAndOrderAndPublicSafety,
                                IsActive = true,
                            },
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "Hon.",
                                    FirstName = "Jean",
                                    MiddleName = "",
                                    LastName= "Perpinan",
                                    Gender = Gender.Female,
                                },
                                Position = Position.SKChairman,
                                IsActive = true,
                            },
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "",
                                    FirstName = "Maria Rosario",
                                    MiddleName = "A.",
                                    LastName= "Ibanez",
                                    Gender = Gender.Female,
                                },
                                Position = Position.BarangayTreasurer,
                                IsActive = true,
                            },
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "",
                                    FirstName = "Evan Nathaniel",
                                    MiddleName = "O.",
                                    LastName= "Chong",
                                    Gender = Gender.Male,
                                },
                                Position = Position.BarangaySecretary,
                                IsActive = true,
                            },
                        },

                    },
                    new Incumbent()
                    {
                        Date = DateTime.Today,
                        Officials = new List<Official>()
                        {
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "Engr.",
                                    FirstName = "Reynante",
                                    MiddleName = "Asprec",
                                    LastName= "Alibuyog",
                                    Gender = Gender.Male,
                                },
                                Position = Position.BarangayCaptain,
                                IsActive = true,
                            },
                         new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "Hon.",
                                    FirstName = "Myrna",
                                    MiddleName = "L.",
                                    LastName= "Cybilla",
                                    Gender = Gender.Female,
                                },
                                Position = Position.BarangayCouncilor,
                                Committee = Committee.CommitteeOnEducationAndInformation,
                                IsActive = true,
                            },
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "Hon.",
                                    FirstName = "Melca",
                                    MiddleName = "V.",
                                    LastName= "Chavez",
                                    Gender = Gender.Male,
                                },
                                Position = Position.BarangayCouncilor,
                                Committee = Committee.CommitteeOnFinanceAndAppropriation,
                                IsActive = true,
                            },
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "Hon.",
                                    FirstName = "Imelda",
                                    MiddleName = "S.",
                                    LastName= "Roca",
                                    Gender = Gender.Female,
                                },
                                Position = Position.BarangayCouncilor,
                                Committee = Committee.CommitteeOnHealthAndSanitation,
                                IsActive = true,
                            },
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "Hon.",
                                    FirstName = "Fe",
                                    MiddleName = "M.",
                                    LastName= "Quijano",
                                    Gender = Gender.Female,
                                },
                                Position = Position.BarangayCouncilor,
                                Committee = Committee.CommitteeOnInfrastractures,
                                IsActive = true,
                            },
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "Hon.",
                                    FirstName = "Sabina",
                                    MiddleName = "A.",
                                    LastName= "Teraza",
                                    Gender = Gender.Female,
                                },
                                Position = Position.BarangayCouncilor,
                                Committee = Committee.CommitteeOnLawsRulesAndRegulations,
                                IsActive = true,
                            },
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "Hon.",
                                    FirstName = "Rusita",
                                    MiddleName = "A.",
                                    LastName= "Luyten",
                                    Gender = Gender.Female,
                                },
                                Position = Position.BarangayCouncilor,
                                Committee = Committee.CommitteeOnLivelihodAndAgriculture,
                                IsActive = true,
                            },
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "Hon.",
                                    FirstName = "Wardiolita",
                                    MiddleName = "O.",
                                    LastName= "Chong",
                                    Gender = Gender.Female,
                                },
                                Position = Position.BarangayCouncilor,
                                Committee = Committee.CommitteeOnPeaceAndOrderAndPublicSafety,
                                IsActive = true,
                            },
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "Hon.",
                                    FirstName = "Jean",
                                    MiddleName = "",
                                    LastName= "Perpinan",
                                    Gender = Gender.Female,
                                },
                                Position = Position.SKChairman,
                            },
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "",
                                    FirstName = "Maria Rosario",
                                    MiddleName = "A.",
                                    LastName= "Ibanez",
                                    Gender = Gender.Female,
                                },
                                Position = Position.BarangayTreasurer,
                                IsActive = true,
                            },
                            new Official()
                            {
                                Person = new Person()
                                {
                                    Prefix = "",
                                    FirstName = "Evan Nathaniel",
                                    MiddleName = "O.",
                                    LastName= "Chong",
                                    Gender = Gender.Male,
                                },
                                Position = Position.BarangaySecretary,
                                IsActive = true,
                            },
                        }
                    },
                };

                foreach (var item in list)
                {
                    if (session.Query<Incumbent>().Any(x => x.Date == item.Date))
                        continue;

                    session.Save(item);
                }

                transaction.Commit();
            }
        }

        private void CreateDummyBlotter()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transation = session.BeginTransaction())
            {
                // TODO: find error on NHibernate.AssertionFailure: collection [CIS.Entities.Barangays.Position.Committees] was not processed by flus()
                //var firstThreeOfficial = session.QueryOver<Official>().Take(3).List();

                // work around
                //var firstThreeOfficial = session.Query<Official>()
                //    .Fetch(x => x.Committee)
                //    .Fetch(x => x.Position)
                //    .ThenFetch(x => x.Committees)
                //    .Take(3)
                //    .ToList();

                var current = session.Query<Incumbent>()
                    .OrderByDescending(x => x.Date)
                    .FirstOrDefault();

                var incumbents = session.Query<Incumbent>()
                    .FetchMany(x => x.Officials)
                    .ThenFetch(x => x.Position)
                    .ToList();


                var dummy = new List<Blotter>()
                {
                    new Blotter()
                    {
                        Complaint = "Theift",
                        Details = "Details",
                        Remarks = "Remarks",
                        Status = BlotterStatus.Open,
                        FiledOn = DateTime.Today,
                        OccuredOn = DateTime.Today.AddDays(-10),
                        Address = new Address()
                        {
                            Address1 = "Address1",
                            Address2 = "Address2",
                            Barangay = "Barangay",
                            City = "City",
                            Province = "Province"
                        },
                        Incumbent = incumbents.First(),
                        Officials = incumbents.First().Officials.Take(4),
                        Complainants = new List<Citizen>()
                        {
                            new Citizen()
                            {
                                Person = new Person()
                                {
                                    FirstName = "John",
                                    MiddleName = "",
                                    LastName = "Del Cardel"
                                },
                                AlsoKnownAs = "AlsoKnownAs1",
                                BirthPlace = "BirthPlace1",
                                Occupation = "Occupation1",
                                Religion = "Religion1",
                                Citizenship = "Citizenship1",
                                EmailAddress = "EmailAddress1",
                                TelephoneNumber = "TelephoneNumber1",
                                CellphoneNumber = "CellphoneNumber1",
                                CurrentAddress = new Address()
                                {
                                    Address1 = "Address11",
                                    Address2 = "Address21",
                                    Barangay = "Barangay1",
                                    City = "City1",
                                    Province = "Province1"
                                },
                                ProvincialAddress = new Address()
                                {
                                    Address1 = "Address11",
                                    Address2 = "Address21",
                                    Barangay = "Barangay1",
                                    City = "City1",
                                    Province = "Province1"
                                },
                            },
                            new Citizen()
                            {
                                Person = new Person()
                                {
                                    FirstName = "FirstName2",
                                    MiddleName = "MiddleName2",
                                    LastName = "LastName2"
                                },
                                AlsoKnownAs = "AlsoKnownAs2",
                                BirthPlace = "BirthPlace2",
                                Occupation = "Occupation2",
                                Religion = "Religion2",
                                Citizenship = "Citizenship2",
                                EmailAddress = "EmailAddress2",
                                TelephoneNumber = "TelephoneNumber2",
                                CellphoneNumber = "CellphoneNumber2",
                                CurrentAddress = new Address()
                                {
                                    Address1 = "Address12",
                                    Address2 = "Address22",
                                    Barangay = "Barangay2",
                                    City = "City2",
                                    Province = "Province2"
                                },
                                ProvincialAddress = new Address()
                                {
                                    Address1 = "Address12",
                                    Address2 = "Address22",
                                    Barangay = "Barangay2",
                                    City = "City2",
                                    Province = "Province2"
                                },
                            }
                        },
                        Respondents = new List<Citizen>()
                        {
                            new Citizen()
                            {
                                Person = new Person()
                                {
                                    FirstName = "FirstName1",
                                    MiddleName = "MiddleName1",
                                    LastName = "LastName1"
                                },
                                AlsoKnownAs = "AlsoKnownAs1",
                                BirthPlace = "BirthPlace1",
                                Occupation = "Occupation1",
                                Religion = "Religion1",
                                Citizenship = "Citizenship1",
                                EmailAddress = "EmailAddress1",
                                TelephoneNumber = "TelephoneNumber1",
                                CellphoneNumber = "CellphoneNumber1",
                                CurrentAddress = new Address()
                                {
                                    Address1 = "Address11",
                                    Address2 = "Address21",
                                    Barangay = "Barangay1",
                                    City = "City1",
                                    Province = "Province1"
                                },
                                ProvincialAddress = new Address()
                                {
                                    Address1 = "Address11",
                                    Address2 = "Address21",
                                    Barangay = "Barangay1",
                                    City = "City1",
                                    Province = "Province1"
                                },
                            },
                            new Citizen()
                            {
                                Person = new Person()
                                {
                                    FirstName = "FirstName2",
                                    MiddleName = "MiddleName2",
                                    LastName = "LastName2"
                                },
                                AlsoKnownAs = "AlsoKnownAs2",
                                BirthPlace = "BirthPlace2",
                                Occupation = "Occupation2",
                                Religion = "Religion2",
                                Citizenship = "Citizenship2",
                                EmailAddress = "EmailAddress2",
                                TelephoneNumber = "TelephoneNumber2",
                                CellphoneNumber = "CellphoneNumber2",
                                CurrentAddress = new Address()
                                {
                                    Address1 = "Address12",
                                    Address2 = "Address22",
                                    Barangay = "Barangay2",
                                    City = "City2",
                                    Province = "Province2"
                                },
                                ProvincialAddress = new Address()
                                {
                                    Address1 = "Address12",
                                    Address2 = "Address22",
                                    Barangay = "Barangay2",
                                    City = "City2",
                                    Province = "Province2"
                                },
                            }
                        },
                        Witnesses = new List<Citizen>()
                        {
                            new Citizen()
                            {
                                Person = new Person()
                                {
                                    FirstName = "FirstName1",
                                    MiddleName = "MiddleName1",
                                    LastName = "LastName1"
                                },
                                AlsoKnownAs = "AlsoKnownAs1",
                                BirthPlace = "BirthPlace1",
                                Occupation = "Occupation1",
                                Religion = "Religion1",
                                Citizenship = "Citizenship1",
                                EmailAddress = "EmailAddress1",
                                TelephoneNumber = "TelephoneNumber1",
                                CellphoneNumber = "CellphoneNumber1",
                                CurrentAddress = new Address()
                                {
                                    Address1 = "Address11",
                                    Address2 = "Address21",
                                    Barangay = "Barangay1",
                                    City = "City1",
                                    Province = "Province1"
                                },
                                ProvincialAddress = new Address()
                                {
                                    Address1 = "Address11",
                                    Address2 = "Address21",
                                    Barangay = "Barangay1",
                                    City = "City1",
                                    Province = "Province1"
                                },
                            },
                            new Citizen()
                            {
                                Person = new Person()
                                {
                                    FirstName = "FirstName2",
                                    MiddleName = "MiddleName2",
                                    LastName = "LastName2"
                                },
                                AlsoKnownAs = "AlsoKnownAs2",
                                BirthPlace = "BirthPlace2",
                                Occupation = "Occupation2",
                                Religion = "Religion2",
                                Citizenship = "Citizenship2",
                                EmailAddress = "EmailAddress2",
                                TelephoneNumber = "TelephoneNumber2",
                                CellphoneNumber = "CellphoneNumber2",
                                CurrentAddress = new Address()
                                {
                                    Address1 = "Address12",
                                    Address2 = "Address22",
                                    Barangay = "Barangay2",
                                    City = "City2",
                                    Province = "Province2"
                                },
                                ProvincialAddress = new Address()
                                {
                                    Address1 = "Address12",
                                    Address2 = "Address22",
                                    Barangay = "Barangay2",
                                    City = "City2",
                                    Province = "Province2"
                                },
                            }
                        }
                    },
                    new Blotter()
                    {
                        Complaint = "Robbery",
                        Details = "Details",
                        Remarks = "Remarks",
                        Status = BlotterStatus.Open,
                        FiledOn = DateTime.Today,
                        OccuredOn = DateTime.Today.AddDays(-10),
                        Address = new Address()
                        {
                            Address1 = "Address1",
                            Address2 = "Address2",
                            Barangay = "Barangay",
                            City = "City",
                            Province = "Province"
                        },
                        Incumbent = incumbents.Last(),
                        Officials = incumbents.Last().Officials.Take(4),
                        Complainants = new List<Citizen>()
                        {
                            new Citizen()
                            {
                                Person = new Person()
                                {
                                    FirstName = "John",
                                    MiddleName = "",
                                    LastName = "Doe"
                                },
                                AlsoKnownAs = "AlsoKnownAs1",
                                BirthPlace = "BirthPlace1",
                                Occupation = "Occupation1",
                                Religion = "Religion1",
                                Citizenship = "Citizenship1",
                                EmailAddress = "EmailAddress1",
                                TelephoneNumber = "TelephoneNumber1",
                                CellphoneNumber = "CellphoneNumber1",
                                CurrentAddress = new Address()
                                {
                                    Address1 = "Address11",
                                    Address2 = "Address21",
                                    Barangay = "Barangay1",
                                    City = "City1",
                                    Province = "Province1"
                                },
                                ProvincialAddress = new Address()
                                {
                                    Address1 = "Address11",
                                    Address2 = "Address21",
                                    Barangay = "Barangay1",
                                    City = "City1",
                                    Province = "Province1"
                                },
                            },
                            new Citizen()
                            {
                                Person = new Person()
                                {
                                    FirstName = "FirstName2",
                                    MiddleName = "MiddleName2",
                                    LastName = "LastName2"
                                },
                                AlsoKnownAs = "AlsoKnownAs2",
                                BirthPlace = "BirthPlace2",
                                Occupation = "Occupation2",
                                Religion = "Religion2",
                                Citizenship = "Citizenship2",
                                EmailAddress = "EmailAddress2",
                                TelephoneNumber = "TelephoneNumber2",
                                CellphoneNumber = "CellphoneNumber2",
                                CurrentAddress = new Address()
                                {
                                    Address1 = "Address12",
                                    Address2 = "Address22",
                                    Barangay = "Barangay2",
                                    City = "City2",
                                    Province = "Province2"
                                },
                                ProvincialAddress = new Address()
                                {
                                    Address1 = "Address12",
                                    Address2 = "Address22",
                                    Barangay = "Barangay2",
                                    City = "City2",
                                    Province = "Province2"
                                },
                            }
                        },
                        Respondents = new List<Citizen>()
                        {
                            new Citizen()
                            {
                                Person = new Person()
                                {
                                    FirstName = "FirstName1",
                                    MiddleName = "MiddleName1",
                                    LastName = "LastName1"
                                },
                                AlsoKnownAs = "AlsoKnownAs1",
                                BirthPlace = "BirthPlace1",
                                Occupation = "Occupation1",
                                Religion = "Religion1",
                                Citizenship = "Citizenship1",
                                EmailAddress = "EmailAddress1",
                                TelephoneNumber = "TelephoneNumber1",
                                CellphoneNumber = "CellphoneNumber1",
                                CurrentAddress = new Address()
                                {
                                    Address1 = "Address11",
                                    Address2 = "Address21",
                                    Barangay = "Barangay1",
                                    City = "City1",
                                    Province = "Province1"
                                },
                                ProvincialAddress = new Address()
                                {
                                    Address1 = "Address11",
                                    Address2 = "Address21",
                                    Barangay = "Barangay1",
                                    City = "City1",
                                    Province = "Province1"
                                },
                            },
                            new Citizen()
                            {
                                Person = new Person()
                                {
                                    FirstName = "FirstName2",
                                    MiddleName = "MiddleName2",
                                    LastName = "LastName2"
                                },
                                AlsoKnownAs = "AlsoKnownAs2",
                                BirthPlace = "BirthPlace2",
                                Occupation = "Occupation2",
                                Religion = "Religion2",
                                Citizenship = "Citizenship2",
                                EmailAddress = "EmailAddress2",
                                TelephoneNumber = "TelephoneNumber2",
                                CellphoneNumber = "CellphoneNumber2",
                                CurrentAddress = new Address()
                                {
                                    Address1 = "Address12",
                                    Address2 = "Address22",
                                    Barangay = "Barangay2",
                                    City = "City2",
                                    Province = "Province2"
                                },
                                ProvincialAddress = new Address()
                                {
                                    Address1 = "Address12",
                                    Address2 = "Address22",
                                    Barangay = "Barangay2",
                                    City = "City2",
                                    Province = "Province2"
                                },
                            }
                        },
                        Witnesses = new List<Citizen>()
                        {
                            new Citizen()
                            {
                                Person = new Person()
                                {
                                    FirstName = "FirstName1",
                                    MiddleName = "MiddleName1",
                                    LastName = "LastName1"
                                },
                                AlsoKnownAs = "AlsoKnownAs1",
                                BirthPlace = "BirthPlace1",
                                Occupation = "Occupation1",
                                Religion = "Religion1",
                                Citizenship = "Citizenship1",
                                EmailAddress = "EmailAddress1",
                                TelephoneNumber = "TelephoneNumber1",
                                CellphoneNumber = "CellphoneNumber1",
                                CurrentAddress = new Address()
                                {
                                    Address1 = "Address11",
                                    Address2 = "Address21",
                                    Barangay = "Barangay1",
                                    City = "City1",
                                    Province = "Province1"
                                },
                                ProvincialAddress = new Address()
                                {
                                    Address1 = "Address11",
                                    Address2 = "Address21",
                                    Barangay = "Barangay1",
                                    City = "City1",
                                    Province = "Province1"
                                },
                            },
                            new Citizen()
                            {
                                Person = new Person()
                                {
                                    FirstName = "FirstName2",
                                    MiddleName = "MiddleName2",
                                    LastName = "LastName2"
                                },
                                AlsoKnownAs = "AlsoKnownAs2",
                                BirthPlace = "BirthPlace2",
                                Occupation = "Occupation2",
                                Religion = "Religion2",
                                Citizenship = "Citizenship2",
                                EmailAddress = "EmailAddress2",
                                TelephoneNumber = "TelephoneNumber2",
                                CellphoneNumber = "CellphoneNumber2",
                                CurrentAddress = new Address()
                                {
                                    Address1 = "Address12",
                                    Address2 = "Address22",
                                    Barangay = "Barangay2",
                                    City = "City2",
                                    Province = "Province2"
                                },
                                ProvincialAddress = new Address()
                                {
                                    Address1 = "Address12",
                                    Address2 = "Address22",
                                    Barangay = "Barangay2",
                                    City = "City2",
                                    Province = "Province2"
                                },
                            }
                        }
                    }
                };

                foreach (var item in dummy)
                {
                    if (session.Query<Blotter>().Any(x => x.Complaint == item.Complaint) == false)
                        session.Save(item);
                }

                transation.Commit();
            }

            using (var session = this.SessionFactory.OpenSession())
            using (var transation = session.BeginTransaction())
            {
                var blotter = session.Query<Blotter>().First();
                NHibernateUtil.Initialize(blotter.Officials);
                NHibernateUtil.Initialize(blotter.Complainants);
                NHibernateUtil.Initialize(blotter.Respondents);
                NHibernateUtil.Initialize(blotter.Witnesses);

                if (blotter.Officials.Count() > 1)
                    blotter.Officials = blotter.Officials.Take(blotter.Officials.Count() - 1);

                if (blotter.Complainants.Count() > 1)
                    blotter.Complainants = blotter.Complainants.Take(blotter.Complainants.Count() - 1);

                if (blotter.Respondents.Count() > 1)
                    blotter.Respondents = blotter.Respondents.Take(blotter.Respondents.Count() - 1);

                if (blotter.Witnesses.Count() > 1)
                    blotter.Witnesses = blotter.Witnesses.Take(blotter.Witnesses.Count() - 1);

                transation.Commit();
            }
        }

        public virtual void Search()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var items = (IReactiveList<BlotterListItemViewModel>)null;

                var blotterAlias = (Blotter)null;
                var query = session.QueryOver<Blotter>(() => blotterAlias);

                switch (this.ViewModel.Criteria.SearchPersonBy)
                {
                    case ConcernedPersonType.Complainant:
                        var complainantAlias = (Citizen)null;
                        query = query.Left.JoinAlias(() => blotterAlias.Complainants, () => complainantAlias);

                        if (!string.IsNullOrWhiteSpace(this.ViewModel.Criteria.FirstName))
                            query = query.WhereRestrictionOn(() => complainantAlias.Person.FirstName).IsInsensitiveLike(this.ViewModel.Criteria.FirstName, MatchMode.Start);

                        if (!string.IsNullOrWhiteSpace(this.ViewModel.Criteria.MiddleName))
                            query = query.WhereRestrictionOn(() => complainantAlias.Person.MiddleName).IsInsensitiveLike(this.ViewModel.Criteria.MiddleName, MatchMode.Start);

                        if (!string.IsNullOrWhiteSpace(this.ViewModel.Criteria.LastName))
                            query = query.WhereRestrictionOn(() => complainantAlias.Person.LastName).IsInsensitiveLike(this.ViewModel.Criteria.LastName, MatchMode.Start);

                        items = query.TransformUsing(Transformers.DistinctRootEntity).List()
                            .Select(x => new BlotterListItemViewModel()
                            {
                                Id = x.Id,
                                FiledOn = x.FiledOn,
                                Complaint = x.Complaint,
                                ConcernedPersons = x.Complainants
                                    .Select(o => o.Person.Fullname)
                                    .ToArray(),
                                ConcernedPersonType = this.ViewModel.Criteria.SearchPersonBy,
                            })
                            .ToReactiveList();

                        break;
                    case ConcernedPersonType.Respondent:
                        var respondentAlias = (Citizen)null;
                        query = query.Left.JoinAlias(() => blotterAlias.Respondents, () => respondentAlias);

                        if (!string.IsNullOrWhiteSpace(this.ViewModel.Criteria.FirstName))
                            query = query.WhereRestrictionOn(() => respondentAlias.Person.FirstName).IsInsensitiveLike(this.ViewModel.Criteria.FirstName, MatchMode.Start);

                        if (!string.IsNullOrWhiteSpace(this.ViewModel.Criteria.MiddleName))
                            query = query.WhereRestrictionOn(() => respondentAlias.Person.MiddleName).IsInsensitiveLike(this.ViewModel.Criteria.MiddleName, MatchMode.Start);

                        if (!string.IsNullOrWhiteSpace(this.ViewModel.Criteria.LastName))
                            query = query.WhereRestrictionOn(() => respondentAlias.Person.LastName).IsInsensitiveLike(this.ViewModel.Criteria.LastName, MatchMode.Start);

                        items = query.TransformUsing(Transformers.DistinctRootEntity).List()
                         .Select(x => new BlotterListItemViewModel()
                         {
                             Id = x.Id,
                             FiledOn = x.FiledOn,
                             Complaint = x.Complaint,
                             ConcernedPersons = x.Respondents
                                 .Select(o => o.Person.Fullname)
                                 .ToArray(),
                             ConcernedPersonType = this.ViewModel.Criteria.SearchPersonBy,
                         })
                         .ToReactiveList();

                        break;
                    case ConcernedPersonType.Witness:
                        var witnesstAlias = (Citizen)null;
                        query = query.Left.JoinAlias(() => blotterAlias.Witnesses, () => witnesstAlias);

                        if (!string.IsNullOrWhiteSpace(this.ViewModel.Criteria.FirstName))
                            query = query.WhereRestrictionOn(() => witnesstAlias.Person.FirstName).IsInsensitiveLike(this.ViewModel.Criteria.FirstName, MatchMode.Start);

                        if (!string.IsNullOrWhiteSpace(this.ViewModel.Criteria.MiddleName))
                            query = query.WhereRestrictionOn(() => witnesstAlias.Person.MiddleName).IsInsensitiveLike(this.ViewModel.Criteria.MiddleName, MatchMode.Start);

                        if (!string.IsNullOrWhiteSpace(this.ViewModel.Criteria.LastName))
                            query = query.WhereRestrictionOn(() => witnesstAlias.Person.LastName).IsInsensitiveLike(this.ViewModel.Criteria.LastName, MatchMode.Start);

                        items = query.TransformUsing(Transformers.DistinctRootEntity).List()
                           .Select(x => new BlotterListItemViewModel()
                           {
                               Id = x.Id,
                               FiledOn = x.FiledOn,
                               Complaint = x.Complaint,
                               ConcernedPersons = x.Witnesses
                                   .Select(o => o.Person.Fullname)
                                   .ToArray(),
                               ConcernedPersonType = this.ViewModel.Criteria.SearchPersonBy,
                           })
                           .ToReactiveList();

                        break;
                }

                this.ViewModel.Items = items;

                transaction.Commit();
            }
        }

        public virtual void Create()
        {
            var dialog = new DialogService<BlotterViewModel>();
            var result = dialog.ShowModal(this, "Create Blotter", null);
            if (result != null)
                this.Search();
        }

        public virtual void Edit(BlotterListItemViewModel item)
        {
            var dialog = new DialogService<BlotterViewModel>();
            dialog.ViewModel.Load.Execute(item.Id);
            var result = dialog.ShowModal(this, "Edit Blotter", null);
            if (result != null)
                this.Search();
        }

        public virtual void Delete(BlotterListItemViewModel item)
        {
            try
            {
                //this.ViewModel.SelectedItem = item;
                //var selected = this.ViewModel.SelectedItem;
                //if (selected == null)
                //    return;

                //var message = string.Format("Do you want to delete warrant for {0} with case {1}", selected.Suspect, selected.Crime);
                //var confirmed = this.MessageBox.Confirm(message, "Delete");
                //if (confirmed == false)
                //    return;

                //using (var session = this.SessionFactory.OpenSession())
                //using (var transaction = session.BeginTransaction())
                //{
                //    var query = session.Query<Warrant>()
                //        .Where(x => x.Id == selected.Id)
                //        .FetchMany(x => x.Suspects)
                //        .ToFutureValue();

                //    var warrant = query.Value;
                //    var suspect = warrant.Suspects.FirstOrDefault(x => x.Id == selected.SuspectId);
                //    if (suspect != null)
                //    {
                //        warrant.DeleteSuspect(suspect);
                //        session.Delete(suspect);
                //    }

                //    if (warrant.Suspects.Count() == 0)
                //        session.Delete(warrant);

                //    transaction.Commit();
                //}

                //this.MessageBox.Inform("Delete has been successfully completed.");

                //this.Populate();
            }
            catch (GenericADOException)
            {
                //throw new InvalidOperationException(string.Format("Unable to delete. Suspect {0} may already be in use.", item.Suspect));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
