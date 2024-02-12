using System;
using System.Collections.Generic;
using System.Linq;
using CIS.Core.Domain.Common;

namespace CIS.Core.Domain.Membership;

public class MemberEducationalAttainment : Entity<Guid>
{
    private Member _member;
    private readonly ICollection<Education> _educations = [];
    private readonly ICollection<Hobby> _hobbies = [];
    private readonly ICollection<Skill> _skills = [];

    public virtual Member Member
    {
        get => _member;
        protected set => _member = value;
    }

    public virtual IEnumerable<Education> Educations
    {
        get => _educations;
        protected set => SyncEducations(value);
    }

    public virtual IEnumerable<Hobby> Hobbies
    {
        get => _hobbies;
        protected set => SyncHobbies(value);
    }

    public virtual IEnumerable<Skill> Skills
    {
        get => _skills;
        protected set => SyncSkills(value);
    }

    private void SyncEducations(IEnumerable<Education> items)
    {
        var itemsToInsert = items.Except(_educations).ToList();
        var itemsToUpdate = _educations.Where(items.Contains).ToList();
        var itemsToRemove = _educations.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
        {
            _educations.Add(item);
        }

        // update
        foreach (var item in itemsToUpdate)
        {
            var value = items.Single(x => x == item);
            item.SerializeWith(value);
        }

        // delete
        foreach (var item in itemsToRemove)
        {
            _educations.Remove(item);
        }
    }

    private void SyncHobbies(IEnumerable<Hobby> items)
    {
        var itemsToInsert = items.Except(_hobbies).ToList();
        var itemsToUpdate = _hobbies.Where(items.Contains).ToList();
        var itemsToRemove = _hobbies.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
        {
            _hobbies.Add(item);
        }

        // update
        foreach (var item in itemsToUpdate)
        {
            var value = items.Single(x => x == item);
            item.SerializeWith(value);
        }

        // delete
        foreach (var item in itemsToRemove)
        {
            _hobbies.Remove(item);
        }
    }

    private void SyncSkills(IEnumerable<Skill> items)
    {
        var itemsToInsert = items.Except(_skills).ToList();
        var itemsToUpdate = _skills.Where(items.Contains).ToList();
        var itemsToRemove = _skills.Except(items).ToList();

        // insert
        foreach (var item in itemsToInsert)
        {
            _skills.Add(item);
        }

        // update
        foreach (var item in itemsToUpdate)
        {
            var value = items.Single(x => x == item);
            item.SerializeWith(value);
        }

        // delete
        foreach (var item in itemsToRemove)
        {
            _skills.Remove(item);
        }
    }   

    public virtual void WithMember(Member member)
    {
        this._member = member;
    }
}
