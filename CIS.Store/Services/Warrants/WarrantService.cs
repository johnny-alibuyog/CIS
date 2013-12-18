using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CIS.Store.Domain.Entities;
using ServiceStack;

namespace CIS.Store.Services.Warrants
{
    public class WarrantService : Service, 
        IGet<GetWarrantRequest>, 
        //IPut<InsertWarrantRequest>, 
        IPost<InsertWarrantRequest>, 
        IPatch<UpdateWarrantRequest>, 
        IDelete<DeleteWarrantRequest>
    {
        //public virtual GetWarrantResponse Get(GetWarrantRequest request)
        //{
        //    return new GetWarrantResponse()
        //    {
        //        Warrants = new Warrant[]
        //        {
        //            new Warrant(){ Id = 1, Code = "01", Crime = "Crime1", SuspectFirstName = "FirstName1", SuspectMiddleName = "MiddleName1", SuspectLastName = "LastName1", SuspectSuffix = "Suffix1" },
        //            new Warrant(){ Id = 1, Code = "01", Crime = "Crime1", SuspectFirstName = "FirstName1", SuspectMiddleName = "MiddleName1", SuspectLastName = "LastName1", SuspectSuffix = "Suffix1" },
        //            new Warrant(){ Id = 1, Code = "01", Crime = "Crime1", SuspectFirstName = "FirstName1", SuspectMiddleName = "MiddleName1", SuspectLastName = "LastName1", SuspectSuffix = "Suffix1" },
        //            new Warrant(){ Id = 1, Code = "01", Crime = "Crime1", SuspectFirstName = "FirstName1", SuspectMiddleName = "MiddleName1", SuspectLastName = "LastName1", SuspectSuffix = "Suffix1" },
        //            new Warrant(){ Id = 1, Code = "01", Crime = "Crime1", SuspectFirstName = "FirstName1", SuspectMiddleName = "MiddleName1", SuspectLastName = "LastName1", SuspectSuffix = "Suffix1" },
        //            new Warrant(){ Id = 1, Code = "01", Crime = "Crime1", SuspectFirstName = "FirstName1", SuspectMiddleName = "MiddleName1", SuspectLastName = "LastName1", SuspectSuffix = "Suffix1" },
        //            new Warrant(){ Id = 1, Code = "01", Crime = "Crime1", SuspectFirstName = "FirstName1", SuspectMiddleName = "MiddleName1", SuspectLastName = "LastName1", SuspectSuffix = "Suffix1" },
        //        }
        //    };
        //}

        //public virtual InsertWarrantResponse Post(InsertWarrantRequest request)
        //{
        //    return new InsertWarrantResponse() { Warrant = new Warrant() };
        //}

        //public virtual UpdateWarrantResponse Patch(UpdateWarrantRequest request)
        //{
        //    return new UpdateWarrantResponse() { Warrant = new Warrant() };
        //}

        //public virtual DeleteWarrantResponse Delete(DeleteWarrantRequest request)
        //{
        //    return new DeleteWarrantResponse();
        //}

        public object Get(GetWarrantRequest request)
        {
            return new GetWarrantResponse()
            {
                Warrants = new Warrant[]
                {
                    new Warrant(){ Id = 1, Code = "01", Crime = "Crime1", SuspectFirstName = "FirstName1", SuspectMiddleName = "MiddleName1", SuspectLastName = "LastName1", SuspectSuffix = "Suffix1" },
                    new Warrant(){ Id = 1, Code = "01", Crime = "Crime1", SuspectFirstName = "FirstName1", SuspectMiddleName = "MiddleName1", SuspectLastName = "LastName1", SuspectSuffix = "Suffix1" },
                    new Warrant(){ Id = 1, Code = "01", Crime = "Crime1", SuspectFirstName = "FirstName1", SuspectMiddleName = "MiddleName1", SuspectLastName = "LastName1", SuspectSuffix = "Suffix1" },
                    new Warrant(){ Id = 1, Code = "01", Crime = "Crime1", SuspectFirstName = "FirstName1", SuspectMiddleName = "MiddleName1", SuspectLastName = "LastName1", SuspectSuffix = "Suffix1" },
                    new Warrant(){ Id = 1, Code = "01", Crime = "Crime1", SuspectFirstName = "FirstName1", SuspectMiddleName = "MiddleName1", SuspectLastName = "LastName1", SuspectSuffix = "Suffix1" },
                    new Warrant(){ Id = 1, Code = "01", Crime = "Crime1", SuspectFirstName = "FirstName1", SuspectMiddleName = "MiddleName1", SuspectLastName = "LastName1", SuspectSuffix = "Suffix1" },
                    new Warrant(){ Id = 1, Code = "01", Crime = "Crime1", SuspectFirstName = "FirstName1", SuspectMiddleName = "MiddleName1", SuspectLastName = "LastName1", SuspectSuffix = "Suffix1" },
                }
            };
        }

        //public object Put(InsertWarrantRequest request)
        //{
        //    return new InsertWarrantResponse() { Warrant = new Warrant() };
        //}

        public object Post(InsertWarrantRequest request)
        {
            return new InsertWarrantResponse() { Warrant = new Warrant() };
        }

        public object Patch(UpdateWarrantRequest request)
        {
            return new UpdateWarrantResponse() { Warrant = new Warrant() };
        }

        public object Delete(DeleteWarrantRequest request)
        {
            return new DeleteWarrantResponse();
        }
    }
}