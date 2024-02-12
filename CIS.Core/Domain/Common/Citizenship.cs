using System;
using System.Collections.Generic;

namespace CIS.Core.Domain.Common;

public class Citizenship : Entity<string>
{
    private string _description;

    public virtual string Descrition
    {
        get => _description;
        set => _description = value;
    }

    public override string ToString()
    {
        return Descrition;
    }

    public Citizenship() { }

    public Citizenship(string id, string description)
    {
        Id = id;
        Descrition = description;
    }

    public static readonly Citizenship Dual = new("Dual", "A status where an individual is recognized as a citizen of two countries simultaneously."); 
    public static readonly Citizenship Filipino = new("Filipino", "Citizenship status for individuals legally recognized as citizens of the Philippines.");
    public static readonly Citizenship Former = new("Former", "Describes someone who once held citizenship in a country but no longer does, for various reasons.");
    public static readonly Citizenship Foreign = new("Foreign", "The status of being a citizen of a country other than the one in which one is currently residing.");
    public static readonly Citizenship Naturalized = new("Naturalized", "Citizenship granted to a foreign national after meeting certain requirements of the host country.");
    public static readonly Citizenship Stateless = new("Stateless", "A condition where an individual does not hold citizenship in any country.");
    public static readonly Citizenship Unknown = new("Unknown", "A term used when an individual's citizenship status is not known or cannot be determined.");
    public static readonly ICollection<Citizenship> List = [ Dual, Filipino, Former, Foreign, Naturalized, Stateless, Unknown ];
}
