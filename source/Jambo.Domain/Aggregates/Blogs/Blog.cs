﻿using Jambo.Domain.Aggregates.Blogs.Events;
using Jambo.Domain.Aggregates.Posts;
using Jambo.Domain.Exceptions;
using Jambo.Domain.ServiceBus;
using System;
using System.Collections.Generic;

namespace Jambo.Domain.Aggregates.Blogs
{
    public class Blog : AggregateRoot
    {
        public string Url { get; private set; }
        public int Rating { get; private set; }
        public bool Enabled { get; private set; }

        public void Start()
        {
            Apply(AddEvent(new BlogCreatedDomainEvent(Guid.NewGuid())));
        }

        public void UpdateUrl(string url)
        {
            Apply(AddEvent(new BlogUrlUpdatedDomainEvent(Id, Version, url)));
        }

        public void Enable()
        {
            Apply(AddEvent(new BlogEnabledDomainEvent(Id, Version)));
        }

        public void Disable()
        {
            Apply(AddEvent(new BlogDisabledDomainEvent(Id, Version)));
        }

        public void Apply(BlogCreatedDomainEvent @event)
        {
            Id = @event.AggregateRootId;
        }

        public void Apply(BlogUrlUpdatedDomainEvent @event)
        {
            Url = @event.Url;
        }

        public void Apply(BlogDisabledDomainEvent @event)
        {
            if (Enabled == false)
            {
                throw new BlogDomainException("The blog is already disabled.");
            }

            Enabled = false;
        }

        public void Apply(BlogEnabledDomainEvent @event)
        {
            if (Enabled == true)
            {
                throw new BlogDomainException("The blog is already enabled.");
            }

            Enabled = true;
        }
    }
}
