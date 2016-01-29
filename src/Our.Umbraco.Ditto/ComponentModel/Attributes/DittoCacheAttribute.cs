﻿using System;

namespace Our.Umbraco.Ditto
{
    /// <summary>
    /// Represents cache configuration data in order to tell Ditto how to cache this property/class
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DittoCacheAttribute : DittoCacheableAttribute
    { }
}