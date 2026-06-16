using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TagCloud
{
    private List<Tag> _tags = new();

    public TagCloud Add(Tag tag)
    {
        if (_tags.Contains(tag))
            _tags.Add(tag);
        return this;
    }

    public TagCloud RemoveAdd(Tag tag)
    {
        if (_tags.Contains(tag))
            _tags.Remove(tag);
        return this;
    }

    public bool Contains(Tag tag)
    {
        return _tags.Contains(tag);
    }
}

