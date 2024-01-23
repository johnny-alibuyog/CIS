using CIS.Core.Entities.Barangays;
using CIS.Core.Entities.Commons;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Features.Commons.Addresses;
using CIS.UI.Features.Commons.Biometrics;
using CIS.UI.Features.Commons.Persons;
using CIS.UI.Utilities.Extentions;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace CIS.UI.Features.Barangays.Citizens;

public class CitizenViewModel : ViewModelBase
{
    private readonly CitizenController _controller;

    public virtual Guid Id { get; set; }

    [Valid]
    public virtual PersonViewModel Person { get; set; }

    public virtual CivilStatus? CivilStatus { get; set; }

    public virtual IReactiveList<CivilStatus> CivilStatuses { get; private set; }

    public virtual string AlsoKnownAs { get; set; }

    public virtual string BirthPlace { get; set; }

    public virtual string Occupation { get; set; }

    public virtual string Religion { get; set; }

    public virtual string Citizenship { get; set; }

    public virtual string EmailAddress { get; set; }

    public virtual string TelephoneNumber { get; set; }

    public virtual string CellphoneNumber { get; set; }

    [Valid]
    public virtual AddressViewModel CurrentAddress { get; set; }

    [Valid]
    public virtual AddressViewModel ProvincialAddress { get; set; }

    public virtual Dictionary<FingerViewModel, BitmapSource> FingerImages { get; set; }

    public virtual BitmapSource PictureImage { get; set; }

    public virtual bool IsPictureImageChanged { get; set; }

    public virtual BitmapSource SignatureImage { get; set; }

    public virtual bool IsSignatureImageChanged { get; set; }

    public virtual BitmapSource FingerImage { get; set; }

    public virtual bool IsFingerImageChanged { get; set; }

    public virtual IReactiveCommand CapturePicture { get; set; }

    public virtual IReactiveCommand CaptureSignature { get; set; }

    public virtual IReactiveCommand CaptureFingerPrint { get; set; }

    public virtual IReactiveCommand Load { get; set; }

    public virtual IReactiveCommand Save { get; set; }

    public CitizenViewModel()
    {
        this.CivilStatuses = Enum.GetValues(typeof(CivilStatus))
            .Cast<CivilStatus>()
            .ToReactiveList();

        this.Person = new PersonViewModel();
        this.FingerImages = new Dictionary<FingerViewModel, BitmapSource>()
        {
            { FingerViewModel.RightThumb, null },
            { FingerViewModel.RightIndex, null },
            { FingerViewModel.RightMiddle, null },
            { FingerViewModel.RightRing, null },
            { FingerViewModel.RightPinky, null },
            { FingerViewModel.LeftThumb, null },
            { FingerViewModel.LeftIndex, null },
            { FingerViewModel.LeftMiddle, null },
            { FingerViewModel.LeftRing, null },
            { FingerViewModel.LeftPinky, null },
        };
        this.CurrentAddress = new AddressViewModel();
        this.ProvincialAddress = new AddressViewModel();

        this.WhenAnyValue(
            x => x.Person.IsValid,
            x => x.CurrentAddress.IsValid,
            x => x.ProvincialAddress.IsValid,
            (
                isPersonValid, 
                isCurrentAddressValid, 
                isProvincialAddressValid
            ) => true
        )
        .Subscribe(_ => this.Revalidate());


        _controller = IoC.Container.Resolve<CitizenController>(new ViewModelDependency(this));
    }

