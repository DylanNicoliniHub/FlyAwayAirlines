﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
    public interface ICommandHandlerRegistry
    {
        void Register(ICommandHandler handler);
    }
}
