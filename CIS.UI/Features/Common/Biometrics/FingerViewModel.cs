﻿using CIS.Core.Domain.Common;
using System.Collections.Generic;
using System.Linq;

namespace CIS.UI.Features.Common.Biometrics;

public class FingerViewModel : ViewModelBase
{
    public virtual string Id { get; private set; }

    public virtual string Name { get; private set; }

    public virtual string ImageUri { get; private set; }

    #region Static Members

    public static readonly FingerViewModel RightThumb = new()
    {
        Id = Finger.RightThumb.Id,
        Name = Finger.RightThumb.Name,
        ImageUri = Finger.RightThumb.ImageUri
    };
    public static readonly FingerViewModel RightIndex = new()
    {
        Id = Finger.RightIndex.Id,
        Name = Finger.RightIndex.Name,
        ImageUri = Finger.RightIndex.ImageUri
    };
    public static readonly FingerViewModel RightMiddle = new()
    {
        Id = Finger.RightMiddle.Id,
        Name = Finger.RightMiddle.Name,
        ImageUri = Finger.RightMiddle.ImageUri
    };
    public static readonly FingerViewModel RightRing = new()
    {
        Id = Finger.RightRing.Id,
        Name = Finger.RightRing.Name,
        ImageUri = Finger.RightRing.ImageUri
    };
    public static readonly FingerViewModel RightPinky = new()
    {
        Id = Finger.RightPinky.Id,
        Name = Finger.RightPinky.Name,
        ImageUri = Finger.RightPinky.ImageUri
    };
    public static readonly FingerViewModel LeftThumb = new()
    {
        Id = Finger.LeftThumb.Id,
        Name = Finger.LeftThumb.Name,
        ImageUri = Finger.LeftThumb.ImageUri
    };
    public static readonly FingerViewModel LeftIndex = new()
    {
        Id = Finger.LeftIndex.Id,
        Name = Finger.LeftIndex.Name,
        ImageUri = Finger.LeftIndex.ImageUri
    };
    public static readonly FingerViewModel LeftMiddle = new()
    {
        Id = Finger.LeftMiddle.Id,
        Name = Finger.LeftMiddle.Name,
        ImageUri = Finger.LeftMiddle.ImageUri
    };
    public static readonly FingerViewModel LeftRing = new()
    {
        Id = Finger.LeftRing.Id,
        Name = Finger.LeftRing.Name,
        ImageUri = Finger.LeftRing.ImageUri
    };
    public static readonly FingerViewModel LeftPinky = new()
    {
        Id = Finger.LeftPinky.Id,
        Name = Finger.LeftPinky.Name,
        ImageUri = Finger.LeftPinky.ImageUri
    };

    public static readonly FingerViewModel[] All =
    [
        FingerViewModel.RightThumb,
        FingerViewModel.RightIndex,
        FingerViewModel.RightMiddle,
        FingerViewModel.RightRing,
        FingerViewModel.RightPinky,
        FingerViewModel.LeftThumb,
        FingerViewModel.LeftIndex,
        FingerViewModel.LeftMiddle,
        FingerViewModel.LeftRing,
        FingerViewModel.LeftPinky
    ];

    public static IList<FingerViewModel> GetByIds(IList<string> ids)
    {
        return FingerViewModel.All.Where(x => ids.Contains(x.Id)).ToList();
    }

    #endregion

    #region Equality Comparer

    public override bool Equals(object obj)
    {
        var that = obj as FingerViewModel;

        if (that == null)
            return false;

        if (string.IsNullOrWhiteSpace(that.Id) && string.IsNullOrWhiteSpace(this.Id))
            return object.ReferenceEquals(that, this);

        return (that.Id == this.Id);
    }

    public override int GetHashCode()
    {
        return (!string.IsNullOrWhiteSpace(this.Id))
            ? this.Id.GetHashCode()
            : base.GetHashCode();
    }

    public static bool operator ==(FingerViewModel x, FingerViewModel y)
    {
        return Equals(x, y);
    }

    public static bool operator !=(FingerViewModel x, FingerViewModel y)
    {
        return !Equals(x, y);
    }

    #endregion
}