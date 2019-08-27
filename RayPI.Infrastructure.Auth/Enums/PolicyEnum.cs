﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RayPI.Infrastructure.Auth.Enums
{
    public enum PolicyEnum
    {
        RequireRoleOfClient,
        RequireRoleOfAdmin,
        RequireRoleOfAdminOrClient
    }
}