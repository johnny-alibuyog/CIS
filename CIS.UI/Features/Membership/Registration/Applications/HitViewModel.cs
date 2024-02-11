using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Domain.Membership;
using CIS.UI.Features.Common.Address;
using CIS.UI.Features.Common.Person;
using CIS.Core.Utility.Extention;
using CIS.Core.Domain.Common;

namespace CIS.UI.Features.Membership.Registration.Applications
{
    public abstract class HitViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual bool IsIdentifiedHit { get; set; }

        public virtual PersonViewModel Applicant { get; set; }

        public abstract HitScore HitScore { get; }

        public abstract string DisplayTitle { get; }

        public abstract string DisplayLabel { get; }

        public abstract string DisplayResult { get; }

        public HitViewModel()
        {
            this.IsIdentifiedHit = true;
        }

        protected HitScore ComputeHitScore(PersonViewModel person1, PersonViewModel person2)
        {
            if (person1 == null || person2 == null)
                return HitScore.Partial;

            if (person1.FirstName.IsEqualTo(person2.FirstName) &&
                person1.MiddleName.IsEqualTo(person2.MiddleName) &&
                person1.LastName.IsEqualTo(person2.LastName))
                return HitScore.Perfect;
            else
                return HitScore.Partial;
        }

        public override string ToString()
        {
            return this.DisplayResult;
        }

        //public override object SerializeWith(object instance)
        //{
        //    if (instance == null)
        //        return null;

        //    if (instance is HitViewModel)
        //    {
        //        var source = instance as HitViewModel;
        //        var target = this;

        //        target.Id = source.Id;
        //        target.IsIdentical = source.IsIdentical;
        //        //target.HitScore = source.HitScore;
        //        return target;
        //    }
        //    //else if (instance is Address)
        //    //{
        //    //    var source = instance as Address;
        //    //    var target = this;

        //    //    target.Address1 = source.Address1;
        //    //    target.Address2 = source.Address2;
        //    //    target.Barangay = source.Barangay;
        //    //    target.City = source.City;
        //    //    target.Province = source.Province;
        //    //    return target;
        //    //}

        //    return null;
        //}

        //public override object DeserializeInto(object instance)
        //{
        //    if (instance == null)
        //        return null;

        //    if (instance is AddressViewModel)
        //    {
        //        var source = this;
        //        var target = instance as AddressViewModel;

        //        target.SerializeWith(source);
        //        return target;
        //    }
        //    //else if (instance is Address)
        //    //{
        //    //    var source = this;
        //    //    var target = instance as Address;

        //    //    target.Address1 = source.Address1;
        //    //    target.Address2 = source.Address2;
        //    //    target.Barangay = source.Barangay;
        //    //    target.City = source.City;
        //    //    target.Province = source.Province;

        //    //    return target;
        //    //}

        //    return null;
        //}
    }
}