    public override object SerializeWith(object instance)
    {
        if (instance == null)
            return null;

        if (instance is CitizenViewModel)
        {
            var source = instance as CitizenViewModel;
            var target = this;

            target.Id = source.Id;
            target.Person.SerializeWith(source.Person);
            target.CivilStatus = source.CivilStatus;
            target.AlsoKnownAs = source.AlsoKnownAs;
            target.BirthPlace = source.BirthPlace;
            target.Occupation = source.Occupation;
            target.Religion = source.Religion;
            target.Citizenship = source.Citizenship;
            target.EmailAddress = source.EmailAddress;
            target.TelephoneNumber = source.TelephoneNumber;
            target.CellphoneNumber = source.CellphoneNumber;
            target.CurrentAddress.SerializeWith(source.CurrentAddress);
            target.ProvincialAddress.SerializeWith(source.ProvincialAddress);
            target.FingerImages[FingerViewModel.RightThumb] = source.FingerImages[FingerViewModel.RightThumb];
            target.FingerImages[FingerViewModel.RightIndex] = source.FingerImages[FingerViewModel.RightIndex];
            target.FingerImages[FingerViewModel.RightMiddle] = source.FingerImages[FingerViewModel.RightMiddle];
            target.FingerImages[FingerViewModel.RightRing] = source.FingerImages[FingerViewModel.RightRing];
            target.FingerImages[FingerViewModel.RightPinky] = source.FingerImages[FingerViewModel.RightPinky];
            target.FingerImages[FingerViewModel.LeftThumb] = source.FingerImages[FingerViewModel.LeftThumb];
            target.FingerImages[FingerViewModel.LeftIndex] = source.FingerImages[FingerViewModel.LeftIndex];
            target.FingerImages[FingerViewModel.LeftMiddle] = source.FingerImages[FingerViewModel.LeftMiddle];
            target.FingerImages[FingerViewModel.LeftRing] = source.FingerImages[FingerViewModel.LeftRing];
            target.FingerImages[FingerViewModel.LeftPinky] = source.FingerImages[FingerViewModel.LeftPinky];
            target.PictureImage = source.PictureImage;
            target.SignatureImage = source.SignatureImage;
            target.FingerImage = source.FingerImage;

            return target;
        }
        else if (instance is Citizen)
        {
            var source = instance as Citizen;
            var target = this;

            target.Id = source.Id;
            target.Person.SerializeWith(source.Person);
            target.CivilStatus = source.CivilStatus;
            target.AlsoKnownAs = source.AlsoKnownAs;
            target.BirthPlace = source.BirthPlace;
            target.Occupation = source.Occupation;
            target.Religion = source.Religion;
            target.Citizenship = source.Citizenship;
            target.EmailAddress = source.EmailAddress;
            target.TelephoneNumber = source.TelephoneNumber;
            target.CellphoneNumber = source.CellphoneNumber;
            target.CurrentAddress.SerializeWith(source.CurrentAddress);
            target.ProvincialAddress.SerializeWith(source.ProvincialAddress);
            target.FingerImages[FingerViewModel.RightThumb] = source.FingerPrint.RightThumb.Image.ToBitmapSource();
            target.FingerImages[FingerViewModel.RightIndex] = source.FingerPrint.RightIndex.Image.ToBitmapSource();
            target.FingerImages[FingerViewModel.RightMiddle] = source.FingerPrint.RightMiddle.Image.ToBitmapSource();
            target.FingerImages[FingerViewModel.RightRing] = source.FingerPrint.RightRing.Image.ToBitmapSource();
            target.FingerImages[FingerViewModel.RightPinky] = source.FingerPrint.RightPinky.Image.ToBitmapSource();
            target.FingerImages[FingerViewModel.LeftThumb] = source.FingerPrint.LeftThumb.Image.ToBitmapSource();
            target.FingerImages[FingerViewModel.LeftIndex] = source.FingerPrint.LeftIndex.Image.ToBitmapSource();
            target.FingerImages[FingerViewModel.LeftMiddle] = source.FingerPrint.LeftMiddle.Image.ToBitmapSource();
            target.FingerImages[FingerViewModel.LeftRing] = source.FingerPrint.LeftRing.Image.ToBitmapSource();
            target.FingerImages[FingerViewModel.LeftPinky] = source.FingerPrint.LeftPinky.Image.ToBitmapSource();
            target.PictureImage = source.Pictures.Any() ? source.Pictures.Last().Image.ToBitmapSource() : null;
            target.SignatureImage = source.Signatures.Any() ? source.Signatures.Last().Image.ToBitmapSource() : null;
            target.FingerImage = source.FingerPrint.RightThumb.Image.ToBitmapSource();

            return target;
        }

        return null;
    }

    public override object DeserializeInto(object instance)
    {
        if (instance == null)
            return null;

        if (instance is CitizenViewModel)
        {
            var source = this;
            var target = instance as CitizenViewModel;

            source.SerializeWith(target);
            return target;
        }
        else if (instance is Citizen)
        {
            var source = this;
            var target = instance as Citizen;

            target.WithId(source.Id);
            target.Person = (Person)source.Person.DeserializeInto(new Person());
            target.CivilStatus = source.CivilStatus;
            target.AlsoKnownAs = source.AlsoKnownAs;
            target.BirthPlace = source.BirthPlace;
            target.Occupation = source.Occupation;
            target.Religion = source.Religion;
            target.Citizenship = source.Citizenship;
            target.EmailAddress = source.EmailAddress;
            target.TelephoneNumber = source.TelephoneNumber;
            target.CellphoneNumber = source.CellphoneNumber;
            target.CurrentAddress = (Address)source.CurrentAddress.DeserializeInto(new Address());
            target.ProvincialAddress = (Address)source.ProvincialAddress.DeserializeInto(new Address());

            if (source.IsFingerImageChanged)
            {
                target.FingerPrint.RightThumb.Image = source.FingerImages[FingerViewModel.RightThumb].ToImage();
                target.FingerPrint.RightIndex.Image = source.FingerImages[FingerViewModel.RightIndex].ToImage();
                target.FingerPrint.RightMiddle.Image = source.FingerImages[FingerViewModel.RightMiddle].ToImage();
                target.FingerPrint.RightRing.Image = source.FingerImages[FingerViewModel.RightRing].ToImage();
                target.FingerPrint.RightPinky.Image = source.FingerImages[FingerViewModel.RightPinky].ToImage();
                target.FingerPrint.LeftThumb.Image = source.FingerImages[FingerViewModel.LeftThumb].ToImage();
                target.FingerPrint.LeftIndex.Image = source.FingerImages[FingerViewModel.LeftIndex].ToImage();
                target.FingerPrint.LeftMiddle.Image = source.FingerImages[FingerViewModel.LeftMiddle].ToImage();
                target.FingerPrint.LeftRing.Image = source.FingerImages[FingerViewModel.LeftRing].ToImage();
                target.FingerPrint.LeftPinky.Image = source.FingerImages[FingerViewModel.LeftPinky].ToImage();
            }

            if (source.IsPictureImageChanged)
            {
                target.AddPicture(new ImageBlob(source.PictureImage.ToImage()));
            }

            if (source.IsSignatureImageChanged)
            {
                target.AddSignature(new ImageBlob(source.SignatureImage.ToImage()));
            }

            return target;
        }

        return null;
    }
}
