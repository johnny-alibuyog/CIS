using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CIS.Store.Domain.Entities;
using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;

namespace CIS.Store.Services.Warrants
{
    public class WarrantService : Service
    {
        public virtual GetWarrantResponse Get(GetWarrantRequest request)
        {
            return new GetWarrantResponse()
            {
                Warrants = request.Id > 0
                    ? new Warrant[] { Db.SingleById<Warrant>(request.Id) }
                    : Db.Select<Warrant>().ToArray()
            };
        }

        public virtual InsertWarrantResponse Post(InsertWarrantRequest request)
        {
            var warrant = request.Warrant.Id == 0
                ? Db.Single<Warrant>(x => x.Where(o =>
                    o.Crime == request.Warrant.Crime &&
                    o.SuspectFirstName == request.Warrant.SuspectFirstName &&
                    o.SuspectMiddleName == request.Warrant.SuspectMiddleName &&
                    o.SuspectLastName == request.Warrant.SuspectLastName &&
                    o.SuspectSuffix == request.Warrant.SuspectSuffix
                ))
                : Db.SingleById<Warrant>(request.Warrant.Id);

            if (warrant != null)
            {
                request.Warrant.Id = warrant.Id;
                warrant.PopulateWith(request.Warrant);
                var count = Db.Update<Warrant>(warrant);
                var remarks = string.Format("updated count: {0}", count);
                Db.Insert<AutditTrail>(new AutditTrail("dummyUser", "dummyOrigin", "WarrantService", "InsertWarrant", remarks));
            }
            else
            {
                request.Warrant.Id = Db.Insert<Warrant>(request.Warrant, selectIdentity: true);
                var remarks = string.Format("inserted id: {0}", warrant.Id);
                Db.Insert<AutditTrail>(new AutditTrail("dummyUser", "dummyOrigin", "WarrantService", "InsertWarrant", remarks));
            }

            return new InsertWarrantResponse() { Warrant = request.Warrant };
        }

        public virtual UpdateWarrantResponse Patch(UpdateWarrantRequest request)
        {
            var count = Db.Update<Warrant>(request.Warrant);
            var remarks = string.Format("updated count: {0}", count);
            Db.Insert<AutditTrail>(new AutditTrail("dummyUser", "dummyOrigin", "WarrantService", "UpdateWarrant", remarks));

            return new UpdateWarrantResponse() { Warrant = request.Warrant };
        }

        public virtual PullWarrantResponse Patch(PullWarrantRequest request)
        {
            var warrants = Db.SelectByIds<Warrant>(request.MissingIds);
            if (request.FetchSize > warrants.Count)
            {
                warrants.AddRange(Db.Select<Warrant>(x => x
                    .Where(o => o.Id > request.LatestId)
                    .OrderBy(o => o.Id)
                    .Limit(request.FetchSize - warrants.Count))
                );
            }

            if (warrants.Count > 0)
            {
                var remarks = string.Format("pulled count: {0}", warrants.Count);
                Db.Insert<AutditTrail>(new AutditTrail("dummyUser", "dummyOrigin", "WarrantService", "PullWarrant", remarks));
            }

            return new PullWarrantResponse() { Warrants = warrants.ToArray() };
        }

        public virtual PushWarrantResponse Patch(PushWarrantRequest request)
        {
            var pulledCount = 0;
            var pushedCount = 0;

            foreach (var item in request.Warrants)
            {
                var warrant = Db.Single<Warrant>(x => x.Where(o =>
                    o.Crime == item.Crime &&
                    o.SuspectFirstName == item.SuspectFirstName &&
                    o.SuspectMiddleName == item.SuspectMiddleName &&
                    o.SuspectLastName == item.SuspectLastName &&
                    o.SuspectSuffix == item.SuspectSuffix
                ));

                if (warrant != null)
                {
                    item.PopulateWith(warrant);
                    pulledCount++;
                }
                else
                {
                    item.Id = Db.Insert<Warrant>(item, selectIdentity: true);
                    pushedCount++;
                }
            }

            if (pulledCount > 0 || pushedCount > 0)
            {
                var remarks = string.Format("pulled count: {0} pushed count: {1}", pulledCount, pushedCount);
                Db.Insert<AutditTrail>(new AutditTrail("dummyUser", "dummyOrigin", "WarrantService", "PushWarrant", remarks));
            }

            return new PushWarrantResponse() { Warrants = request.Warrants };
        }

        public virtual DeleteWarrantResponse Delete(DeleteWarrantRequest request)
        {
            var count = Db.DeleteById<Warrant>(request.Id);
            var remarks = string.Empty;
            if (count > 0)
                remarks = string.Format("deleted id: {0}", request.Id);
            else
                remarks = string.Format("unable to delete id: {0}", request.Id);

            Db.Insert<AutditTrail>(new AutditTrail("dummyUser", "dummyOrigin", "WarrantService", "PushWarrant", remarks));

            return new DeleteWarrantResponse();
        }
    }
}