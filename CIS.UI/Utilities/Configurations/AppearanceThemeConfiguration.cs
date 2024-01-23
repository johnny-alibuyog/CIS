﻿using FirstFloor.ModernUI.Presentation;
using System;
using System.Xml.Serialization;

namespace CIS.UI.Utilities.Configurations;

[Serializable]
public class AppearanceThemeConfiguration
{
    public static readonly AppearanceThemeConfiguration Empty = new();

    private string _displayName;
    private string _relativeUri;

    public virtual string DisplayName
    {
        get { return _displayName; }
        set { _displayName = value; }
    }

    public virtual string RelativeUri
    {
        get { return _relativeUri; }
        set { _relativeUri = value; }
    }

    [XmlIgnore]
    public virtual Link Value
    {
        get
        {
            return new Link()
            {
                DisplayName = _displayName,
                Source = new Uri(_relativeUri, UriKind.Relative)
            };
        }
        set
        {
            _displayName = value.DisplayName;
            _relativeUri = value.Source.OriginalString;
        }
    }

    #region Equality Comparer

    public override bool Equals(object obj)
    {
        var that = obj as AppearanceThemeConfiguration;

        if (that == null)
            return false;

        if (that.DisplayName != this.DisplayName)
            return false;

        if (that.RelativeUri != this.RelativeUri)
            return false;

        return true;
    }

    public override int GetHashCode()
    {
        var hashCode = 17;
        hashCode = hashCode * 23 + (!string.IsNullOrWhiteSpace(this.DisplayName) ? this.DisplayName.GetHashCode() : 0);
        hashCode = hashCode * 23 + (!string.IsNullOrWhiteSpace(this.RelativeUri) ? this.RelativeUri.GetHashCode() : 0);

        return hashCode;
    }

    public static bool operator ==(AppearanceThemeConfiguration x, AppearanceThemeConfiguration y)
    {
        return Equals(x, y);
    }

    public static bool operator !=(AppearanceThemeConfiguration x, AppearanceThemeConfiguration y)
    {
        return !Equals(x, y);
    }

    #endregion
}
